namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;

    public sealed partial class EventOperations {

        public static Action<string, ActionCode> ActionStateUpdateEvent;

        private static void OnActionStateUpdateEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var actionState = (ActionCode)eventData.Parameters[(byte)ParameterCode.ActionState];
            if (ActionStateUpdateEvent != null) {
                ActionStateUpdateEvent(name, actionState);
            }
        }
    }
}