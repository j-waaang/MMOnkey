﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;

    internal class ClientInterestArea : InterestArea {

        private static SendParameters PositionSendParameters = new SendParameters() { ChannelId = (byte)ChannelId.Position, Unreliable = true };
        private readonly PositionFilter m_PositionFilter;

        protected override IEnumerable<Region> FocusedRegions {
            get {
                var regions = World.Instance.Get9RegionsFromPoint(Center);
                Debug.Assert(
                    regions.Count() <= 9 && regions.Count() >= 4,
                    string.Format("Get region error with input {0} and region count {1}", Center, regions.Count()));
                return regions;
            }
        }

        public ClientInterestArea(Entity attachedEntity) : base(attachedEntity) {
            m_PositionFilter = new PositionFilter(attachedEntity);
        }

        /// <summary>
        /// Entity enters area
        /// </summary>
        public override void OnEntityEnter(Entity entity) {
            if (entity == m_AttachedEntity) { return; }
            var eventData = entity.GetEntitySnapshot();
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Item exits area
        /// </summary>
        public override void OnEntityExit(Entity entity) {
            if (entity == m_AttachedEntity) { return; }
            var ev = new RemovePlayerEvent() {
                Username = entity.Name,
            };
            IEventData eventData = new EventData((byte)EventCode.RemovePlayer, ev);
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Forwards the message to the client.
        /// </summary>
        protected override void OnEntityEvent(EventMessage message) {
            if (message.broadcastOptions == BroadcastOptions.IgnoreOwner &&
                message.sender == EntityName) {
                return;
            }

            EventMessage.CounterEventReceive.Increment();
            m_AttachedEntity.SendEvent(message.eventData, message.sendParameters);
        }

        protected override void OnPositionUpdate(Entity updatedEntity) {
            if (updatedEntity.Name == EntityName) { return; }
            if (m_PositionFilter.FilterPosition(updatedEntity.Position)) { return; }

            EventMessage.CounterEventReceive.Increment();

            UpdateClientPosition(updatedEntity);
        }

        private void UpdateClientPosition(Entity updatedEntity) {
            var moveEvent = new MoveEvent(updatedEntity.Name, updatedEntity.Position);
            IEventData eventData = new EventData((byte)EventCode.Move, moveEvent);
            m_AttachedEntity.SendEvent(eventData, PositionSendParameters);
        }
    }
}
