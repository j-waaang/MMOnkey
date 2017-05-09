namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.CharacterSelection {

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Core.MessageHandling.Requests;
    using Core.MessageHandling.Responses;

    public class EnterWorldController : MonoBehaviour {
        /// <summary>  
        ///  Set by textinput inspector.
        /// </summary>  
        public string Username { get; set; }

        /// <summary>  
        ///  Called by button click.
        /// </summary>  
        public void OnEnterWorldClick() {
            if(Username == "") {
                Debug.LogError("No username entered");
                return;
            }
            RequestOperations.EnterWorldRequest(Username);
            ResponseOperations.EnterWorldEvent += OnEnteredWorldResponse;
        }

        /// <summary>  
        ///  Callback on response.
        /// </summary>  
        private void OnEnteredWorldResponse(Vector2 position) {
            ResponseOperations.EnterWorldEvent -= OnEnteredWorldResponse;
            SceneManager.LoadScene("World");
        }
    }
}
