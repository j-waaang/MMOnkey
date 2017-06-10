namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class BowAutoAttackRequest : ActionObject {

        public BowAutoAttackRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }
    }
}
