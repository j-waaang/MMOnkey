namespace JYW.ThesisMMO.MMOServer {
    using System;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    class InitialOperationHandler : IOperationHandler {
        public void OnDisconnect(PeerBase peer) {
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            throw new NotImplementedException();
        }
    }
}
