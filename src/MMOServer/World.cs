namespace JYW.ThesisMMO.MMOServer {

    using System;
    using System.Collections.Generic;
    using Common.Types;
    using Common.Codes;
    using Events;
    using Photon.SocketServer;
    using ExitGames.Logging;

    internal class World : IDisposable {
        public static World Instance { get; private set; }
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, PlayerEntity> m_Entities;

        private const float InterestDistance = 15f;

        internal World() {
            if (Instance == null) {
                Instance = this;
            }
            else { return; }
            m_Entities = new Dictionary<string, PlayerEntity>();
        }

        /// <summary>
        /// First add a new entity.
        /// </summary>
        internal void AddEntity(PlayerEntity newEntity) {

            var newPlayerEvent = new MoveEvent() {
                Username = newEntity.Username,
                Position = newEntity.Position
            };

            IEventData eventData = new EventData((byte)EventCode.Move, newPlayerEvent);
            var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };


            var interestedEntities = GetEntitiesInInterestRange(newEntity);
            foreach (PlayerEntity entity in interestedEntities) {
                entity.Peer.SendEvent(eventData, sendParameters);
            }
            
            m_Entities[newEntity.Username] = newEntity;
        }

        internal void RemoveEntity(string id) {
            m_Entities.Remove(id);
        }

        internal void NotifyEntityAboutExistingPlayers(string username) {
            var entityToNotify = m_Entities[username];

            foreach (PlayerEntity existingEntity in m_Entities.Values) {
                if (existingEntity == entityToNotify) { continue; }
                if (Vector.Distance(existingEntity.Position, entityToNotify.Position) > InterestDistance) { continue; }

                var existingPlayerEvent = new MoveEvent() {
                    Username = existingEntity.Username,
                    Position = existingEntity.Position
                };

                IEventData eventData = new EventData((byte)EventCode.Move, existingPlayerEvent);
                var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
                entityToNotify.Peer.SendEvent(eventData, sendParameters);
            }
        }

        internal void MoveEntity(string username, Vector position) {

            var movedEntity = m_Entities[username];
            IEventData eventData;
            MoveEvent moveEvent;
            var sendParameters = new SendParameters { Unreliable = true, ChannelId = 0 };

            var interestedEntities = GetEntitiesInInterestRange(movedEntity);
            foreach (PlayerEntity interestedEntity in interestedEntities) {
                if (Vector.Distance(position, interestedEntity.Position) < InterestDistance) {
                    moveEvent = new MoveEvent() {
                        Username = interestedEntity.Username,
                        Position = interestedEntity.Position
                    };
                    eventData = new EventData((byte)EventCode.Move, moveEvent);

                    movedEntity.Peer.SendEvent(eventData, sendParameters);
                }
            }

            movedEntity.Position = position;
            // TODO: check if new position is valid

            moveEvent = new MoveEvent() {
                Username = movedEntity.Username,
                Position = movedEntity.Position
            };
            eventData = new EventData((byte)EventCode.Move, moveEvent);

            foreach (PlayerEntity entity in interestedEntities) {
                entity.Peer.SendEvent(eventData, sendParameters);
            }
        }
        
        private List<PlayerEntity> GetEntitiesInInterestRange(PlayerEntity centerEntity) {
            var entities = new List<PlayerEntity>();
            foreach (PlayerEntity entity in m_Entities.Values) {
                if (entity == centerEntity) { continue; }
                if (Vector.Distance(entity.Position, centerEntity.Position) > InterestDistance) { continue; }

                entities.Add(entity);
            }

            return entities;
        }

        public void Dispose() {
            Instance = null;
        }
    }
}
