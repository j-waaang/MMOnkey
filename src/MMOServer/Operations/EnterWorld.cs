namespace JYW.ThesisMMO.MMOServer.Operations {
    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class EnterWorld : Operation {
        public EnterWorld(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Username)]
        public string Username { get; set; }
    }
}
