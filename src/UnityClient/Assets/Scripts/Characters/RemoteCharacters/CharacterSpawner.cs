namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using UnityEngine;
    using System.Collections.Generic;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

    /// <summary>  
    ///  Listen to move events from the server.
    ///  Creates or destroys characters if needed.
    /// </summary>  
    public class CharacterSpawner : MonoBehaviour {

        [SerializeField]
        private RemoteCharacterController m_RemoteCharacterPrefab;

        private Dictionary<string, GameObject> m_RemoteCharacters = new Dictionary<string, GameObject>();

        private void Awake() {
            EventOperations.MoveEvent += OnMoveEvent;
            GameData.ClientCharacterPositionChange += OnClientCharacterPositionChange;
        }

        private void OnMoveEvent(string username, Vector2 position) {
            if (m_RemoteCharacters.ContainsKey(username) == false) {
                AddEntity(username, position);
            }
            else if (!PositionInsideInterestArea(position)){
                DestroyCharacter(username);
            }
        }

        private static bool PositionInsideInterestArea(Vector2 position) {
            var distanceToClientCharacter = Vector2.Distance(GameData.ClientCharacterPosition, position);
            return distanceToClientCharacter < GameData.InterestDistance;
        }

        private void AddEntity(string username, Vector2 position) {
            var character = Instantiate(m_RemoteCharacterPrefab, position, Quaternion.identity);
            character.Initialize(username);
            m_RemoteCharacters.Add(username, character.gameObject);
        }

        private void OnClientCharacterPositionChange(Vector2 position) {
            var removals = new List<string>();

            foreach(var character in m_RemoteCharacters) {
                if (!PositionInsideInterestArea(character.Value.transform.position)) {
                    removals.Add(character.Key);
                }
            }

            foreach(var removal in removals) {
                DestroyCharacter(removal);
            }
        }

        private void DestroyCharacter(string name) {
            Destroy(m_RemoteCharacters[name]);
            m_RemoteCharacters.Remove(name);
        }
    }
}
