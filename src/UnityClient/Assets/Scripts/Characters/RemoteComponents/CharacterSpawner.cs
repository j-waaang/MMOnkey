using UnityEngine;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
    using Util;

    /// <summary>  
    ///  Creates and destroys remote characters.
    /// </summary>  
    public class CharacterSpawner : MonobehaviourSingleton<CharacterSpawner> {

        [SerializeField]
        private RemoteCharacter m_RemoteCharacterPrefab;

        private readonly Dictionary<string, GameObject> m_RemoteCharacters = new Dictionary<string, GameObject>();

        protected CharacterSpawner() { }

        private void Awake() {
            EventOperations.NewPlayerEvent += CreateRemoteCharacter;
            EventOperations.EntityExitRegionEvent += OnEntityExitRegionEvent;
            EventOperations.EntityDeathEvent += OnEntityDeathEvent;
        }

        private void CreateRemoteCharacter(string name, Vector3 position, int health, int maxHealth) {
            Debug.Assert(m_RemoteCharacters.ContainsKey(name) == false, string.Format("Cannot create {0} because it already/still exists.", name));
            var character = Instantiate(m_RemoteCharacterPrefab, position, Quaternion.identity);
            character.Initialize(name, health, maxHealth);
            m_RemoteCharacters.Add(name, character.gameObject);
        }

        private void OnEntityExitRegionEvent(string name) {
            Debug.Assert(m_RemoteCharacters.ContainsKey(name) == true, string.Format("Cannot remove {0} because it doesn't exist.", name));
            Destroy(m_RemoteCharacters[name]);
            m_RemoteCharacters.Remove(name);
        }

        private void OnEntityDeathEvent(string name) {
            Debug.Assert(m_RemoteCharacters.ContainsKey(name) == true, string.Format("Cannot remove {0} because it doesn't exist.", name));
            var character = m_RemoteCharacters[name];
            character.SendMessage("OnDeath");
            m_RemoteCharacters.Remove(name);
        }
    }
}
