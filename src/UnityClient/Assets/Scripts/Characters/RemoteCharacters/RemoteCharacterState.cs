using JYW.ThesisMMO.Common.Entities;
using JYW.ThesisMMO.UnityClient.Characters;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;

public class RemoteCharacterState : CharacterState {

    private void Start() {
        EventOperations.ActionStateUpdateEvent += OnActionStateUpdateEvent;
    }

    private void OnActionStateUpdateEvent(string name, ActionState newState) {
        ActionState = newState;
    }

    private void OnDestroy() {
        EventOperations.ActionStateUpdateEvent -= OnActionStateUpdateEvent;
    }

}
