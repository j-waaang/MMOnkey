namespace JYW.ThesisMMO.MMOServer {
    using Common.Codes;
    using Common.Types;
    using System.Collections.Generic;
    using MMOPeer = Peers.MMOPeer;
    class PlayerEntity {

        public string Username { get; private set; }
        public MMOPeer Peer { get; private set; }
        public Vector Position { get; set; }
        public HashSet<SkillCodes> Skills { get; set; }
        public AutoAttackCodes AutoAttackType { get; set; }

        public PlayerEntity(MMOPeer peer, string username, Vector position) {
            Username = username;
            Peer = peer;
        }
    }
}
