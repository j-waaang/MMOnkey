using UnityEngine;

namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking {
    using ExitGames.Client.Photon;

    public class ServerPeer : MonoBehaviour {

        private PhotonPeer m_PhotonPeer;
        private ServerPeerListener m_ServerPeerListener;
        private const string m_ApplicationName = "MMOServer";
        private bool m_Connected = false;

        private void Awake() {
            m_ServerPeerListener = new ServerPeerListener();
            m_PhotonPeer = new PhotonPeer(m_ServerPeerListener, ConnectionProtocol.Tcp);
            m_ServerPeerListener.ConnectedEvent += Connected;
            m_ServerPeerListener.DisconnectedEvent -= Disconnected;
        }

        private void Update() {
            if(!m_Connected) { return; }

            m_PhotonPeer.Service();
        }

        public void Connect(string serverAddress) {
            m_PhotonPeer.Connect(serverAddress, m_ApplicationName);
            Debug.Log("Connecting to server at " + serverAddress);
            DontDestroyOnLoad(gameObject);
        }

        private void Connected() {
            m_Connected = true;
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
            m_ServerPeerListener.ConnectedEvent += Connected;
            m_ServerPeerListener.DisconnectedEvent -= Disconnected;
        }
    }
}