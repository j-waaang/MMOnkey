namespace JYW.ThesisMMO.MMOServer {

    using Common.Codes;
    using Common.Entities;
    using Common.Types;
    using Entities;
    using ExitGames.Logging;
    using Photon.SocketServer;
    using ActionObjects;
    using System.Collections.Generic;
    using MMOPeer = Peers.MMOPeer;

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
        internal HashSet<CharacterActionCode> Skills { get; set; }

        private MMOPeer m_Peer;
        private bool m_AiControlled;
        private ActionObject m_ActionObject = null;

        /// <summary> 
        /// Use this constructor if it is controlled by a peer/client.
        /// </summary>
        internal Entity(MMOPeer peer, string name, Vector position, float maxHealth) {
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

        internal bool CanPerformAction(CharacterActionCode action) {
            if (ActionState != ActionState.Idle) { return false; }

            return true;
        }

        internal void AttachActionObject(ActionObject actionObject) {
            if (m_ActionObject != null) {
                log.DebugFormat(
                    "Trying to attach {0} to {1} failed. Entity still has a ActionObject of type {2}.",
                    actionObject.GetType(),
                    Name,
                    m_ActionObject.GetType());
                return;
            }
            m_ActionObject = actionObject;
        }
    }
}
