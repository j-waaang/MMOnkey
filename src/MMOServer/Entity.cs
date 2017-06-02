namespace JYW.ThesisMMO.MMOServer {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer;
    using System.Collections.Generic;
    using MMOPeer = Peers.MMOPeer;

    /// <summary> 
    /// Entity which is stored in the game world.
    /// </summary>
    class Entity {

        // TODO: Restrict variables to be only changeable by game world.

        public string Name { get; private set; }
        public Vector Position { get; set; }
        public HashSet<SkillCodes> Skills { get; set; }
        public AutoAttackCodes AutoAttackType { get; set; }

        private MMOPeer m_Peer;
        private bool m_AiControlled;

        /// <summary> 
        /// Use this constructor if it is controlled by a peer/client.
        /// </summary>
        public Entity(MMOPeer peer, string name, Vector position) {
            Name = name;
            m_Peer = peer;
            m_AiControlled = false;
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
    }
}
