namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using System;
    using UnityEngine;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;
    using Common.Types;

    public sealed partial class EventOperations {

        public static Action<string, ActionCode, Vector3?> ActionStateUpdateEvent;

        private static void OnActionStateUpdateEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var actionState = (ActionCode)eventData.Parameters[(byte)ParameterCode.ActionState];

            object lookDirObj;
            eventData.Parameters.TryGetValue((byte)ParameterCode.LookDirection, out lookDirObj);

            Vector3? lookDir = null;
            if(lookDirObj != null) {
                var lookDirVec = (Vector)lookDirObj;
                lookDir = new Vector3(lookDirVec.X, 0, lookDirVec.Z);
            }

            if (ActionStateUpdateEvent != null) {
                ActionStateUpdateEvent(name, actionState, lookDir);
            }
        }
    }
}