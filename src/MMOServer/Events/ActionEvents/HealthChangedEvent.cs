namespace JYW.ThesisMMO.MMOServer.Events.ActionEvents {

    using Common.Codes;
    using Photon.SocketServer.Rpc;

    class HealthChangedEvent : ActionEvent{

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.Damage)]
        public int Damage;

        [DataMember(Code = (byte)ParameterCode.CurrentHealth)]
        public int CurrenHealth;
    }
}
