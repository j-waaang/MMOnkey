namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public static partial class EventOperations {

        /// <summary>  
        ///  Called whenever a new player enters the world.
        /// </summary>  
        public static Action<string, ActionCode, Vector3> NewSkillEntityEvent;

        private static void OnNewSkillEntityEvent(EventData eventData) {
            var name = (string)eventData.Parameters[(byte)ParameterCode.Name];
            var vecPos = (Vector)eventData.Parameters[(byte)ParameterCode.Position];
            var vec3Pos = new Vector3(vecPos.X, 0, vecPos.Z);
            var action = (ActionCode)eventData.Parameters[(byte)ParameterCode.ActionCode];


            if (NewSkillEntityEvent != null) {
                NewSkillEntityEvent(name, action, vec3Pos);
            }
        }
    }
}
