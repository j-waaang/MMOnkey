namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public static partial class EventOperations {

        public static Action<string, Vector3> FilteredMoveEvent;

        private static void OnFilteredMoveEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var vecPos = (Vector)eventData.Parameters[(byte)ParameterCode.Position];
            var unityVec = new Vector3(vecPos.X, 0, vecPos.Z);
            if (FilteredMoveEvent != null) {
                FilteredMoveEvent(name, unityVec);
            }
        }
    }
}