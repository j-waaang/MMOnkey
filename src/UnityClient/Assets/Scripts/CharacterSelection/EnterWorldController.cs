namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.CharacterSelection {

    using UnityEngine;
    using Networking;

    public class EnterWorldController : MonoBehaviour {

        public void OnEnterWorld() {
            Game.Instance.EnterWorld(EnteredWorldCallback);
        }

        private void EnteredWorldCallback(Vector2 position) {
            Debug.Log("Entered world at " + position);
        } 
    }
}
