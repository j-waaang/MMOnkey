namespace JYW.ThesisMMO.MMOServer.Operations.Responses {
    using Common.Types;
    using Common.Codes;
    using Photon.SocketServer.Rpc;

    class EnterWorldResponse {
        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }
    }
}
