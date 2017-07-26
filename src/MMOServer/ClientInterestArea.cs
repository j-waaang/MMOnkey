using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Photon.SocketServer;
using Photon.SocketServer.Concurrency;
using ExitGames.Concurrency.Fibers;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using JYW.ThesisMMO.MMOServer.CSAIM;

    internal class ClientInterestArea : InterestArea {

        protected readonly IFiber m_SubscriptionManagementFiber = new PoolFiber();
        protected readonly Dictionary<Region, IDisposable> m_RegionChangedSubscriptions = new Dictionary<Region, IDisposable>();
        protected readonly Dictionary<Region, UnsubscriberCollection> m_RegionEventSubscriptions = new Dictionary<Region, UnsubscriberCollection>();
        protected readonly HashSet<Region> m_Regions = new HashSet<Region>();

        private static int PositionUpdateIntervalInMs = 30;
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
            m_SubscriptionManagementFiber.Start();
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
            var ev = new EntityEvent() {
                Username = entity.Name,
            };
            IEventData eventData = new EventData((byte)EventCode.EntityExitRegion, ev);
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Forwards the message to the client.
        /// </summary>
        private void OnEntityEvent(EventMessage message) {
            if (message.broadcastOptions == BroadcastOptions.IgnoreOwner &&
                message.sender == EntityName) {
                return;
            }

            EventMessage.CounterEventReceive.Increment();
            m_AttachedEntity.SendEvent(message.eventData, message.sendParameters);
        }

        private void OnPositionUpdate(Entity entity) {
            if (entity.Name == EntityName) { return; }
            //log.InfoFormat("{0} got {1} move update", EntityName, entity.Name);
            m_PositionFilter.OnPositionUpdate(entity);
        }

        protected override void ChangeRegion(Region from, Region to) {
            if (to != null) {
                log.InfoFormat("{0} moved to {1} region.", EntityName, to.ToString());
            }
            UpdateRegionSubscription();
            base.ChangeRegion(from, to);
        }

        /// <summary>
        /// Subs and unsubs from regions depending on focus.
        /// Should be called when entering a new region or the entity moved.
        /// </summary>
        protected void UpdateRegionSubscription() {
            var focus = FocusedRegions;
            SubscribeRegions(focus);
            UnsubscribeRegionsNotIn(focus);
        }

        protected void SubscribeRegions(IEnumerable<Region> newRegions) {
            foreach (Region r in newRegions) {
                if (m_Regions.Contains(r)) {
                    continue;
                }
                m_Regions.Add(r);
                SubscribeToRegion(r);
                r.RequestInfoInRegionChannel.Publish(this);
            }
        }

        protected void UnsubscribeRegionsNotIn(IEnumerable<Region> regionsToSurvive) {
            var toUnsubscribeEnumerable = m_Regions.Except(regionsToSurvive);
            var toUnsubscribe = toUnsubscribeEnumerable.ToArray(); // make copy

            foreach (var r in toUnsubscribe) {
                m_Regions.Remove(r);
                OnRegionExit(r);
                r.RequestRegionExitInfoChannel.Publish(this);
            }
        }

        protected void SubscribeToRegion(Region region) {
            var subscription = region.EntityRegionChangedChannel.Subscribe(m_SubscriptionManagementFiber, OnEntityRegionChange);
            m_RegionChangedSubscriptions.Add(region, subscription);

            m_RegionEventSubscriptions[region] = new UnsubscriberCollection(
                region.RegionEventChannel.Subscribe(m_EntityFiber, OnEntityEvent),
                region.PositionUpdateChannel.SubscribeToLast(m_EntityFiber, OnPositionUpdate, PositionUpdateIntervalInMs));
        }

        protected void OnEntityRegionChange(EntityRegionChangedMessage message) {
            if (message.Entity == m_AttachedEntity) { return; }

            var r0 = m_Regions.Contains(message.From);
            var r1 = m_Regions.Contains(message.To);
            if (r0 && r1) {
                // do nothing
            }
            else if (r0) // item exits area
            {
                OnEntityExit(message.Entity);
            }
            else if (r1) // item enters area
            {
                OnEntityEnter(message.Entity);
            }
        }

        /// <summary>
        /// Region exits area.
        /// </summary>
        protected void OnRegionExit(Region region) {
            m_RegionChangedSubscriptions[region].Dispose();
            m_RegionChangedSubscriptions.Remove(region);

            m_RegionEventSubscriptions[region].Dispose();
            m_RegionEventSubscriptions.Remove(region);
        }

        public override void Dispose() {
            UnsubscribeRegionsNotIn(Enumerable.Empty<Region>());
            m_SubscriptionManagementFiber.Dispose();
            base.Dispose();
        }
    }
}
