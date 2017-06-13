using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient.Characters;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

public class RemoteActionStateComponent : ActionStateComponent {

    private void Start() {
        EventOperations.ActionStateUpdateEvent += OnActionStateUpdateEvent;
    }

    private void OnActionStateUpdateEvent(string name, ActionCode newState) {
        if(gameObject.name != name) { return; }
        ActionState = newState;
    }

    private void OnDestroy() {
        EventOperations.ActionStateUpdateEvent -= OnActionStateUpdateEvent;
    }
}
