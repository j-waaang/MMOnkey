namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using UnityEngine;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

    public class CharacterSpawner : MonoBehaviour {

        [SerializeField] RemoteCharacterController m_RemoteCharacterPrefab;

        private void Awake() {
            EventOperations.NewPlayerEvent += SpawnCharacter;
        }

        private void SpawnCharacter(string username, Vector2 position) {
            var character = Instantiate(m_RemoteCharacterPrefab, position, Quaternion.identity);
            character.Initialize(username);
        }
    }
}
