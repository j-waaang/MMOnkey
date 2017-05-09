namespace JYW.ThesisMMO.MMOServer {
    using System;
    using System.Collections.Generic;
    using Common.Types;
    using Common.Codes;
    using Events;
    using Photon.SocketServer.Concurrency;
    using ExitGames.Concurrency.Fibers;
    using ExitGames.Concurrency.Channels;
    using Photon.SocketServer;
    using ExitGames.Logging;

    internal class World : IDisposable{
        public static World Instance { get; private set; }
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, PlayerEntity> m_Entities;
        private ActionQueue m_ActionQueue;
        private IFiber m_Fiber;
        private Channel<Message> m_MessageChannel = new Channel<Message>();

        internal World() {
            if(Instance == null) {
                Instance = this;
            }
            else { return; }
            m_Entities = new Dictionary<string, PlayerEntity>();

        }
        private void SetupActionFiberChannel() {
            m_Fiber = new PoolFiber();
            m_Fiber.Start();
            m_ActionQueue = new ActionQueue(this, m_Fiber);
        }
        /// <summary>
        /// First add a new entity.
        /// </summary>
        internal void AddEntity(PlayerEntity entity) {
            log.DebugFormat("Adding entity with name: " + entity.Username);
            m_Entities[entity.Username] = entity;

            var newPlayerEvent = new NewPlayerEvent();
            newPlayerEvent.Username = entity.Username;
            newPlayerEvent.Position = entity.Position;
            IEventData eventData = new EventData((byte)EventCode.NewPlayer, newPlayerEvent);
            var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
            var message = new Message(eventData, sendParameters);
            m_MessageChannel.Publish(message);
        }
        /// <summary>
        /// Then subscribe for future messages. To avoid receiving messages from AddEntity in the first place.
        /// </summary>
        internal void SubscribeToMessageChannel(IFiber fiber, Action<Message> receive) {
            m_MessageChannel.Subscribe(fiber, receive);
        }

        internal void RemoveEntity(string id) {
            m_Entities.Remove(id);
        }

        internal void NotifyEntityAboutExistingPlayers(string username) {
            var entityToNotify = m_Entities[username];

            foreach (PlayerEntity existingEntity in m_Entities.Values) {
                if(existingEntity == entityToNotify) { continue; }

                var existingPlayerEvent = new NewPlayerEvent();
                existingPlayerEvent.Username = existingEntity.Username;
                existingPlayerEvent.Position = existingEntity.Position;
                IEventData eventData = new EventData((byte)EventCode.NewPlayer, existingPlayerEvent);
                var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
                entityToNotify.Peer.SendEvent(eventData, sendParameters);
            }
        }

        internal void MoveEntity(string username, Vector position) {
            var entity = m_Entities[username];
            entity.Position = position;
            // TODO: check if position change is valid
        }
        public void Dispose() {
            Instance = null;
        }
    }
}
