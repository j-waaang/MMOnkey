using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;

public class RemotePositionComponent : MonoBehaviour {

    private void Start() {
        EventOperations.MoveEvent += OnMoveEvent;
    }

    /// <summary>  
    ///  Listen to the servers events and moves on event.
    /// </summary>  
    private void OnMoveEvent(string name, Vector3 position) {
        if (gameObject.name != name) { return; }

        transform.position = position;
    }

    private void OnDestroy() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }

    private void OnDeath() {
        EventOperations.MoveEvent -= OnMoveEvent;
    }
}
