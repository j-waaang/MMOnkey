namespace JYW.ThesisMMO.MMOServer {
    using Common.Types;
    using Photon.SocketServer;
    using ClientPeer = Peers.ClientPeer;
    using Protocol = Common.Types.Protocol;
    sealed class ServerApplication : ApplicationBase {
        private World m_World;

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new ClientPeer(initRequest);
        }
        protected override void Setup() {
            RegisterTypes();
            CreateWorld();
        }
        private static void RegisterTypes() {
            Photon.SocketServer.Protocol.TryRegisterCustomType(
                typeof(Vector),
                (byte)Protocol.CustomTypeCodes.Vector,
                Protocol.SerializeVector,
                Protocol.DeserializeVector);
        }
        private void CreateWorld() {
            m_World = new World();
        }
        protected override void TearDown() {
        }
    }
}
