namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public sealed partial class EventOperations {

        public static Action<string, Vector3> MoveEvent;

        private static void OnMoveEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var vecPos = (Vector)eventData.Parameters[(byte)ParameterCode.Position];
            var unityVec = new Vector3(vecPos.X, 0, vecPos.Z);
            if (MoveEvent != null) {
                MoveEvent(name, unityVec);
            }
        }
    }
}