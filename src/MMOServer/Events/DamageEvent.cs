namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Photon.SocketServer.Rpc;

    class DamageEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.Damage)]
        public int Damage;

        [DataMember(Code = (byte)ParameterCode.CurrentHealth)]
        public int CurrenHealth;
    }
}
