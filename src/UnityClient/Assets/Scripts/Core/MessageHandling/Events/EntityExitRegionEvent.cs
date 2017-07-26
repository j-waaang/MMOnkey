using System;
using ExitGames.Client.Photon;
using UnityEngine;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using JYW.ThesisMMO.Common.Codes;

    public static partial class EventOperations {

        public static Action<string> EntityExitRegionEvent;

        private static void OnEntityExitRegionEvent(EventData eventData) {
            if (EntityExitRegionEvent == null) { return; }
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            //Debug.LogFormat("{0} exit region event", name);
            EntityExitRegionEvent(name);
        }
    }
}