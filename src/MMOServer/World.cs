namespace JYW.ThesisMMO.MMOServer {

    using System;
    using System.Collections.Generic;

    using Photon.SocketServer;
    using ExitGames.Logging;

    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Entities;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;

    /// <summary> 
    /// The game world containing entities and methods modifiying them.
    /// </summary>
    internal class World : IDisposable {

        public static World Instance { get; private set; }

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Entity> m_Entities;

        private const float InterestDistance = 15f;

        /// <summary>
        /// Creates a instance of the game world.
        /// </summary>
        internal World() {
            if (Instance == null) {
                Instance = this;
            }
            else { return; }
            m_Entities = new Dictionary<string, Entity>();
        }

        /// <summary>
        /// Adding a entity to the game world.
        /// </summary>
        internal void AddEntity(Entity newEntity) {

            var newPlayerEvent = new MoveEvent() {
                Username = newEntity.Name,
                Position = newEntity.Position
            };

            IEventData eventData = new EventData((byte)EventCode.Move, newPlayerEvent);
            var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };

            foreach (Entity entity in m_Entities.Values) {
                entity.SendEvent(eventData, sendParameters);
            }

            //var interestedEntities = GetEntitiesInInterestRange(newEntity);
            //foreach (Entity entity in interestedEntities) {
            //    entity.SendEvent(eventData, sendParameters);
            //}
            
            m_Entities[newEntity.Name] = newEntity;
        }

        /// <summary>
        /// Removes the entity from the list.
        /// </summary>
        internal void RemoveEntity(string id) {
            m_Entities.Remove(id);
        }

        internal void NotifyEntityAboutExistingPlayers(string username) {
            var entityToNotify = m_Entities[username];

            foreach (Entity existingEntity in m_Entities.Values) {
                if (existingEntity == entityToNotify) { continue; }

                //if (Vector.Distance(existingEntity.Position, entityToNotify.Position) > InterestDistance) { continue; }

                var existingPlayerEvent = new MoveEvent() {
                    Username = existingEntity.Name,
                    Position = existingEntity.Position
                };

                IEventData eventData = new EventData((byte)EventCode.Move, existingPlayerEvent);
                var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
                entityToNotify.SendEvent(eventData, sendParameters);
            }
        }

        internal void MoveEntity(string username, Vector position) {

            var movedEntity = m_Entities[username];
            movedEntity.Position = position;
            var moveEvent = new MoveEvent() {
                Username = movedEntity.Name,
                Position = movedEntity.Position
            };
            IEventData eventData = new EventData((byte)EventCode.Move, moveEvent);
            var sendParameters = new SendParameters { Unreliable = true, ChannelId = 0 };

            foreach (Entity entity in m_Entities.Values) {
                if(entity == movedEntity) { continue; }
                entity.SendEvent(eventData, sendParameters);
            }

            //var movedEntity = m_Entities[username];
            //IEventData eventData;
            //MoveEvent moveEvent;
            //var sendParameters = new SendParameters { Unreliable = true, ChannelId = 0 };

            //var interestedEntities = GetEntitiesInInterestRange(movedEntity);
            //foreach (Entity interestedEntity in interestedEntities) {
            //    if (Vector.Distance(position, interestedEntity.Position) < InterestDistance) {
            //        moveEvent = new MoveEvent() {
            //            Username = interestedEntity.Name,
            //            Position = interestedEntity.Position
            //        };
            //        eventData = new EventData((byte)EventCode.Move, moveEvent);

            //        movedEntity.SendEvent(eventData, sendParameters);
            //    }
            //}

            //movedEntity.Position = position;
            //// TODO: check if new position is valid

            //moveEvent = new MoveEvent() {
            //    Username = movedEntity.Name,
            //    Position = movedEntity.Position
            //};
            //eventData = new EventData((byte)EventCode.Move, moveEvent);

            //foreach (Entity entity in interestedEntities) {
            //    entity.SendEvent(eventData, sendParameters);
            //}
        }

        //private List<Entity> GetEntitiesInInterestRange(Entity centerEntity) {
        //    var entities = new List<Entity>();
        //    foreach (Entity entity in m_Entities.Values) {
        //        if (entity == centerEntity) { continue; }
        //        if (Vector.Distance(entity.Position, centerEntity.Position) > InterestDistance) { continue; }

        //        entities.Add(entity);
        //    }

        //    return entities;
        //}

        internal bool CanPerformAction(string actionSource, ActionCode action, Target target) {

            if (!m_Entities[actionSource].CanPerformAction(action)) { return false; }

            // TODO: Test distance
            switch (action) {
                case ActionCode.AxeAutoAttack:
                    break;
                case ActionCode.BowAutoAttack:
                    break;
                case ActionCode.Move:
                    break;
                case ActionCode.Dash:
                    break;
                case ActionCode.DistractingShot:
                    break;
                case ActionCode.FireStorm:
                    break;
                case ActionCode.HammerBash:
                    break;
                case ActionCode.OrisonOfHealing:
                    break;
                default:
                    break;
            }
            return true;
        }

        internal void ApplyModifier(string target, Modifier modifier) {
            modifier.ApplyOnEntity(m_Entities[target]);
        }

        public void Dispose() {
            Instance = null;
        }
    }
}
