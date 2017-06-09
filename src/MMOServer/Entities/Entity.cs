namespace JYW.ThesisMMO.MMOServer {

    using Common.Codes;
    using Common.Entities;
    using Common.Types;
    using Entities;
    using Photon.SocketServer;
    using System.Collections.Generic;
    using MMOPeer = Peers.MMOPeer;

    /// <summary> 
    /// Entity which is stored in the game world.
    /// </summary>
    class Entity {

        private Dictionary<AttributeCode, Attribute> m_Attributes = new Dictionary<AttributeCode, Attribute>();

        // TODO: Restrict variables to be only changeable by game world.
        internal string Name { get; private set; }
        internal Vector Position { get; set; }
        internal float CurHealth { get; set; }
        internal float MaxHealth { get; set; }
        internal ActionState ActionState { get; set; }
        internal MovementState MovementState { get; set; }
        internal WeaponCode AutoAttackType { get; set; }
        internal HashSet<CharacterActionCode> Skills { get; set; }

        private MMOPeer m_Peer;
        private bool m_AiControlled;

        /// <summary> 
        /// Use this constructor if it is controlled by a peer/client.
        /// </summary>
        public Entity(MMOPeer peer, string name, Vector position, float maxHealth) {
            Name = name;
            m_Peer = peer;
            m_AiControlled = false;
            MaxHealth = maxHealth;
            CurHealth = maxHealth;
            ActionState = ActionState.Idle;
        }

        /// <summary> 
        /// Use this constructor if it is controlled by the server's AI Module.
        /// </summary>
        public Entity(string name, Vector position) {
            Name = name;
            Position = position;
            m_AiControlled = true;
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// </summary>
        public SendResult SendEvent(IEventData eventData, SendParameters sendParameters) {

            if (m_AiControlled) { return SendResult.Ok; }

            return m_Peer.SendEvent(eventData, sendParameters);
        }

        public bool CanPerformAction(CharacterActionCode action) {
            if(ActionState != ActionState.Idle) { return false; }

            return true;
        }
    }
}
