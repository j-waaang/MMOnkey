namespace JYW.ThesisMMO.MMOServer {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class ClientPeer : Peer {

        private readonly InitialOperationHandler m_InitialOpHandler;

        public ClientPeer(InitRequest initRequest) : base(initRequest) {
            m_InitialOpHandler = new InitialOperationHandler(this);
            SetCurrentOperationHandler(m_InitialOpHandler);
        }
    }
}
