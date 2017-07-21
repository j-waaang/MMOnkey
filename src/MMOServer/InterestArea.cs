using System;
using System.Collections.Generic;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer.Concurrency;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Types;

    /// <summary> 
    /// Subs entity enter/exit events. Pubs current region.
    /// </summary>
    internal class InterestArea : IDisposable{

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        protected readonly Entity m_AttachedEntity;

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
            m_EntityFiber.Start();
        }

        public void Update() {
            var newRegion = World.Instance.GetRegionFromPoint(Center);
            if (m_CurrentRegion != newRegion) {
                ChangeRegion(m_CurrentRegion, newRegion);
            }
        }

        protected virtual void ChangeRegion(Region from, Region to) {
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

        public void PublishEvent(EventMessage msg) {
            m_CurrentRegion.RegionEventChannel.Publish(msg);
        }

        public void PublishMove() {
            m_CurrentRegion.PositionUpdateChannel.Publish(m_AttachedEntity);
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

        public virtual void Dispose() {
            ChangeRegion(m_CurrentRegion, null);
            m_EntityFiber.Dispose();
        }
    }
}
