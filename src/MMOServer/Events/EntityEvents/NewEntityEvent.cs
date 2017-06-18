using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;

    abstract public class NewEntityEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Name;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;

        [DataMember(Code = (byte)ParameterCode.EntityType)]
        public int EntityType;
    }
}
