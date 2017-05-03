namespace JYW.ThesisMMO.MMOServer {
    using Common.Types;
    using MMOPeer = Peers.MMOPeer;
    class PlayerEntity {
        public string Username { get; private set; }
        public MMOPeer Peer { get; private set; }
        public Vector Position { get; set; }

        public PlayerEntity(MMOPeer peer, string username, Vector position) {
            Username = username;
            Peer = peer;
        }
    }
}
