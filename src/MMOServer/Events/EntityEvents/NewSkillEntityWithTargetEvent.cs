using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events.EntityEvents {

    using JYW.ThesisMMO.Common.Codes;

    class NewSkillEntityWithTargetEvent : NewSkillEntityEvent {

        [DataMember(Code = (byte)ParameterCode.Target)]
        public string Target;
    }
}
