namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.CharacterSelection {

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Networking;

    public class EnterWorldController : MonoBehaviour {

        //Set by textinput inspector
        public string Username { get; set; }

        public void OnEnterWorld() {
            if(Username == "") {
                Debug.LogError("No username entered");
                return;
            }
            Game.Instance.EnterWorld(Username, EnteredWorldCallback);
        }

        private void EnteredWorldCallback(Vector2 position) {
            Debug.Log("Entered world at " + position);
            SceneManager.LoadScene("World");
        } 
    }
}
