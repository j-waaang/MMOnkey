namespace JYW.ThesisMMO.MMOServer.Peers.OperationHandlers {
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Types;
    using Common.Codes;
    using Common.Entities;
    using Operations;
    using Operations.Responses;
    using ClientPeer = Peers.ClientPeer;
    class EntityOperationHandler : IOperationHandler {
        private ClientPeer m_Peer;
        internal EntityOperationHandler(ClientPeer peer) {
            m_Peer = peer;
        }
        public void OnDisconnect(PeerBase peer) {
            peer.Dispose();
        }
        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            switch ((OperationCode)operationRequest.OperationCode) {
                case OperationCode.EnterWorld:
                    return DefaultResponses.CreateNegativeResponse(operationRequest, ReturnCode.OperationNotAllowed);
                case OperationCode.Move:
                    return OperationMove(peer, operationRequest, sendParameters);
                default:
                    return DefaultResponses.CreateNegativeResponse(operationRequest, ReturnCode.OperationNotSupported);
            }
        }

        private OperationResponse OperationMove(PeerBase peer, OperationRequest request, SendParameters sendParameters) {

            var operation = new Move(peer.Protocol, request);

            if (!operation.IsValid) {
                return new OperationResponse(request.OperationCode) {
                    ReturnCode = (int)ReturnCode.InvalidOperationParameter,
                    DebugMessage = operation.GetErrorMessage()
                };
            }

            World.Instance.entityCache.MoveEntity(m_Peer.Username, operation.Position);

            //We don't respond directly on movement. World cache updates movement to clients.
            return null;
        }
    }
}
