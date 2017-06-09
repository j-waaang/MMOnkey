namespace JYW.ThesisMMO.MMOServer.Requests {
    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class CharacterActionRequest : Operation{
        public CharacterActionRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.CombatActionCode)]
        public CharacterActionCode actionCode { get; set; }
    }
}
