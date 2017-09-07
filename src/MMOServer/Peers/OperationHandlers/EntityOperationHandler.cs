namespace JYW.ThesisMMO.MMOServer.Peers.OperationHandlers {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using ExitGames.Logging;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Operations.Responses;
    using JYW.ThesisMMO.MMOServer.Requests;
    using JYW.ThesisMMO.MMOServer.ActionObjects;
    using AI;

    class EntityOperationHandler : IOperationHandler {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private ActionObjectFactory m_ActionObjectFactory = new ActionObjectFactory();
        private MMOPeer m_Peer;

        internal EntityOperationHandler(MMOPeer peer) {
            m_Peer = peer;
        }

        public void OnDisconnect(PeerBase peer) {
            World.Instance.RemoveEntity(m_Peer.Name);
            //World.Instance.DisconnectPeer(m_Peer.Name);
            peer.Dispose();
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            switch ((OperationCode)operationRequest.OperationCode) {
                case OperationCode.EnterWorld:
                    return DefaultResponses.CreateNegativeResponse(operationRequest, ReturnCode.OperationNotAllowed);
                case OperationCode.Move:
                    return OperationMove(peer, operationRequest);
                case OperationCode.CharacterAction:
                    return OperationCharacterAction(peer, operationRequest);
                case OperationCode.StartAiLoop:
                    AILooper.Instance.Start();
                    return null;
                case OperationCode.ToggleAiLoop:
                    AILooper.Instance.ToggleLoop();
                    return null;
                default:
                    return DefaultResponses.CreateNegativeResponse(operationRequest, ReturnCode.OperationNotSupported);
            }
        }

        private OperationResponse OperationCharacterAction(PeerBase peer, OperationRequest request) {
            log.DebugFormat("Received character action from {0}", m_Peer.Name);
            var actionObject = m_ActionObjectFactory.CreateActionObject(m_Peer.Name, peer.Protocol, request);

            if(actionObject == null) {
                return DefaultResponses.CreateNegativeResponse(request, m_ActionObjectFactory.LastCreationFailReason);
            }

            if (!actionObject.CheckPrerequesite()) {
                log.InfoFormat("Prerequisite check failed.");
                return DefaultResponses.CreateActionRequestResponse(request, ReturnCode.Declined);
            }

            actionObject.StartAction();

            return DefaultResponses.CreateActionRequestResponse(request, ReturnCode.OK);
        }

        private OperationResponse OperationMove(PeerBase peer, OperationRequest request) {

            var operation = new MoveRequest(peer.Protocol, request);

            if (!operation.IsValid) {
                return new OperationResponse(request.OperationCode) {
                    ReturnCode = (int)ReturnCode.InvalidOperationParameter,
                    DebugMessage = operation.GetErrorMessage()
                };
            }

            World.Instance.MoveEntity(m_Peer.Name, operation.Position);

            //We don't respond directly on movement. World cache updates movement to clients.
            return null;
        }
    }
}
