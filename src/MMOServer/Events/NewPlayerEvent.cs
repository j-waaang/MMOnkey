namespace JYW.ThesisMMO.MMOServer.Events {
    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer.Rpc;

    internal class NewPlayerEvent : DataContract{
        internal NewPlayerEvent(string username, Vector position) {
            Username = username;
            Position = position;
        }

        [DataMember(Code = (byte)ParameterCode.Username)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;
    }
}
