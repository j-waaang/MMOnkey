using ExitGames.Logging;
using Photon.SocketServer;
using System.Collections.Generic;
using System;

namespace JYW.ThesisMMO.MMOServer {

    using Common.Codes;
    using Common.Types;
    using MMOPeer = Peers.MMOPeer;
    using Entities.Attributes;
    using Events;
    using AI;

    /// <summary> 
    /// Entity which is stored in the game world.
    /// </summary>
    internal class Entity : IDisposable{

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public string Name { get; }
        public MMOPeer Peer { get; }

        protected bool m_AiControlled;

        private readonly Dictionary<AttributeCode, Attribute> m_Attributes = new Dictionary<AttributeCode, Attribute>();
        protected InterestArea m_InterestArea;

        /// <summary> 
        /// Readonly position. Set with Move().
        /// </summary>
        public Vector Position { get; private set; }

        /// <summary> 
        /// Changes the position.
        /// </summary>
        public void Move(Vector position) {
            Position = position;
            m_InterestArea.Update();
            m_InterestArea.PublishMove();
        }

        /// <summary> 
        /// Leave out peer if this is a AI controlled enity.
        /// </summary>
        public Entity(string name, Vector position, Attribute[] attributes, MMOPeer peer) {
            // TODO: Create AIEntity deriving from this class.

            Name = name;
            Position = position;

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

            log.InfoFormat("Created {0} at {1}", name, position);
        }

        protected virtual void SetInterestArea() {
            m_InterestArea = new InterestArea(this);
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

        public virtual void Die() {
            log.InfoFormat("{0} died.", Name);
            if (m_AiControlled) {
                AILooper.Instance.RemoveEntity(this);
            }
            World.Instance.RemoveEntity(Name);
        }

        public virtual void OnAddedToWorld() {
            SetInterestArea();
            m_InterestArea.Update();
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

        public void Dispose() {
            m_InterestArea.Dispose();
        }

        public void PublishEvent(EventMessage msg) {
            m_InterestArea.PublishEvent(msg);
        }
    }
}
