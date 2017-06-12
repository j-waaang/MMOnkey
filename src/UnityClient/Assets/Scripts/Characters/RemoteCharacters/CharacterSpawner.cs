namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using UnityEngine;
    using System.Collections.Generic;

    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

    /// <summary>  
    ///  Creates and destroys remote characters.
    /// </summary>  
    public class CharacterSpawner : MonoBehaviour {

        [SerializeField] private RemoteCharacter m_RemoteCharacterPrefab;

        private Dictionary<string, GameObject> m_RemoteCharacters = new Dictionary<string, GameObject>();

        private void Awake() {
            EventOperations.NewPlayerEvent += CreateRemoteCharacter;
            EventOperations.RemovePlayerEvent += RemoveRemoteCharacter;
        }

        private void CreateRemoteCharacter(string name, Vector2 position, int health, int maxHealth) {
            if (m_RemoteCharacters.ContainsKey(name) == false) {
                var character = Instantiate(m_RemoteCharacterPrefab, position, Quaternion.identity);
                character.Initialize(name, health, maxHealth);
                m_RemoteCharacters.Add(name, character.gameObject);
            }
        }

        private void RemoveRemoteCharacter(string name) {
            var character = m_RemoteCharacters[name];
            Destroy(character);
            m_RemoteCharacters.Remove(name);
        }
    }
}
