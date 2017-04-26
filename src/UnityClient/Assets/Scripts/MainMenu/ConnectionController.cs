namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.MainMenu {
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using Networking;

        /// <summary>  
        ///  1. Listen to onClick of the connect button and takes information from the input field and hands it to the connector.
        ///  2. Gets callback from server peer and loads character creation scene.
        ///  
        ///  TODO: Let controller create server peer?
        /// </summary>  
        public class ConnectionController : MonoBehaviour {

        [SerializeField] private Button m_ConnectButton;
        [SerializeField] private Text m_ConnectInput;

        private void Awake() {
            m_ConnectButton.onClick.AddListener(OnConnect);
        }

        private void OnConnect() {
            Game.Instance.Connect(m_ConnectInput.text, OnConnected);
        }

        private void OnConnected() {
            SceneManager.LoadScene("CharacterSelection");
        }
    }
}
