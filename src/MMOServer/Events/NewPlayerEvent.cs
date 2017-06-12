namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer.Rpc;

    class NewPlayerEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Name;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;

        [DataMember(Code = (byte)ParameterCode.MaxHealth)]
        public int MaxHealth;

        [DataMember(Code = (byte)ParameterCode.CurrentHealth)]
        public int CurrentHealth;
    }
}
