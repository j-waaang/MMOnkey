using JYW.ThesisMMO.Common.Codes;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events.EntityEvents {
    class NewSkillEntityEvent : NewEntityEvent{

        [DataMember(Code = (byte)ParameterCode.ActionCode)]
        public int ActionCode;

        public bool SnapshotEquals(NewSkillEntityEvent other) {
            return base.SnapshotEquals(other) && ActionCode == other.ActionCode;
        }

        public override bool SnapshotEquals(NewEntityEvent other) {
            var cast = other as NewSkillEntityEvent;
            if(cast == null) { return false; }
            return SnapshotEquals(cast);
        }

        public override IEventData ToEventData() {
            return new EventData((byte)EventCode.NewEntity, this);
        }
    }
}
