using ExitGames.Concurrency.Fibers;
using JYW.ThesisMMO.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JYW.ThesisMMO.MMOServer {
    internal class InterestArea {

        public readonly object SyncRoot = new object();

        private readonly HashSet<Region> m_Regions;
        private readonly IFiber m_SubscriptionManagementFiber;
        private readonly Dictionary<Region, IDisposable> m_RegionSubscriptions;
        private readonly Entity m_AttachedEntity;
        private readonly Vector m_CenterToMin;

        public InterestArea(Entity attachedEntity, float interestRadius) {
            m_AttachedEntity = attachedEntity;
            m_CenterToMin = m_AttachedEntity.Position - new Vector(interestRadius, interestRadius);
            m_Regions = new HashSet<Region>();
            m_SubscriptionManagementFiber = new PoolFiber();
            m_SubscriptionManagementFiber.Start();
        }

        private void UpdateInterestManagment() {
            BoundingBox2D focus = new BoundingBox2D(m_AttachedEntity.Position + m_CenterToMin, m_AttachedEntity.Position - m_CenterToMin);
            var focusedRegions = World.Instance.GetRegions(focus);
            SubscribeRegions(focusedRegions);
            UnsubscribeRegionsNotIn(focusedRegions);
        }

        private void OnItemRegionChange(EntityRegionChangedMessage message) {
            var r0 = regions.Contains(message.Region0);
            var r1 = regions.Contains(message.Region1);
            if (r0 && r1) {
                // do nothing
            }
            else if (r0) // item exits area
            {
                this.OnItemExit(message.ItemSnapshot.Source);
            }
            else if (r1) // item enters area
            {
                this.OnItemEnter(message.ItemSnapshot);
            }

        }

        /// <summary>
        /// Item enters area
        /// </summary>
        public virtual void OnItemEnter(ItemSnapshot snapshot) {
        }

        /// <summary>
        /// Item exits area
        /// </summary>
        public virtual void OnItemExit(Item item) {
        }

        /// <summary>
        /// Region enters area.
        /// </summary>
        protected virtual void OnRegionEnter(Region region) {
            var subscription = region.EntityRegionChangedChannel.Subscribe(m_SubscriptionManagementFiber, OnItemRegionChange);
            m_RegionSubscriptions.Add(region, subscription);
        }

        /// <summary>
        /// Region exits area.
        /// </summary>
        protected virtual void OnRegionExit(Region region) {
            IDisposable subscription;
            if (m_RegionSubscriptions.TryGetValue(region, out subscription)) {
                subscription.Dispose();
                m_RegionSubscriptions.Remove(region);
            }
        }

        private void SubscribeRegions(IEnumerable<Region> newRegions) {
            foreach (Region r in newRegions) {
                if (m_Regions.Contains(r)) { continue; }
                m_Regions.Add(r);
                OnRegionEnter(r);
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
    }
}
