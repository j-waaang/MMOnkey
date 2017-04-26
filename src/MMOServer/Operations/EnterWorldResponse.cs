namespace JYW.ThesisMMO.MMOServer.Operations {
    using Common;
    using Common.Codes;
    using Photon.SocketServer.Rpc;

    class EnterWorldResponse {

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }
    }
}
