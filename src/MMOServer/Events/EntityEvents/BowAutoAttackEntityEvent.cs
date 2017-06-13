namespace JYW.ThesisMMO.MMOServer.Events.EntityEvents {

    using Photon.SocketServer.Rpc;

    using JYW.ThesisMMO.Common.Codes;

    class BowAutoAttackEntityEvent : EntityEvent {

        [DataMember(Code = (byte)ParameterCode.Target)]
        public string Target;

    }
}
