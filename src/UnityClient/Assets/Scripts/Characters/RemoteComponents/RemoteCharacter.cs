namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using UnityEngine;

    /// <summary>  
    ///  Character controller through events from the server.
    /// </summary>  
    public class RemoteCharacter : MonoBehaviour {

        /// <summary>  
        ///  Subscribe to server events.
        /// </summary>  
        internal void Initialize(string name, int health, int maxHealth) {
            SetName(name);
            GetComponent<HealthComponent>().Initialize(health, maxHealth);
        }

        private void SetName(string name) {
            gameObject.name = name;
            transform.GetChild(0).name = name; // Easier to differentiate debug messages
        }

        private void OnDeath() {
            if(GameData.Target == gameObject) { GameData.Target = null; }
            Destroy(GetComponent<Collider>());
        }
    }
}