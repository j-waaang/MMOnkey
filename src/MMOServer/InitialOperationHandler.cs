namespace JYW.ThesisMMO.MMOServer {

    using System;
    using System.ComponentModel;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Common.Codes;
    using Common.Entities;
    using Operations;
    using Common;

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
                    return NegativeResponse(operationRequest, ReturnCode.OperationNotAllowed);
                default:
                    return NegativeResponse(operationRequest, ReturnCode.OperationNotSupported);
            }
        }

        private OperationResponse NegativeResponse(OperationRequest operationRequest, ReturnCode returnCode) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short) returnCode,
                DebugMessage = returnCode.ToString() + ": " + operationRequest.OperationCode
            };
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
            World.Instance.AddEntity(entity);


            var responseObject = new EnterWorldResponse {
                Position = position
            };

            return new OperationResponse(request.OperationCode, responseObject) {
                ReturnCode = (short)ReturnCode.OK
            };
        }

        private Vector GetRandomWorldPosition() {
            return Vector.Zero;
        }
    }
}
