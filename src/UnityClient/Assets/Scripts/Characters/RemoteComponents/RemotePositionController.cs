using JYW.ThesisMMO.Common.Entities;
using JYW.ThesisMMO.UnityClient.Core;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;

public class RemotePositionController : MonoBehaviour {

    private const float m_AutoIdle = 0.3f;

    private MovementStateComponent m_MovementState;
    private RotationController m_RotationController;
    private Vector3 m_LastReceivedPosition;
    private float m_LastReceivedTime;

    private Vector3 m_PredictedPos;

    private void Start() {
        m_MovementState = GetComponent<MovementStateComponent>();
        m_RotationController = GetComponent<RotationController>();
        EventOperations.MoveEvent += OnMoveEvent;
    }

    private void Update() {
        if(m_MovementState.MovementState == MovementState.Idle) { return; }

        if(Time.time - m_LastReceivedTime > m_AutoIdle) {
            m_MovementState.MovementState = MovementState.Idle;
            // Undo prediction
            transform.position = m_LastReceivedPosition;
        }

        //Debug.LogFormat("Ping: {0}", Game.Instance.GetRTT());
    }

    /// <summary>  
    ///  Listen to the servers events and moves on event.
    /// </summary>  
    private void OnMoveEvent(string name, Vector3 receivedPosition) {
        if (gameObject.name != name) { return; }

        //var predictedPosition = PredictPosition(receivedPosition);
        var predictedPosition = receivedPosition;

        UpdateRotation(predictedPosition);

        m_MovementState.MovementState = MovementState.Moving;
        transform.position = predictedPosition;
        m_LastReceivedPosition = receivedPosition;
        m_LastReceivedTime = Time.time;
    }

    private Vector3 PredictPosition(Vector3 receivedPosition) {
        var moveDirection = receivedPosition - m_LastReceivedPosition;
        return receivedPosition + moveDirection * 0.001f * Game.Instance.GetRTT() * 0.5f;
    }

    private void UpdateRotation(Vector3 newPosition) {
        m_RotationController.LookAt(newPosition - transform.position);
    }

    private void OnDestroy() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }

    private void OnDeath() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }
}
