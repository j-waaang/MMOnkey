namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using Common.Codes;
    using ExitGames.Client.Photon;

    public sealed partial class EventOperations {
        internal static void OnEvent(EventData eventData) {
            switch ((EventCode)eventData.Code) {
                case EventCode.ActionStateUpdate:
                    OnActionStateUpdateEvent(eventData);
                    return;
                case EventCode.HealthUpdate:
                    OnHealthUpdateEvent(eventData);
                    return;
                case EventCode.Move:
                    OnMoveEvent(eventData);
                    return;
                case EventCode.NewPlayer:
                    OnNewPlayerEvent(eventData);
                    return;
                case EventCode.RemovePlayer:
                    OnRemovePlayerEvent(eventData);
                    return;
            }
            Debug.LogError("Cannot handle response.");
        }
    }
}
