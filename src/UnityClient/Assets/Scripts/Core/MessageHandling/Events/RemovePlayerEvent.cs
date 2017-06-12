namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public sealed partial class EventOperations {

        public static Action<string> PlayerRemovedEvent;

        private static void OnPlayerRemovedEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            if (PlayerRemovedEvent != null) {
                PlayerRemovedEvent(name);
            }
        }
    }
}