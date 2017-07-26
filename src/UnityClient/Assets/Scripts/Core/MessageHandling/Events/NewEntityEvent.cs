namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using System;
    using ExitGames.Client.Photon;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    public static partial class EventOperations {

        private static void OnNewEntityEvent(EventData eventData) {
            var entityType = (EntityType)eventData.Parameters[(byte)ParameterCode.EntityType];

            switch (entityType) {
                case EntityType.Character:
                    throw new NotImplementedException();
                case EntityType.Skill:
                    OnNewSkillEntityEvent(eventData);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
