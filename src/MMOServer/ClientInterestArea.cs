using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.MMOServer.Events;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYW.ThesisMMO.MMOServer {
    class ClientInterestArea : InterestArea {
        public ClientInterestArea(Entity attachedEntity, float interestRadius) : base(attachedEntity, interestRadius) {
        }

        /// <summary>
        /// Subs and unsubs regions surrounding center.
        /// Should be called when entering a new region or the entity moved.
        /// </summary>
        public override void UpdateRegionSubscription() {
            var focusedRegions = World.Instance.Get9RegionsFromPoint(m_Center);
            SubscribeRegions(focusedRegions);
            UnsubscribeRegionsNotIn(focusedRegions);
        }

        /// <summary>
        /// Entity enters area
        /// </summary>
        public override void OnEntityEnter(Entity entity) {
            var eventData = entity.GetEntitySnapshot();
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Item exits area
        /// </summary>
        public override void OnEntityExit(Entity entity) {
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
            base.OnEntityEvent(message);
            EventMessage.CounterEventReceive.Increment();
            m_AttachedEntity.SendEvent(message.EventData, message.SendParameters);
        }
    }
}
