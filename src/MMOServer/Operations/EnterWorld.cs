namespace JYW.ThesisMMO.MMOServer.Operations {

    using Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class EnterWorld : Operation {
        public EnterWorld(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }
    }
}
