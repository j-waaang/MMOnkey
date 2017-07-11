using ExitGames.Concurrency.Fibers;
using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.Common.Types;
using JYW.ThesisMMO.MMOServer.Events;
using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JYW.ThesisMMO.MMOServer {
    internal class InterestArea {

        public readonly object SyncRoot = new object();

        private readonly IFiber m_SubscriptionManagementFiber = new PoolFiber();
        private readonly HashSet<Region> m_Regions = new HashSet<Region>();
        private readonly Dictionary<Region, IDisposable> m_RegionChangedSubscriptions = new Dictionary<Region, IDisposable>();
        private readonly Dictionary<Region, IDisposable> m_RegionEventSubscriptions = new Dictionary<Region, IDisposable>();

        private readonly Entity m_AttachedEntity;
        private readonly Vector m_CenterToMin;

        public InterestArea(Entity attachedEntity, float interestRadius) {
            m_AttachedEntity = attachedEntity;
            m_CenterToMin = m_AttachedEntity.Position - new Vector(interestRadius, interestRadius);
            m_SubscriptionManagementFiber.Start();

            //UpdateInterestManagment();
        }

        public void UpdateInterestManagment() {
            BoundingBox2D focus = new BoundingBox2D(m_AttachedEntity.Position + m_CenterToMin, m_AttachedEntity.Position - m_CenterToMin);
            var focusedRegions = World.Instance.GetRegions(focus);
            SubscribeRegions(focusedRegions);
            UnsubscribeRegionsNotIn(focusedRegions);
        }

        private void SubscribeRegions(IEnumerable<Region> newRegions) {
            foreach (Region r in newRegions) {
                if (m_Regions.Contains(r)) { continue; }
                m_Regions.Add(r);
                SubscribeToRegion(r);
                r.RequestEnterRegionChannel.Publish(this);
            }
        }

        private void UnsubscribeRegionsNotIn(IEnumerable<Region> regionsToSurvive) {
            var toUnsubscribeEnumerable = m_Regions.Except(regionsToSurvive);
            var toUnsubscribe = toUnsubscribeEnumerable.ToArray(); // make copy

            foreach (var r in toUnsubscribe) {
                m_Regions.Remove(r);
                OnRegionExit(r);
                r.RequestExitRegionChannel.Publish(this);
            }
        }

        private void OnEntityRegionChange(EntityRegionChangedMessage message) {
            var r0 = m_Regions.Contains(message.From);
            var r1 = m_Regions.Contains(message.To);
            if (r0 && r1) {
                // do nothing
            }
            else if (r0) // item exits area
            {
                OnItemExit(message.Entity);
            }
            else if (r1) // item enters area
            {
                OnEntityEnter(message.Entity);
            }
        }

        private void SubscribeToRegion(Region region) {
            var subscription = region.EntityRegionChangedChannel.Subscribe(m_SubscriptionManagementFiber, OnEntityRegionChange);
            m_RegionChangedSubscriptions.Add(region, subscription);

            subscription = region.RegionEventChannel.Subscribe(m_AttachedEntity.Peer.RequestFiber, OnItemEvent);
            m_RegionEventSubscriptions[region] = subscription;
        }

        /// <summary>
        /// Region exits area.
        /// </summary>
        private void OnRegionExit(Region region) {
            IDisposable subscription;
            if (m_RegionChangedSubscriptions.TryGetValue(region, out subscription)) {
                subscription.Dispose();
                m_RegionChangedSubscriptions.Remove(region);
            }
        }

        /// <summary>
        /// Event relayed by subscribed region from region's items.
        /// </summary>
        private void OnItemEvent(EventMessage message) {
            EventMessage.CounterEventReceive.Increment();
            m_AttachedEntity.Peer.SendEvent(message.EventData, message.SendParameters);
        }

        /// <summary>
        /// Entity enters area
        /// </summary>
        private void OnEntityEnter(Entity entity) {
            var eventData = entity.GetNewEntityEventData();
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Item exits area
        /// </summary>
        public virtual void OnItemExit(Entity entity) {
            var ev = new RemovePlayerEvent() {
                Username = entity.Name,
            };
            IEventData eventData = new EventData((byte)EventCode.RemovePlayer, ev);
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Region enters area.
        /// </summary>



    }
}
