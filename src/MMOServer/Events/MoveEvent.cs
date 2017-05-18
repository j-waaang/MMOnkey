namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer.Rpc;

    class MoveEvent {

        [DataMember(Code = (byte)ParameterCode.Username)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;
    }
}
