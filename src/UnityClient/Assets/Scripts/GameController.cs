using UnityEngine;
using UnityEngine.SceneManagement;

namespace JYW.ThesisMMO.UnityClient {

    using Core.MessageHandling.Events;

    public sealed class GameController : MonoBehaviour {
        
        private void Start() {
            EventOperations.EntityDeathEvent += OnEntityDeath;
            EventOperations.FireEnqueuedEvents();
        }

        private void OnEntityDeath(string name) {
            if(name != GameData.characterSetting.Name) { return; }
            SceneManager.LoadScene(0);
        }
    }
}