namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Player {

    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

    /// <summary>  
    ///  Changes the position of the client character on input and updates the server representation.
    /// </summary>  
    public class PositionController : MonoBehaviour {
        private float m_MovementSpeed = 0.2f;
        private Rigidbody2D m_Rigidbody;

        public Vector2 inputVector;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            inputVector = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical"));
            inputVector = inputVector.normalized;
            m_Rigidbody.position += inputVector * m_MovementSpeed;
            UpdateNetworkPosition();
        }

        private void UpdateNetworkPosition() {
            RequestOperations.MoveRequest(m_Rigidbody.position);
        }
    }
}
