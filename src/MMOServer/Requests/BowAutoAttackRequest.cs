namespace JYW.ThesisMMO.MMOServer.Requests {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class BowAutoAttackRequest : CharacterActionRequest {

        public BowAutoAttackRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }
    }
}
