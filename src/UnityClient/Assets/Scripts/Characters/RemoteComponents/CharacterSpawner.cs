using UnityEngine;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
    using Util;

    /// <summary>  
    ///  Creates and destroys remote characters.
    /// </summary>  
    public class CharacterSpawner : MonobehaviourSingleton<CharacterSpawner> {

        [SerializeField] private RemoteCharacter m_RemoteCharacterPrefab;

        public static Dictionary<string, RemoteCharacter> RemoteCharacters { get; private set; } 

        protected CharacterSpawner() { }

        private void Awake() {
            EventOperations.NewPlayerEvent += CreateRemoteCharacter;
            EventOperations.RemovePlayerEvent += RemoveRemoteCharacter;
            RemoteCharacters = new Dictionary<string, RemoteCharacter>();
        }

        private void CreateRemoteCharacter(string name, Vector3 position, int health, int maxHealth) {
            if (RemoteCharacters.ContainsKey(name) == false) {
                var character = Instantiate(m_RemoteCharacterPrefab, position, Quaternion.identity);
                character.Initialize(name, health, maxHealth);
                RemoteCharacters.Add(name, character);
            }
        }

        private void RemoveRemoteCharacter(string name) {
            if(RemoteCharacters.ContainsKey(name) == false) { return; }

            var character = RemoteCharacters[name];
            character.SendMessage("OnDeath");
            RemoteCharacters.Remove(name);
        }
    }
}
