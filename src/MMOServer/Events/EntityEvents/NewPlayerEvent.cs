using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;

    class NewPlayerEvent : NewEntityEvent{

        [DataMember(Code = (byte)ParameterCode.MaxHealth)]
        public int MaxHealth;

        [DataMember(Code = (byte)ParameterCode.CurrentHealth)]
        public int CurrentHealth;

        public bool SnapshotEquals(NewPlayerEvent other) {
            return base.SnapshotEquals(other) && MaxHealth == other.MaxHealth && CurrentHealth == other.CurrentHealth;
        }

        public override bool SnapshotEquals(NewEntityEvent other) {
            var cast = other as NewPlayerEvent;
            if(cast == null) { return false; }
            return SnapshotEquals(cast);
        }
    }
}
