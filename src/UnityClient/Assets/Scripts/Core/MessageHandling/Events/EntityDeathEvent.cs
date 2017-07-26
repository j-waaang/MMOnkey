using System;
using ExitGames.Client.Photon;
using UnityEngine;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using JYW.ThesisMMO.Common.Codes;

    public static partial class EventOperations {

        public static Action<string> EntityDeathEvent;

        private static void OnEntityDeathEvent(EventData eventData) {
            if (EntityDeathEvent == null) { return; }
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            EntityDeathEvent(name);
        }
    }
}