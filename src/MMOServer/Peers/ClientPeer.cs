namespace JYW.ThesisMMO.MMOServer.Peers {

    using OperationHandlers;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class ClientPeer : Peer {

        public string Username { get; set; }
        private readonly InitialOperationHandler m_InitialOpHandler;

        public ClientPeer(InitRequest initRequest) : base(initRequest) {
            m_InitialOpHandler = new InitialOperationHandler(this);
            SetCurrentOperationHandler(m_InitialOpHandler);
        }
    }
}
