using JYW.ThesisMMO.Common.Entities;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;

public class RemotePositionController : MonoBehaviour {

    private const float m_AutoIdle = 0.1f;

    private MovementStateComponent m_MovementState;
    private Vector3 m_LastReceivedPosition;
    private float m_LastReceivedTime;

    private void Start() {
        m_MovementState = GetComponent<MovementStateComponent>();
        EventOperations.MoveEvent += OnMoveEvent;
    }

    private void Update() {
        if(m_MovementState.MovementState == MovementState.Idle) { return; }

        if(Time.time - m_LastReceivedTime > m_AutoIdle) {
            m_MovementState.MovementState = MovementState.Idle;
        }
    }

    /// <summary>  
    ///  Listen to the servers events and moves on event.
    /// </summary>  
    private void OnMoveEvent(string name, Vector3 receivedPosition) {
        if (gameObject.name != name) { return; }

        var predictedPosition = PredictPosition(receivedPosition);

        UpdateRotation(predictedPosition);

        m_MovementState.MovementState = MovementState.Moving;
        transform.position = predictedPosition;
        m_LastReceivedPosition = predictedPosition;
        m_LastReceivedTime = Time.time;
    }

    private Vector3 PredictPosition(Vector3 outdatedPosition) {
        // TODO: Do dead reckoning
        return outdatedPosition;
    }

    private void UpdateRotation(Vector3 newPosition) {
        transform.forward = newPosition - transform.position;
    }

    private void OnDestroy() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }

    private void OnDeath() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }
}
