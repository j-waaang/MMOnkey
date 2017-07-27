using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Local {

    using JYW.ThesisMMO.Common.Entities;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

    /// <summary>  
    ///  Changes the position of the client character on input and updates the server representation.
    /// </summary>  
    public class LocalPositionController : MonoBehaviour {

        private RemoteMovementSpeedComponent m_MovementSpeed;
        private MovementStateComponent m_MovementState;
        private RotationController m_RotationController;

        private Vector3 m_LastSendVector;
        private float m_LastSendTime = 0;
        private float m_MinMovDistance = 0.15f;
        private const float m_SendRateInSeconds = 0.033f;
        private bool m_CanMove = true;

        private void Awake() {
            m_RotationController = GetComponent<RotationController>();
            m_MovementSpeed = GetComponent<RemoteMovementSpeedComponent>();
            m_MovementState = GetComponent<MovementStateComponent>();
        }

        private void Update() {
            if (!m_CanMove) { return; }
            UpdatePosition();
            UpdateNetworkPosition();
        }

        /// <summary>  
        ///  Changes the position depending on the input.
        /// </summary>  
        private void UpdatePosition() {
            var inputVector = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));

            if (inputVector == Vector3.zero) {
                m_MovementState.MovementState = MovementState.Idle;
                return;
            }

            inputVector = inputVector.normalized;
            transform.position += inputVector * m_MovementSpeed.MovementSpeed * Time.deltaTime;

            m_RotationController.LookAt(inputVector);
            m_MovementState.MovementState = MovementState.Moving;
        }

        /// <summary>  
        ///  Send new position to the server.
        /// </summary> 
        private void UpdateNetworkPosition() {
            if (!IsWorthNotifiyingServer()) { return; }
            m_LastSendTime = Time.time;
            m_LastSendVector = transform.position;
            RequestOperations.MoveRequest(transform.position);
        }

        /// <summary>  
        ///  Time and distance checks.
        /// </summary>  
        private bool IsWorthNotifiyingServer() {
            var distance = Vector3.Distance(transform.position, m_LastSendVector);

            return
                (Time.time - m_LastSendTime > m_SendRateInSeconds) &&
                (distance > m_MinMovDistance);
        }

        private void OnDeath() {
            m_CanMove = false;
        }
    }
}
