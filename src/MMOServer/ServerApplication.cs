namespace JYW.ThesisMMO.MMOServer {
    using Photon.SocketServer;
    class ServerApplication : ApplicationBase {
        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new ClientPeer(initRequest);
        }

        protected override void Setup() {
            CreateWorld();
        }

        protected override void TearDown() {
        }

        private void CreateWorld() {

        }
    }
}
