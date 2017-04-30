namespace JYW.ThesisMMO.MMOServer.Peers.OperationHandlers {
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Types;
    using Common.Codes;
    using Common.Entities;
    using Operations;
    using Operations.Responses;
    using ClientPeer = Peers.ClientPeer;
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
                    return OperationEnterWorld(peer, operationRequest, sendParameters);
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

            //TODO: Think about where characters should enter the world.
            var position = GetRandomWorldPosition();
            var entity = new CharacterEntity(operation.Username, position);

            //TODO: Missing checks when adding new entity. E.g. is there already an entity with the same name.
            World.Instance.entityCache.AddEntity(entity);

            m_Peer.Username = operation.Username;
            m_Peer.SetCurrentOperationHandler(new EntityOperationHandler(m_Peer));

            var responseObject = new EnterWorldResponse {
                Position = position
            };

            return new OperationResponse(request.OperationCode, responseObject) {
                ReturnCode = (short)ReturnCode.OK
            };
        }
        private Vector GetRandomWorldPosition() {
            //TODO: Actually return a random position.
            return Vector.Zero;
        }
    }
}
