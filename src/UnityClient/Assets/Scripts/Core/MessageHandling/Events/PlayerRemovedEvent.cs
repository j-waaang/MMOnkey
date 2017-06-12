namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class EventOperations {

        public static Action<string> RemovePlayerEvent;

        private static void OnRemovePlayerEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            if (RemovePlayerEvent != null) {
                RemovePlayerEvent(name);
            }
        }
    }
}