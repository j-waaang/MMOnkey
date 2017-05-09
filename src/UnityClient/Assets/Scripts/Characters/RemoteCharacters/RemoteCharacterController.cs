namespace JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters {

    using UnityEngine;

    public class RemoteCharacterController : MonoBehaviour {

        private string m_Username;
        
        internal void Initialize(string username) {
            m_Username = username;
        }
    }
}