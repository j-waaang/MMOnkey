namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class EventOperations {

        public static event Action<string, float> SpeedUpdateEvent;

        private static void OnSpeedUpdate(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var speed = (float)eventData.Parameters[(byte)ParameterCode.FloatValue];
            if (SpeedUpdateEvent != null) {
                SpeedUpdateEvent(name, speed);
            }
        }
    }
}