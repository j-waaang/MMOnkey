namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.MainMenu {
    using UnityEngine;
    public class ApplicationBackgroundBehaviour : MonoBehaviour {
        private void Awake() {
            Application.runInBackground = true;
        }
    }
}
