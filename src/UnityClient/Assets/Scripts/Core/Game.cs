namespace JYW.ThesisMMO.UnityClient.Core {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using ExitGames.Client.Photon;
    using JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking;
    using JYW.ThesisMMO.UnityClient.Util;
    using JYW.ThesisMMO.UnityClient.Core.Photon;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.Common.Codes;
    using Protocol = JYW.ThesisMMO.Common.Types.Protocol;
    using Operations = JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking.Operations;

    public class Game : MonobehaviourSingleton<Game> {
        private PhotonPeer m_PhotonPeer;
        private ServerPeerListener m_ServerPeerListener;

        private RequestForwarder m_RequestForwarder;
        private ResponseForwarder m_ResponseForwarder;
        private EventForwarder m_EventForwarder;

        private const string m_ApplicationName = "MMOServer";
        public bool Connected { get; private set; }
        private Action m_ConnectedCallback;
        private Action<Vector2> m_EnteredWorldCallback;

        private void Awake() {
            DontDestroyOnLoad(gameObject);

            m_ServerPeerListener = new ServerPeerListener();
            m_ServerPeerListener.ConnectedEvent += OnConnected;
            m_ServerPeerListener.DisconnectedEvent += OnDisconnected;

            m_PhotonPeer = new PhotonPeer(m_ServerPeerListener, ConnectionProtocol.Udp);

            RegisterTypes();
        }

        private void RegisterTypes() {
            PhotonPeer.RegisterType(typeof(Vector), (byte)Protocol.CustomTypeCodes.Vector, Protocol.SerializeVector, Protocol.DeserializeVector);
        }

        private void Update() {
            m_PhotonPeer.Service();
        }


        public void Connect(string serverAddress, Action connectedCallback) {
            m_ConnectedCallback = connectedCallback;
            m_PhotonPeer.Connect(serverAddress, m_ApplicationName);
            Debug.Log("Connecting to server at " + serverAddress);
        }

        private void OnConnected() {
            Connected = true;
            m_ConnectedCallback();
            m_ConnectedCallback = null;
            CreateMessageForwarders();
        }

        private void CreateMessageForwarders() {
            m_RequestForwarder = new RequestForwarder(m_PhotonPeer);
        }

        private void Disconnect() {
            m_PhotonPeer.Disconnect();
            Connected = false;
        }

        private void OnDisconnected() {
            Connected = false;
        }

        protected override void OnDestroy() {
            Disconnect();
            m_ServerPeerListener.ConnectedEvent -= OnConnected;
            m_ServerPeerListener.DisconnectedEvent -= OnDisconnected;
            base.OnDestroy();
        }

        internal void EnterWorld(string username,Action<Vector2> callback) {
            m_EnteredWorldCallback = callback;
            Operations.EnterWorld(SendOperation, username);
            m_ServerPeerListener.EnterWorldEvent += OnEnteredWorld;
        }

        internal void OnEnteredWorld(Vector2 position) {
            m_EnteredWorldCallback(position);
            m_EnteredWorldCallback = null;
        }

        private void SendOperation(OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId) {
            m_PhotonPeer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }
    }
}