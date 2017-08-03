using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer;

    abstract public class NewEntityEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Name;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;

        [DataMember(Code = (byte)ParameterCode.EntityType)]
        public int EntityType;

        public virtual bool SnapshotEquals(NewEntityEvent other) {
            return Name == other.Name && EntityType == other.EntityType;
        }

        public virtual IEventData ToEventData() {
            return new EventData((byte)EventCode.NewPlayer, this);
        }
    }
}
