using ExitGames.Logging;
using Photon.SocketServer;
using System.Collections.Generic;
using System;
using Photon.SocketServer.Concurrency;
using ExitGames.Concurrency.Fibers;

namespace JYW.ThesisMMO.MMOServer {

    using Common.Codes;
    using Common.Types;
    using MMOPeer = Peers.MMOPeer;
    using Entities.Attributes;
    using Events;
    using AI;
    using System.Diagnostics;

    /// <summary> 
    /// Entity which is stored in the game world.
    /// </summary>
    internal class Entity {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public string Name { get; }

        private IFiber m_EntityFiber;
        public virtual IFiber Fiber {
            get {
                if (m_EntityFiber == null) {
                    m_EntityFiber = new PoolFiber();
                    m_EntityFiber.Start();
                }
                return m_EntityFiber;
            }
        }
        public MMOPeer Peer { get; }
        private const float InterestRadius = 0f;

        protected bool m_AiControlled;

        private readonly Dictionary<AttributeCode, Attribute> m_Attributes = new Dictionary<AttributeCode, Attribute>();
        private Region m_CurrentRegion;
        protected InterestArea m_InterestArea;

        private IDisposable m_RegionSubscription;

        /// <summary> 
        /// Readonly position. Set with Move().
        /// </summary>
        public Vector Position { get; private set; }

        /// <summary> 
        /// Changes the position.
        /// </summary>
        public virtual void Move(Vector position) {
            Position = position;
        }

        /// <summary> 
        /// Leave out peer if this is a AI controlled enity.
        /// </summary>
        public Entity(string name, Vector position, Attribute[] attributes, MMOPeer peer) {
            Name = name;
            Position = position;

            log.InfoFormat("Created {0} entity at {1}", name, position);

            if (peer != null) {
                Peer = peer;
                m_AiControlled = false;
            }
            else {
                m_AiControlled = true;
            }

            if (attributes != null) {
                foreach (Attribute attribute in attributes) {
                    m_Attributes.Add(attribute.AttributeCode, attribute);
                    attribute.SetEntity(this);
                }
            }

            string attributesString = "";
            foreach (AttributeCode code in m_Attributes.Keys) {
                attributesString += code.ToString();
            }
            log.DebugFormat("Entity created w. name {0} w. attributes {1}", Name, attributesString);
        }

        protected virtual void SetInterestArea() {
            m_InterestArea = new InterestArea(this, InterestRadius);
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// </summary>
        public virtual SendResult SendEvent(IEventData eventData, SendParameters sendParameters) {
            return SendResult.Ok;
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// Default sendparameters are used. Reliable. Channel 0
        /// </summary>
        public virtual SendResult SendEvent(IEventData eventData) {
            return SendResult.Ok;
        }

        public bool IsIdle() {
            var actionState = m_Attributes[AttributeCode.ActionState] as ActionStateAttribute;
            if (actionState.GetActionState() != ActionCode.Idle) { return false; }

            return true;
        }

        public bool CanPerformAction(ActionCode action) {
            var actionState = m_Attributes[AttributeCode.ActionState] as ActionStateAttribute;
            if (actionState.GetActionState() != ActionCode.Idle) { return false; }

            return true;
        }

        public Attribute GetAttribute(AttributeCode attributeCode) {
            Attribute attribute = null;
            m_Attributes.TryGetValue(attributeCode, out attribute);

            return attribute;
        }

        public void Die() {
            log.InfoFormat("{0} died.", Name);
            if (m_AiControlled) {
                AILooper.Instance.RemoveEntity(this);
            }
            World.Instance.RemoveEntity(Name);
        }

        public virtual void OnAddedToWorld() {
            SetInterestArea();
            UpdateInterestManagment();
        }

        private void UpdateInterestManagment() {

            var newRegion = World.Instance.GetRegionFromPoint(Position);
            if (m_CurrentRegion != newRegion) {
                ChangeRegion(m_CurrentRegion, newRegion);
            }

            m_InterestArea.UpdateRegionSubscription();
        }

        private void ChangeRegion(Region from, Region to) {
            m_CurrentRegion = to;

            if (m_RegionSubscription != null) {
                m_RegionSubscription.Dispose();
            }

            var msg = new EntityRegionChangedMessage(from, to, this);
            if (from != null) {
                from.EntityRegionChangedChannel.Publish(msg);
            }

            Debug.Assert(to != null, "Cannot change to null region.");

            to.EntityRegionChangedChannel.Publish(msg);
            m_RegionSubscription = new UnsubscriberCollection(
                //this.EventChannel.Subscribe(this.Fiber, (m) => newRegion.ItemEventChannel.Publish(m)), // route events through region to interest area

                // region entered interest area fires message to let item notify interest area about enter
                to.RequestInfoInRegionChannel.Subscribe(Fiber, (m) => { m.OnEntityEnter(this); }),

                // region exited interest area fires message to let item notify interest area about exit
                to.RequestRegionExitInfoChannel.Subscribe(Fiber, (m) => { m.OnEntityExit(this); })
            );
        }

        public virtual IEventData GetEntitySnapshot() {
            var newPlayerEv = new NewPlayerEvent() {
                Name = Name,
                Position = Position,
                CurrentHealth = ((IntAttribute)GetAttribute(AttributeCode.Health)).GetValue(),
                MaxHealth = ((IntAttribute)GetAttribute(AttributeCode.MaxHealth)).GetValue()
            };
            return new EventData((byte)EventCode.NewPlayer, newPlayerEv);
        }
    }
}
