namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.MainMenu {
    using UnityEngine;
    using UnityEngine.UI;
    using Networking;

        /// <summary>  
        ///  Listen to onClick of the connect button and takes information from the input field and hands it to the connector.
        ///  
        ///  TODO: Let controller create server peer?
        /// </summary>  
        public class ConnectionController : MonoBehaviour {

        [SerializeField] private Button m_ConnectButton;
        [SerializeField] private Text m_ConnectInput;
        [SerializeField] private ServerPeer m_ServerPeer;

        private void Awake() {
            m_ConnectButton.onClick.AddListener(OnConnect);
        }

        private void OnConnect() {
            m_ServerPeer.Connect(m_ConnectInput.text);
        }
    }
}
