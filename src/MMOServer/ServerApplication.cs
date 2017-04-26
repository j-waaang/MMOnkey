namespace JYW.ThesisMMO.MMOServer {
    using Photon.SocketServer;
    sealed class ServerApplication : ApplicationBase {
        private World m_World;

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new ClientPeer(initRequest);
        }
        protected override void Setup() {
            CreateWorld();
        }
        private void CreateWorld() {
            m_World = new World();
        }
        protected override void TearDown() {
        }
    }
}
