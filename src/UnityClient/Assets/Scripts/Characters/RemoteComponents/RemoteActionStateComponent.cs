using UnityEngine;
using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient.Characters;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

public class RemoteActionStateComponent : ActionStateComponent {

    private RotationController m_RotationController;

    private void Start() {
        m_RotationController = GetComponent<RotationController>();
        EventOperations.ActionStateUpdateEvent += OnActionStateUpdateEvent;
    }

    private void OnActionStateUpdateEvent(string name, ActionCode newState, Vector3? lookDir) {
        if(gameObject.name != name) { return; }
        ActionState = newState;

        if(lookDir == null) { return; }

        float lockDuration = GetCastDuration(newState);
        m_RotationController.LookAt(lookDir.Value, lockDuration);
    }

    private float GetCastDuration(ActionCode action) {
        switch (action) {
            case ActionCode.DistractingShot:
                return 0.5f;
            case ActionCode.AxeAutoAttack:
            case ActionCode.BowAutoAttack:
            case ActionCode.HammerBash:
            case ActionCode.OrisonOfHealing:
                return 1f;

            case ActionCode.FireStorm:
                return 2f;
            default:
                return 0f;
        }
    }

    private void OnDeath() {
        Debug.Log("Private on death called by message");
        EventOperations.ActionStateUpdateEvent -= OnActionStateUpdateEvent;
    }

    private void OnDestroy() {
        EventOperations.ActionStateUpdateEvent -= OnActionStateUpdateEvent;
    }
}
