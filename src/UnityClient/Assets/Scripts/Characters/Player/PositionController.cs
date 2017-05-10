namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Player {

    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

    /// <summary>  
    ///  Changes the position of the client character on input and updates the server representation.
    /// </summary>  
    public class PositionController : MonoBehaviour {

        private Rigidbody2D m_Rigidbody;
        private float m_MovementSpeed = 0.2f;

        private Vector2 m_LastSendVector;
        private float m_LastSendTime = 0;
        private float m_MinMovDistance = 0.25f;
        private const float m_SendRateInSeconds = 0.25f;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            UpdatePosition();
            NotifyAboutPositionUpdate();
        }

        /// <summary>  
        ///  Changes the position depending on the input.
        /// </summary>  
        private void UpdatePosition() {
            Vector2 inputVector = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical"));
            inputVector = inputVector.normalized;
            m_Rigidbody.position += inputVector * m_MovementSpeed;
        }

        /// <summary>  
        ///  Actors which are interest in the position change are notified.
        /// </summary>  
        private void NotifyAboutPositionUpdate() {
            GameData.ClientCharacterPosition = m_Rigidbody.position;
            UpdateNetworkPosition();
        }

        /// <summary>  
        ///  Send new position to the server.
        /// </summary> 
        private void UpdateNetworkPosition() {
            if(!PositionPropagationCondition()) { return; }
            m_LastSendTime = Time.time;
            m_LastSendVector = m_Rigidbody.position;
            RequestOperations.MoveRequest(m_Rigidbody.position);
        }

        /// <summary>  
        ///  Time and distance based.
        /// </summary>  
        private bool PositionPropagationCondition() {
            var distance = Vector2.Distance(m_Rigidbody.position, m_LastSendVector);

            return
                (Time.time - m_LastSendTime > m_SendRateInSeconds) &&
                (distance > m_MinMovDistance);
        }
    }
}
