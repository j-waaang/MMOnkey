namespace JYW.ThesisMMO.MMOServer.Requests {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class EnterWorldRequest : Operation {

        public EnterWorldRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Username)]
        public string Username { get; set; }

        [DataMember(Code = (byte)ParameterCode.AutoAttackType)]
        public int AutoAttackType { get; set; }

        [DataMember(Code = (byte)ParameterCode.Skill)]
        public int Skill1 { get; set; }

        [DataMember(Code = (byte)ParameterCode.Skill)]
        public int Skill2 { get; set; }

        [DataMember(Code = (byte)ParameterCode.Skill)]
        public int Skill3 { get; set; }
    }
}
