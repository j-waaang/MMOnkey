namespace JYW.ThesisMMO.MMOServer.Requests {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class EnterWorldRequest : Operation {

        public EnterWorldRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.CharacterName)]
        public string Name { get; set; }

        [DataMember(Code = (byte)ParameterCode.Weapon)]
        public int Weapon { get; set; }

        [DataMember(Code = (byte)ParameterCode.Skill)]
        public int[] Skills { get; set; }
    }
}
