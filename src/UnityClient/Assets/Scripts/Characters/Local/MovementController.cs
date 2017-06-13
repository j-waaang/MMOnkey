namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Local {

    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;

    using JYW.ThesisMMO.Common.Entities;
    using JYW.ThesisMMO.UnityClient.Characters;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

    /// <summary>  
    ///  Changes the position of the client character on input and updates the server representation.
    /// </summary>  
    public class MovementController : MonoBehaviour {

        private RemoteMovementSpeedComponent m_MovementSpeed;

        private CharacterAnimationController m_CharacterAnimationController;
        private Vector3 m_LastSendVector;
        private float m_LastSendTime = 0;
        private float m_MinMovDistance = 0.15f;
        private const float m_SendRateInSeconds = 0.25f;

        private void Awake() {
            m_MovementSpeed = GetComponent<RemoteMovementSpeedComponent>();
            m_CharacterAnimationController = GetComponent<CharacterAnimationController>();
        }

        private void FixedUpdate() {
            UpdatePosition();
            NotifyAboutPositionUpdate();
        }

        /// <summary>  
        ///  Changes the position depending on the input.
        /// </summary>  
        private void UpdatePosition() {
            var inputVector = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"),0 , CrossPlatformInputManager.GetAxisRaw("Vertical"));

            if (inputVector == Vector3.zero) {
                MovementState = MovementState.Idle;
                return;
            }

            inputVector = inputVector.normalized;
            transform.position += inputVector * m_MovementSpeed.MovementSpeed;
            transform.forward = inputVector;
            //UpdateRotation(inputVector);
            MovementState = MovementState.Moving;
        }

        //private void UpdateRotation(Vector2 lookDirection) {
        //    float rot_z = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        //}

        /// <summary>  
        ///  Actors which are interest in the position change are notified.
        /// </summary>  
        private void NotifyAboutPositionUpdate() {
            //GameData.ClientCharacterPosition = transform.position;
            UpdateNetworkPosition();
        }

        /// <summary>  
        ///  Send new position to the server.
        /// </summary> 
        private void UpdateNetworkPosition() {
            if (!PositionPropagationCondition()) { return; }
            m_LastSendTime = Time.time;
            m_LastSendVector = transform.position;
            RequestOperations.MoveRequest(transform.position);
        }

        /// <summary>  
        ///  Time and distance based.
        /// </summary>  
        private bool PositionPropagationCondition() {
            var distance = Vector3.Distance(transform.position, m_LastSendVector);

            return
                (Time.time - m_LastSendTime > m_SendRateInSeconds) &&
                (distance > m_MinMovDistance);
        }

        private MovementState m_MovementState;
        private MovementState MovementState {
            get {
                return m_MovementState;
            }

            set {
                if(value == m_MovementState) { return; }

                m_MovementState = value;
                m_CharacterAnimationController.UpdateRunningAnimation(value);
            }
        }
    }
}
