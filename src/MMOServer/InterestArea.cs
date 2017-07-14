using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Types;

    internal class InterestArea {

        public readonly object SyncRoot = new object();

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected readonly IFiber m_SubscriptionManagementFiber = new PoolFiber();
        protected readonly HashSet<Region> m_Regions = new HashSet<Region>();
        protected readonly Dictionary<Region, IDisposable> m_RegionChangedSubscriptions = new Dictionary<Region, IDisposable>();
        protected readonly Dictionary<Region, IDisposable> m_RegionEventSubscriptions = new Dictionary<Region, IDisposable>();

        protected readonly Entity m_AttachedEntity;

        protected Vector m_Center {
            get {
                return m_AttachedEntity.Position;
            }
        }

        public InterestArea(Entity attachedEntity, float interestRadius) {
            m_AttachedEntity = attachedEntity;
            //m_CenterToMin = new Vector(-interestRadius, -interestRadius);
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
            //log.InfoFormat("{0} is supped to {1} region(s)", m_AttachedEntity.Name, m_Regions.Count);
        }
        protected void SubscribeRegions(IEnumerable<Region> newRegions) {
            foreach (Region r in newRegions) {
                if (m_Regions.Contains(r)) {
                    continue; }
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
            //log.InfoFormat("{0} subbed to region {1},{2}'s EntityRegionChangedChannel and RegionEventChannel.",
            //    m_AttachedEntity.Name,
            //    region.X,
            //    region.Z);

            var subscription = region.EntityRegionChangedChannel.Subscribe(m_SubscriptionManagementFiber, OnEntityRegionChange);
            m_RegionChangedSubscriptions.Add(region, subscription);

            subscription = region.RegionEventChannel.Subscribe(m_AttachedEntity.Fiber, OnEntityEvent);
            m_RegionEventSubscriptions[region] = subscription;
        }

        private void OnRequestEntitySnapshot(InterestArea obj) {
            throw new NotImplementedException();
        }

        private void OnEntityRegionChange(EntityRegionChangedMessage message) {
            if(message.Entity == m_AttachedEntity) {
                return; }
            //log.InfoFormat("{0} received EntityRegionChangedMessage from {1}", m_AttachedEntity.Name, message.Entity.Name);

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
        protected virtual void OnEntityEvent(EventMessage message) {
        }

        /// <summary>
        /// Entity enters region
        /// </summary>
        public virtual void OnEntityEnter(Entity entity) {
        }

        /// <summary>
        /// Entity exits region
        /// </summary>
        public virtual void OnEntityExit(Entity entity) {
        }
    }
}
