using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;

public class RemotePositionController : MonoBehaviour {

    private MovementStateComponent m_MovementState;

    private Vector3 m_LastReceivedPosition;

    private void Awake() {
        m_MovementState = GetComponent<MovementStateComponent>();
    }

    private void Start() {
        EventOperations.MoveEvent += OnMoveEvent;
    }

    /// <summary>  
    ///  Listen to the servers events and moves on event.
    /// </summary>  
    private void OnMoveEvent(string name, Vector3 position) {
        if (gameObject.name != name) { return; }

        transform.position = position;
        m_LastReceivedPosition = position;
    }

    private void OnDestroy() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }

    private void OnDeath() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }
}
