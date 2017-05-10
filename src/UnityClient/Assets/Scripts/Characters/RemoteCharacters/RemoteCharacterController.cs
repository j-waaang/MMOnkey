namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using UnityEngine;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

    /// <summary>  
    ///  Character controller through events from the server.
    /// </summary>  
    public class RemoteCharacterController : MonoBehaviour {

        private string m_Username;
        private bool m_Initialized;

        /// <summary>  
        ///  Subscribe to server events.
        /// </summary>  
        internal void Initialize(string username) {
            m_Username = username;
            EventOperations.MoveEvent += OnMoveEvent;
            m_Initialized = true;
        }

        /// <summary>  
        ///  Listen to the servers events and moves on event.
        /// </summary>  
        private void OnMoveEvent(string username, Vector2 position) {
            if (!m_Initialized) { return; }

            if (m_Username != username) { return; }

            transform.position = position;
        }

        /// <summary>  
        ///  Unsubscribe to server events.
        /// </summary>  
        private void OnDestroy() {
            EventOperations.MoveEvent -= OnMoveEvent;
        }
    }
}