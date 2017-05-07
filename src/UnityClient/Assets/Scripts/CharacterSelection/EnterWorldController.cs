namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.CharacterSelection {

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Core.MessageHandling;

    public class EnterWorldController : MonoBehaviour {
        /// <summary>  
        ///  Set by textinput inspector.
        /// </summary>  
        public string Username { get; set; }

        public void OnEnterWorld() {
            if(Username == "") {
                Debug.LogError("No username entered");
                return;
            }
            RequestOperations.EnterWorld(Username);
            //Game.Instance.EnterWorld(Username, EnteredWorldCallback);
        }

        //private void EnteredWorldCallback(Vector2 position) {
        //    Debug.Log("Entered world at " + position);
        //    SceneManager.LoadScene("World");
        //} 
    }
}
