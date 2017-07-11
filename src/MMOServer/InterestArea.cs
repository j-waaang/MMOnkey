using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.Common.Types;
using JYW.ThesisMMO.MMOServer.Events;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JYW.ThesisMMO.MMOServer {
    internal class InterestArea {

        public readonly object SyncRoot = new object();

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IFiber m_SubscriptionManagementFiber = new PoolFiber();
        private readonly HashSet<Region> m_Regions = new HashSet<Region>();
        private readonly Dictionary<Region, IDisposable> m_RegionChangedSubscriptions = new Dictionary<Region, IDisposable>();
        private readonly Dictionary<Region, IDisposable> m_RegionEventSubscriptions = new Dictionary<Region, IDisposable>();

        private readonly Entity m_AttachedEntity;
        private readonly Vector m_CenterToMin;

        private BoundingBox2D m_Focus {
            get {
                return new BoundingBox2D(m_AttachedEntity.Position + m_CenterToMin, m_AttachedEntity.Position - m_CenterToMin);
            }
        }

        #region public methods
        public InterestArea(Entity attachedEntity, float interestRadius) {
            m_AttachedEntity = attachedEntity;
            m_CenterToMin = m_AttachedEntity.Position - new Vector(interestRadius, interestRadius);
            m_SubscriptionManagementFiber.Start();
        }

        /// <summary>
        /// Subs and unsubs from regions depending on focus.
        /// Should be called when entering a new region or the entity moved.
        /// </summary>
        public void UpdateRegionSubscription() {
            var focusedRegions = World.Instance.GetRegions(m_Focus);
            SubscribeRegions(focusedRegions);
            UnsubscribeRegionsNotIn(focusedRegions);

            string subbedregions = "";
            foreach(var region in m_Regions) {
                subbedregions += region.ToString() + "/n";
            }

            log.InfoFormat("{0} subed to regions {1}", m_AttachedEntity.Name, subbedregions);
        }
        #endregion public methods
        #region private methods
        private void SubscribeRegions(IEnumerable<Region> newRegions) {
            foreach (Region r in newRegions) {
                if (m_Regions.Contains(r)) { continue; }
                m_Regions.Add(r);
                SubscribeToRegion(r);
                r.RequestInfoInRegionChannel.Publish(this);
            }
        }

        private void UnsubscribeRegionsNotIn(IEnumerable<Region> regionsToSurvive) {
            var toUnsubscribeEnumerable = m_Regions.Except(regionsToSurvive);
            var toUnsubscribe = toUnsubscribeEnumerable.ToArray(); // make copy

            foreach (var r in toUnsubscribe) {
                m_Regions.Remove(r);
                OnRegionExit(r);
                r.RequestRegionExitInfoChannel.Publish(this);
            }
        }

        private void SubscribeToRegion(Region region) {
            var subscription = region.EntityRegionChangedChannel.Subscribe(m_SubscriptionManagementFiber, OnEntityRegionChange);
            m_RegionChangedSubscriptions.Add(region, subscription);

            subscription = region.RegionEventChannel.Subscribe(m_AttachedEntity.Peer.RequestFiber, OnItemEvent);
            m_RegionEventSubscriptions[region] = subscription;
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
        private void OnItemExit(Entity entity) {
            var ev = new RemovePlayerEvent() {
                Username = entity.Name,
            };
            IEventData eventData = new EventData((byte)EventCode.RemovePlayer, ev);
            m_AttachedEntity.SendEvent(eventData);
        }
        #endregion private methods
    }
}
