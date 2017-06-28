namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using System;
    using UnityEngine;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using Common.Types;

    public sealed partial class EventOperations {

        public static Action<string, ActionCode, Vector3?> ActionStateUpdateEvent;

        private static void OnActionStateUpdateEvent(EventData eventData) {
            if (ActionStateUpdateEvent == null) { return; }

            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var actionState = (ActionCode)eventData.Parameters[(byte)ParameterCode.ActionState];

            var lookDirVec = (Vector)eventData.Parameters[(byte)ParameterCode.LookDirection];
            Vector3? lookDir = new Vector3(lookDirVec.X, 0, lookDirVec.Z);
            if (actionState == ActionCode.Idle) {
                lookDir = null;
            }

            ActionStateUpdateEvent(name, actionState, lookDir);
        }
    }
}