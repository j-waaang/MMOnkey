namespace JYW.ThesisMMO.MMOServer.Peers.OperationHandlers {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Codes;
    using Operations;
    using Operations.Responses;
    using ExitGames.Logging;

    class EntityOperationHandler : IOperationHandler {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private MMOPeer m_Peer;

        internal EntityOperationHandler(MMOPeer peer) {
            m_Peer = peer;
            World.Instance.NotifyEntityAboutExistingPlayers(m_Peer.Username);
        }

        public void OnDisconnect(PeerBase peer) {
            World.Instance.RemoveEntity(m_Peer.Username);
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

            var operation = new MoveRequest(peer.Protocol, request);

            if (!operation.IsValid) {
                return new OperationResponse(request.OperationCode) {
                    ReturnCode = (int)ReturnCode.InvalidOperationParameter,
                    DebugMessage = operation.GetErrorMessage()
                };
            }

            World.Instance.MoveEntity(m_Peer.Username, operation.Position);

            //We don't respond directly on movement. World cache updates movement to clients.
            return null;
        }
    }
}
