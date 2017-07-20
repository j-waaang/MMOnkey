using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer.Concurrency;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Types;

    internal class InterestArea : IDisposable{

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        protected readonly IFiber m_SubscriptionManagementFiber = new PoolFiber();
        protected readonly HashSet<Region> m_Regions = new HashSet<Region>();
        protected readonly Dictionary<Region, IDisposable> m_RegionChangedSubscriptions = new Dictionary<Region, IDisposable>();
        protected readonly Dictionary<Region, UnsubscriberCollection> m_RegionEventSubscriptions = new Dictionary<Region, UnsubscriberCollection>();
        protected readonly Entity m_AttachedEntity;

        private static int PositionUpdateIntervalInMs = 30;

        private Region m_CurrentRegion;
        private IDisposable m_RegionSubscription;

        protected readonly IFiber m_EntityFiber = new PoolFiber();

        protected virtual IEnumerable<Region> FocusedRegions {
            get {
                return new Region[] { World.Instance.GetRegionFromPoint(m_AttachedEntity.Position) };
            }
        }
        protected Vector Center {
            get {
                return m_AttachedEntity.Position;
            }
        }
        protected string EntityName {
            get {
                return m_AttachedEntity.Name;
            }
        }

        public InterestArea(Entity attachedEntity) {
            m_AttachedEntity = attachedEntity;
            m_SubscriptionManagementFiber.Start();
            m_EntityFiber.Start();
        }

        public void Update() {
            var newRegion = World.Instance.GetRegionFromPoint(Center);
            if (m_CurrentRegion != newRegion) {
                log.InfoFormat("{0} moved to {1} region.", EntityName, newRegion.ToString());
                UpdateRegionSubscription();
                ChangeRegion(m_CurrentRegion, newRegion);
            }
        }

        private void ChangeRegion(Region from, Region to) {
            m_CurrentRegion = to;

            if (m_RegionSubscription != null) {
                m_RegionSubscription.Dispose();
            }

            var msg = new EntityRegionChangedMessage(from, to, m_AttachedEntity);
            if (from != null) {
                from.EntityRegionChangedChannel.Publish(msg);
            }

            if (to != null) {
                to.EntityRegionChangedChannel.Publish(msg);
                m_RegionSubscription = new UnsubscriberCollection(
                    //this.EventChannel.Subscribe(this.Fiber, (m) => newRegion.ItemEventChannel.Publish(m)), // route events through region to interest area

                    // region entered interest area fires message to let item notify interest area about enter
                    to.RequestInfoInRegionChannel.Subscribe(m_EntityFiber, (m) => { m.OnEntityEnter(m_AttachedEntity); }),

                    // region exited interest area fires message to let item notify interest area about exit
                    to.RequestRegionExitInfoChannel.Subscribe(m_EntityFiber, (m) => { m.OnEntityExit(m_AttachedEntity); })
                );
            }
        }

        /// <summary>
        /// Subs and unsubs from regions depending on focus.
        /// Should be called when entering a new region or the entity moved.
        /// </summary>
        public void UpdateRegionSubscription() {
            var focus = FocusedRegions;
            SubscribeRegions(focus);
            UnsubscribeRegionsNotIn(focus);
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
                log.InfoFormat("{0} unsubbed from {1}", EntityName, r);
            }
        }

        protected void SubscribeToRegion(Region region) {
            var subscription = region.EntityRegionChangedChannel.Subscribe(m_SubscriptionManagementFiber, OnEntityRegionChange);
            m_RegionChangedSubscriptions.Add(region, subscription);

            m_RegionEventSubscriptions[region] = new UnsubscriberCollection(
                region.RegionEventChannel.Subscribe(m_EntityFiber, OnEntityEvent),
                region.PositionUpdateChannel.SubscribeToLast(m_EntityFiber, OnPositionUpdate, PositionUpdateIntervalInMs));
        }

        private void OnEntityRegionChange(EntityRegionChangedMessage message) {
            if(message.Entity == m_AttachedEntity) { return; }

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

        public void PublishEvent(EventMessage msg) {
            m_CurrentRegion.RegionEventChannel.Publish(msg);
        }

        public void PublishMove() {
            m_CurrentRegion.PositionUpdateChannel.Publish(m_AttachedEntity);
        }

        /// <summary>
        /// Region exits area.
        /// </summary>
        private void OnRegionExit(Region region) {
            m_RegionChangedSubscriptions[region].Dispose();
            m_RegionChangedSubscriptions.Remove(region);

            m_RegionEventSubscriptions[region].Dispose();
            m_RegionEventSubscriptions.Remove(region);
        }

        /// <summary>
        /// Event relayed by subscribed region from region's entities.
        /// </summary>
        protected virtual void OnEntityEvent(EventMessage message) {
        }

        /// <summary>
        /// Position update relayed by subscribed region from region's entities.
        /// </summary>
        protected virtual void OnPositionUpdate(Entity updatedEntity) {

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

        public void Dispose() {
            UnsubscribeRegionsNotIn(Enumerable.Empty<Region>());
            ChangeRegion(m_CurrentRegion, null);
            m_SubscriptionManagementFiber.Dispose();
            m_EntityFiber.Dispose();
        }
    }
}
