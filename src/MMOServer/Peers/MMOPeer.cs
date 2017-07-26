using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.Peers {

    using OperationHandlers;

    class MMOPeer : Peer {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public string Name { get; set; }
        private InitialOperationHandler m_InitialOpHandler;

        public MMOPeer(InitRequest initRequest) : base(initRequest) {
            m_InitialOpHandler = new InitialOperationHandler(this);
            SetCurrentOperationHandler(m_InitialOpHandler);
        }
        
        public void ResetPeer() {
            log.InfoFormat("{0} resets peer", Name);
            Name = "";
            m_InitialOpHandler = new InitialOperationHandler(this);
            SetCurrentOperationHandler(m_InitialOpHandler);
        }
    }
}
