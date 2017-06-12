namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class EventOperations {

        public static event Action<string, int, int> HealthUpdateEvent;

        private static void OnHealthUpdateEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var damage = (int)eventData.Parameters[(byte)ParameterCode.Damage];
            var curHealth = (int)eventData.Parameters[(byte)ParameterCode.CurrentHealth];
            if (HealthUpdateEvent != null) {
                HealthUpdateEvent(name, damage, curHealth);
            }
        }
    }
}