namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public sealed partial class EventOperations {

        public static Action<string, Vector2> MoveEvent;

        private static void OnMoveEvent(EventData eventData) {
            var username = (string)eventData.Parameters[(byte)ParameterCode.Username];
            var vecPos = (Vector)eventData.Parameters[(byte)ParameterCode.Position];
            var vec2Pos = new Vector2(vecPos.X, vecPos.Y);
            if (MoveEvent != null) {
                MoveEvent(username, vec2Pos);
            }
        }
    }
}