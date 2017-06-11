namespace JYW.ThesisMMO.MMOServer.Peers.OperationHandlers {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Types;
    using Common.Codes;
    using Operations.Responses;
    using Peers;
    using Requests;
    using ExitGames.Logging;

    class InitialOperationHandler : IOperationHandler {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private MMOPeer m_Peer;

        internal InitialOperationHandler(MMOPeer peer) {
            m_Peer = peer;
        }

        public void OnDisconnect(PeerBase peer) {
            if(m_Peer.Username != null) {
                World.Instance.RemoveEntity(m_Peer.Username);
            }
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

            var operation = new EnterWorldRequest(peer.Protocol, request);

            if (!operation.IsValid) {
                return new OperationResponse(request.OperationCode) {
                    ReturnCode = (int)ReturnCode.InvalidOperationParameter,
                    DebugMessage = operation.GetErrorMessage()
                };
            }

            DebugCharacter(operation);

            var entity = EntityFactory.CreatePeerControlledEntity(m_Peer, operation);

            // TODO: Think about a different place to store the username.
            m_Peer.Username = operation.Name;

            // Send entered world response.
            var responseData = new EnterWorldResponse {
                Position = entity.Position
            };
            var response = new OperationResponse(request.OperationCode, responseData) {
                ReturnCode = (short)ReturnCode.OK
            };
            m_Peer.SendOperationResponse(response, sendParameters);

            // Must happen after OK is sent to server.
            // Notify other peers about new player by adding the entity to the world cache.
            //TODO: Missing checks when adding new entity. E.g. is there already an entity with the same name.
            World.Instance.AddEntity(entity);

            return null;
        }

        private void DebugCharacter(EnterWorldRequest request) {
            var dbgString = "New character";
            dbgString += " " + request.Name;

            foreach (int skill in request.Skills) {
                dbgString += " " + (ActionCode)skill;
            }

            log.DebugFormat(dbgString);
        }

        private OperationResponse OperationReadyToReceiveGameEventsRequest(PeerBase peer, OperationRequest request, SendParameters sendParameters) {
            m_Peer.SetCurrentOperationHandler(new EntityOperationHandler(m_Peer));
            return null;
        }
    }
}
