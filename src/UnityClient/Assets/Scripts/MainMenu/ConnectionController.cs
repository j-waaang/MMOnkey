namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.MainMenu {
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Core;

    /// <summary>  
    ///  Initiates the connection to the server.
    /// </summary>  
    public class ConnectionController : MonoBehaviour {

        /// <summary>  
        ///  Set by input field.
        /// </summary> 
        public string ConnectionAddress { get; set; }

        public void OnConnect() {
            CheckConnectionAddress();
            Game.Instance.Connect(ConnectionAddress, OnConnected);
        }

        private void CheckConnectionAddress() {
            if (!string.IsNullOrEmpty(ConnectionAddress)) { return; }

            ConnectionAddress = "localhost:5055";
            Debug.Log("No connection address entered. Using localhost:5055");
        }

        private void OnConnected() {
            SceneManager.LoadScene("CharacterSelection");
        }
    }
}
