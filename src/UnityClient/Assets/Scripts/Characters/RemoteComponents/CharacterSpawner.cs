using UnityEngine;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

    /// <summary>  
    ///  Creates and destroys remote characters.
    /// </summary>  
    public class CharacterSpawner : MonoBehaviour {

        [SerializeField] private RemoteCharacter m_RemoteCharacterPrefab;

        private Dictionary<string, RemoteCharacter> m_RemoteCharacters = new Dictionary<string, RemoteCharacter>();

        private void Awake() {
            EventOperations.NewPlayerEvent += CreateRemoteCharacter;
            EventOperations.RemovePlayerEvent += RemoveRemoteCharacter;
        }

        private void CreateRemoteCharacter(string name, Vector3 position, int health, int maxHealth) {
            if (m_RemoteCharacters.ContainsKey(name) == false) {
                var character = Instantiate(m_RemoteCharacterPrefab, position, Quaternion.identity);
                character.Initialize(name, health, maxHealth);
                m_RemoteCharacters.Add(name, character);
            }
        }

        private void RemoveRemoteCharacter(string name) {
            if(m_RemoteCharacters.ContainsKey(name) == false) { return; }

            var character = m_RemoteCharacters[name];
            character.SendMessage("OnDeath");
            m_RemoteCharacters.Remove(name);
        }
    }
}
