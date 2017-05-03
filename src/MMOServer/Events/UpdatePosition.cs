namespace JYW.ThesisMMO.MMOServer.Events {
    using Common.Types;
    using Common.Codes;
    using Photon.SocketServer.Rpc;
    class UpdatePosition {
        [DataMember(Code = (byte)ParameterCode.Username)]
        public string Username { get; set; }

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }
    }
}
