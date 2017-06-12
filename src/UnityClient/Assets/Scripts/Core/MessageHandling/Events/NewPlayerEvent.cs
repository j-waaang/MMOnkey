namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public sealed partial class EventOperations {

        /// <summary>  
        ///  Called whenever a new player enters the world.
        /// </summary>  
        public static Action<string, Vector2, int, int> NewPlayerEvent;

        private static void OnNewPlayerEvent(EventData eventData) {
            // TODO: Extend this event with more data later.
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var vecPos = (Vector)eventData.Parameters[(byte)ParameterCode.Position];
            var vec2Pos = new Vector2(vecPos.X, vecPos.Y);
            var health = (int)eventData.Parameters[(byte)ParameterCode.CurrentHealth];
            var maxHealth = (int)eventData.Parameters[(byte)ParameterCode.MaxHealth];

            if (NewPlayerEvent != null) {
                NewPlayerEvent(name, vec2Pos, health, maxHealth);
            }
            else {
                Debug.Log("No new player listener");
            }
        }
    }
}
