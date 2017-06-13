namespace JYW.ThesisMMO.MMOServer.Events.EntityEvents {

    using Photon.SocketServer.Rpc;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    class EntityEvent {
        [DataMember(Code = (byte)ParameterCode.ActionCode)]
        public int ActionCode;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;
    }
}
