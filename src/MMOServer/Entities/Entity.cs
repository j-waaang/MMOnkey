namespace JYW.ThesisMMO.MMOServer {

    using Common.Codes;
    using Common.Entities;
    using Common.Types;
    using ExitGames.Logging;
    using Photon.SocketServer;
    using ActionObjects;
    using System.Collections.Generic;
    using MMOPeer = Peers.MMOPeer;
    using Entities.Attributes;

    /// <summary> 
    /// Entity which is stored in the game world.
    /// </summary>
    internal class Entity {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private Dictionary<AttributeCode, Attribute> m_Attributes = new Dictionary<AttributeCode, Attribute>();

        // TODO: Clean up variables and move to attributes dict.
        // TODO: Restrict variables to be only changeable by game world.
        internal string Name { get; private set; }
        internal Vector Position { get; set; }
        internal float CurHealth { get; set; }
        internal float MaxHealth { get; set; }
        internal ActionState ActionState { get; set; }
        internal MovementState MovementState { get; set; }
        internal WeaponCode AutoAttackType { get; set; }
        internal HashSet<ActionCode> Skills { get; set; }

        private MMOPeer m_Peer;
        private bool m_AiControlled;
        private ActionObject m_ActionObject = null;

        /// <summary> 
        /// Leave out peer if this is a AI controlled enity.
        /// </summary>
        internal Entity(string name, Vector position, Attribute[] attributes, MMOPeer peer) {
            Name = name;
            Position = position;

            if(peer != null) {
                m_Peer = peer;
                m_AiControlled = false;
            }
            else {
                m_AiControlled = true;
            }

            foreach(Attribute attribute in attributes) {
                m_Attributes.Add(attribute.AttributeCode, attribute);
                attribute.SetEntity(this);
            }

            string attributesString = "";
            foreach(AttributeCode code in m_Attributes.Keys) {
                attributesString += code.ToString();
            }
            log.DebugFormat("Entity created w. name {0} w. attributes {1}", Name, attributesString);
        }

        /// <summary> 
        /// Use this constructor if it is controlled by the server's AI Module.
        /// </summary>
        [System.Obsolete]
        internal Entity(string name, Vector position) {
            Name = name;
            Position = position;
            m_AiControlled = true;
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// </summary>
        internal SendResult SendEvent(IEventData eventData, SendParameters sendParameters) {

            if (m_AiControlled) { return SendResult.Ok; }

            return m_Peer.SendEvent(eventData, sendParameters);
        }

        internal bool CanPerformAction(ActionCode action) {
            var actionState = m_Attributes[AttributeCode.ActionState] as ActionStateAttribute;
            if (actionState.ActionState != ActionState.Idle) { return false; }

            return true;
        }

        internal Attribute GetAttribute(AttributeCode attributeCode) {
            return m_Attributes[attributeCode];
        }

        internal void Die() {
            log.InfoFormat("{0} died.", Name);
        }
    }
}
