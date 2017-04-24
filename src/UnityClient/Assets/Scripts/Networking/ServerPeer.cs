
namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking {
    using System;
    using UnityEngine;
    using ExitGames.Client.Photon;

    public class ServerPeer : MonoBehaviour {

        private PhotonPeer m_PhotonPeer;
        private ServerPeerListener m_ServerPeerListener;
        private const string m_ApplicationName = "MMOServer";
        private bool m_Connected = false;
        private Action m_ConnectedCallback;

        private void Awake() {
            DontDestroyOnLoad(gameObject);

            m_ServerPeerListener = new ServerPeerListener();
            m_ServerPeerListener.ConnectedEvent += Connected;
            m_ServerPeerListener.DisconnectedEvent += Disconnected;

            m_PhotonPeer = new PhotonPeer(m_ServerPeerListener, ConnectionProtocol.Tcp);
        }

        private void Update() {
            m_PhotonPeer.Service();
        }

        public void Connect(string serverAddress, Action connectedCallback) {
            m_ConnectedCallback = connectedCallback;
            m_PhotonPeer.Connect(serverAddress, m_ApplicationName);
            Debug.Log("Connecting to server at " + serverAddress);
        }

        private void Connected() {
            m_Connected = true;
            m_ConnectedCallback();
            m_ConnectedCallback = null;
        }

        private void Disconnect() {
            m_PhotonPeer.Disconnect();
            m_Connected = false;
        }

        private void Disconnected() {
            m_Connected = false;
        }

        private void OnDestroy() {
            Disconnect();
            m_ServerPeerListener.ConnectedEvent -= Connected;
            m_ServerPeerListener.DisconnectedEvent -= Disconnected;
        }
    }
}