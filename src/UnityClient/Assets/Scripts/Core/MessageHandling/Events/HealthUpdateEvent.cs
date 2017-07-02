using ExitGames.Client.Photon;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class EventOperations {

        public delegate void HealthUpdate(string name, int damage, int curHealth);
        public static event HealthUpdate HealthUpdatedEvent;

        private static void OnHealthUpdateEvent(EventData eventData) {
            if (HealthUpdatedEvent == null) { return; }

            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var damage = (int)eventData.Parameters[(byte)ParameterCode.Damage];
            var curHealth = (int)eventData.Parameters[(byte)ParameterCode.CurrentHealth];
            HealthUpdatedEvent(name, damage, curHealth);
        }
    }
}