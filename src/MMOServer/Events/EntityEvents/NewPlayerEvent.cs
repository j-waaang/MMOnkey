using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;

    class NewPlayerEvent : NewEntityEvent{

        [DataMember(Code = (byte)ParameterCode.MaxHealth)]
        public int MaxHealth;

        [DataMember(Code = (byte)ParameterCode.CurrentHealth)]
        public int CurrentHealth;
    }
}
