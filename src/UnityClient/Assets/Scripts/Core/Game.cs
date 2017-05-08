namespace JYW.ThesisMMO.UnityClient.Core {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using ExitGames.Client.Photon;
    using JYW.ThesisMMO.UnityClient.Util;
    using JYW.ThesisMMO.UnityClient.Core.Photon;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Responses;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.Common.Codes;
    using Protocol = JYW.ThesisMMO.Common.Types.Protocol;

    public class Game : MonobehaviourSingleton<Game> {
        private PhotonPeer m_PhotonPeer;
        private ServerPeerListener m_ServerPeerListener;

        private RequestForwarder m_RequestForwarder;
        private ResponseOperations m_ResponseForwarder;
        private EventForwarder m_EventForwarder;

        private const string m_ApplicationName = "MMOServer";
        public bool Connected { get; private set; }
        private Action m_ConnectedCallback;

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

        private void SendOperation(OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId) {
            m_PhotonPeer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }
    }
}