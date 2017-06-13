using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;

public class RemoteMovementSpeedComponent : MovementSpeedComponent {

    private void Start() {
        EventOperations.SpeedUpdateEvent += OnSpeedUpdateEvent;
    }

    private void OnSpeedUpdateEvent(string name, float speed) {
        if (gameObject.name != name) { return; }
        MovementSpeed = speed;
    }

    private void OnDestroy() {
        EventOperations.SpeedUpdateEvent -= OnSpeedUpdateEvent;
    }
}
