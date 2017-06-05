namespace JYW.ThesisMMO.MMOServer.Requests {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class AutoAttackRequest : Operation {

        public AutoAttackRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.CharacterName)]
        public string Target { get; set; }
    }
}
