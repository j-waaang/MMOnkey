namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using ExitGames.Client.Photon;
    using Util;
    using Common;

    public class Game : Singleton<Game> {

        private PhotonPeer m_PhotonPeer;

        private ServerPeerListener m_ServerPeerListener;
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

        internal void EnterWorld(Action<Vector2> callback) {
            m_EnteredWorldCallback = callback;
            Operations.EnterWorld(SendOperation, "Yolo");
        }

        internal void OnEnteredWorld() {
            m_EnteredWorldCallback(Vector2.zero);
            m_EnteredWorldCallback = null;
        }

        private void SendOperation(OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId) {
            m_PhotonPeer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }
    }
}