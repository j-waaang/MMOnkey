namespace JYW.ThesisMMO.UnityClient.CharacterSelection {

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Core.MessageHandling.Requests;
    using Core.MessageHandling.Responses;

    public class EnterWorldController : MonoBehaviour {
        
        [SerializeField] CharacterCreationController m_CharacterCreationController;

        /// <summary>  
        ///  Called by button click.
        /// </summary>  
        public void OnEnterWorldClick() {
            var characterSettings = m_CharacterCreationController.GetCharacterSetting();
            if (characterSettings == null) {
                Debug.LogError("No username entered");
                return;
            }

            RequestOperations.EnterWorldRequest(characterSettings);
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
