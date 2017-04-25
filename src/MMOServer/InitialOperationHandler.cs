namespace JYW.ThesisMMO.MMOServer {

    using System;
    using System.ComponentModel;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common;
    using Operations;

    class InitialOperationHandler : IOperationHandler {

        private ClientPeer m_Peer;

        internal InitialOperationHandler(ClientPeer peer) {
            m_Peer = peer;
        }

        public void OnDisconnect(PeerBase peer) {
            peer.Dispose();
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            switch ((OperationCode)operationRequest.OperationCode) {
                case OperationCode.EnterWorld:
                    break;
                case OperationCode.Move:
                    throw new InvalidOperationException();
                default:
                    throw new InvalidEnumArgumentException();
            }
            throw new InvalidOperationException();
        }

        public OperationResponse OperationEnterWorld(PeerBase peer, OperationRequest request, SendParameters sendParameters) {
            var operation = new EnterWorld(peer.Protocol, request);

            throw new NotImplementedException();
        }
    }
}
