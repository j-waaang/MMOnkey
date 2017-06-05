namespace JYW.ThesisMMO.MMOServer.Requests {

    using Common.Types;
    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class MoveRequest : Operation{
        public MoveRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }
    }
}
