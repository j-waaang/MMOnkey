namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Player {
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;
    public class PositionController : MonoBehaviour {
        [SerializeField] private float m_MovementSpeed = 0.5f;
        private Rigidbody2D m_Rigidbody;

        public Vector2 inputVector;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            inputVector = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical"));
            inputVector = inputVector.normalized;
            m_Rigidbody.position += inputVector * m_MovementSpeed;
        }
    }
}
