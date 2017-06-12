namespace JYW.ThesisMMO.MMOServer.Peers {

    using OperationHandlers;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Types;

    class MMOPeer : Peer {

        public string Name { get; set; }
        private readonly InitialOperationHandler m_InitialOpHandler;

        public MMOPeer(InitRequest initRequest) : base(initRequest) {
            m_InitialOpHandler = new InitialOperationHandler(this);
            SetCurrentOperationHandler(m_InitialOpHandler);
        }

        public void UpdatePosition(string username, Vector position) {
        }
    }
}
