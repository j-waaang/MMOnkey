namespace JYW.ThesisMMO.MMOServer {
    using Photon.SocketServer;
    class MMOServerApplication : ApplicationBase {
        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(initRequest);
        }

        protected override void Setup() {
        }

        protected override void TearDown() {
        }
    }
}
