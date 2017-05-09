namespace JYW.ThesisMMO.MMOServer.Peers.OperationHandlers {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Types;
    using Common.Codes;
    using Operations;
    using Operations.Responses;
    using Peers;

    class InitialOperationHandler : IOperationHandler {

        private MMOPeer m_Peer;

        internal InitialOperationHandler(MMOPeer peer) {
            m_Peer = peer;
        }

        public void OnDisconnect(PeerBase peer) {
            peer.Dispose();
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            switch ((OperationCode)operationRequest.OperationCode) {
                case OperationCode.EnterWorld:
                    return OperationEnterWorld(peer, operationRequest, sendParameters);
                case OperationCode.ReadyToReceiveGameEvents:
                    return OperationReadyToReceiveGameEventsRequest(peer, operationRequest, sendParameters);
                case OperationCode.Move:
                    return DefaultResponses.CreateNegativeResponse(operationRequest, ReturnCode.OperationNotAllowed);
                default:
                    return DefaultResponses.CreateNegativeResponse(operationRequest, ReturnCode.OperationNotSupported);
            }
        }

        private OperationResponse OperationEnterWorld(PeerBase peer, OperationRequest request, SendParameters sendParameters) {

            var operation = new EnterWorld(peer.Protocol, request);

            if (!operation.IsValid) {
                return new OperationResponse(request.OperationCode) {
                    ReturnCode = (int)ReturnCode.InvalidOperationParameter,
                    DebugMessage = operation.GetErrorMessage()
                };
            }

            // TODO: Think about where characters should enter the world.
            var position = GetRandomWorldPosition();
            var entity = new PlayerEntity(m_Peer, operation.Username, position);

            // TODO: Think about a different place to store the username.
            m_Peer.Username = operation.Username;

            // Send entered world response.
            var responseData = new EnterWorldResponse {
                Position = position
            };
            var response = new OperationResponse(request.OperationCode, responseData) {
                ReturnCode = (short)ReturnCode.OK
            };
            m_Peer.SendOperationResponse(response, sendParameters);

            // Notify other peers about new player by adding the entity to the world cache.
            //TODO: Missing checks when adding new entity. E.g. is there already an entity with the same name.
            World.Instance.AddEntity(entity);

            return null;
        }

        private OperationResponse OperationReadyToReceiveGameEventsRequest(PeerBase peer, OperationRequest request, SendParameters sendParameters) {
            m_Peer.SetCurrentOperationHandler(new EntityOperationHandler(m_Peer));
            return null;
        }

        private Vector GetRandomWorldPosition() {
            // TODO: Actually return a random position.
            return Vector.Zero;
        }
    }
}
