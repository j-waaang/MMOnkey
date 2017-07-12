using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Events;

    internal class InterestArea {

        public readonly object SyncRoot = new object();

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected readonly IFiber m_SubscriptionManagementFiber = new PoolFiber();
        protected readonly HashSet<Region> m_Regions = new HashSet<Region>();
        protected readonly Dictionary<Region, IDisposable> m_RegionChangedSubscriptions = new Dictionary<Region, IDisposable>();
        protected readonly Dictionary<Region, IDisposable> m_RegionEventSubscriptions = new Dictionary<Region, IDisposable>();

        protected readonly Entity m_AttachedEntity;
        protected readonly Vector m_CenterToMin;

        protected BoundingBox2D m_Focus {
            get {
                return new BoundingBox2D(m_AttachedEntity.Position + m_CenterToMin, m_AttachedEntity.Position - m_CenterToMin);
            }
        }

        #region public methods
        public InterestArea(Entity attachedEntity, float interestRadius) {
            m_AttachedEntity = attachedEntity;
            m_CenterToMin = new Vector(-interestRadius, -interestRadius);
            m_SubscriptionManagementFiber.Start();
        }

        /// <summary>
        /// Subs and unsubs from regions depending on focus.
        /// Should be called when entering a new region or the entity moved.
        /// </summary>
        public virtual void UpdateRegionSubscription() {
            Region[] focusedRegions = { World.Instance.GetRegionFromPoint(m_AttachedEntity.Position) };
            SubscribeRegions(focusedRegions);
            UnsubscribeRegionsNotIn(focusedRegions);

            string subbedregions = "";
            foreach(var region in m_Regions) {
                subbedregions += region.ToString() + " ";
            }

            log.InfoFormat("{0} subed to {2} regions {1}", m_AttachedEntity.Name, subbedregions, m_Regions.Count);
        }
        #endregion public methods
        #region private methods
        protected void SubscribeRegions(IEnumerable<Region> newRegions) {
            foreach (Region r in newRegions) {
                if (m_Regions.Contains(r)) { continue; }
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

            subscription = region.RegionEventChannel.Subscribe(m_AttachedEntity.Fiber, OnItemEvent);
            m_RegionEventSubscriptions[region] = subscription;
        }

        private void OnRequestEntitySnapshot(InterestArea obj) {
            throw new NotImplementedException();
        }

        private void OnEntityRegionChange(EntityRegionChangedMessage message) {
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
        public void OnEntityEnter(Entity entity) {
            var eventData = entity.GetEntitySnapshot();
            m_AttachedEntity.SendEvent(eventData);
        }

        /// <summary>
        /// Item exits area
        /// </summary>
        public void OnEntityExit(Entity entity) {
            var ev = new RemovePlayerEvent() {
                Username = entity.Name,
            };
            IEventData eventData = new EventData((byte)EventCode.RemovePlayer, ev);
            m_AttachedEntity.SendEvent(eventData);
        }
        #endregion private methods
    }
}
