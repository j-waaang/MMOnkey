using ExitGames.Logging;
using Photon.SocketServer;
using System.Collections.Generic;

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
    internal class Entity {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private Dictionary<AttributeCode, Attribute> m_Attributes = new Dictionary<AttributeCode, Attribute>();

        // TODO: Add quick references for attributes.
        public string Name { get; private set; }
        public Vector Position { get; set; }

        protected MMOPeer m_Peer;
        protected bool m_AiControlled;

        /// <summary> 
        /// Leave out peer if this is a AI controlled enity.
        /// </summary>
        public Entity(string name, Vector position, Attribute[] attributes, MMOPeer peer) {
            Name = name;
            Position = position;

            if (peer != null) {
                m_Peer = peer;
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

            World.Instance.AddEntity(this);
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// </summary>
        public SendResult SendEvent(IEventData eventData, SendParameters sendParameters) {

            if (m_AiControlled) { return SendResult.Ok; }

            return m_Peer.SendEvent(eventData, sendParameters);
        }

        public bool CanPerformAction(ActionCode action) {
            var actionState = m_Attributes[AttributeCode.ActionState] as ActionStateAttribute;
            if (actionState.ActionState != ActionCode.Idle) { return false; }

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

        public virtual IEventData GetNewEntityEventData() {
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
