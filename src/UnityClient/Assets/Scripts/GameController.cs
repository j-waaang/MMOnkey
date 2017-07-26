using UnityEngine;
using UnityEngine.SceneManagement;

namespace JYW.ThesisMMO.UnityClient {

    using Core.MessageHandling.Events;

    public sealed class GameController : MonoBehaviour {

        [SerializeField] private GameObject m_Player;
        
        private void Start() {
            EventOperations.EntityDeathEvent += OnEntityDeath;
            EventOperations.FireEnqueuedEvents();
        }

        private void OnEntityDeath(string name) {
            if(name != GameData.characterSetting.Name) { return; }

            Debug.Log("Player died");
            m_Player.SendMessage("OnDeath");
        }
    }
}