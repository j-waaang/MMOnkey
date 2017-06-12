namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Photon.SocketServer.Rpc;

    class RemovePlayerEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Username;
    }
}
