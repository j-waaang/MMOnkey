namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using Common.Codes;
    using ExitGames.Client.Photon;

    public sealed partial class EventOperations {
        // TODO: Merge some event codes e.g. new player event and new entity event.

        public static void OnEvent(EventData eventData) {
            switch ((EventCode)eventData.Code) {
                case EventCode.ActionStateUpdate:
                    OnActionStateUpdateEvent(eventData);
                    return;
                case EventCode.AttributeChangedEvent:
                    AttributeUpdateEvent(eventData);
                    return;
                case EventCode.HealthUpdate:
                    OnHealthUpdateEvent(eventData);
                    return;
                case EventCode.Move:
                    OnMoveEvent(eventData);
                    return;
                case EventCode.NewEntity:
                    OnNewEntityEvent(eventData);
                    return;
                case EventCode.NewPlayer:
                    OnNewPlayerEvent(eventData);
                    return;
                case EventCode.RemovePlayer:
                    OnRemovePlayerEvent(eventData);
                    return;
            }
            Debug.LogError("Don't know how to handle incoming event.");
        }

        private static void AttributeUpdateEvent(EventData eventData) {
            var attribute = (AttributeCode)eventData.Parameters[(byte)ParameterCode.AttributeCode];

            switch (attribute) {
                case AttributeCode.Speed:
                    OnSpeedUpdate(eventData);
                    return;
            }
            Debug.LogError("Don't know how to handle incoming attribute event.");
        }
    }
}
