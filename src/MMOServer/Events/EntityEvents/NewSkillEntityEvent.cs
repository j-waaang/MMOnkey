using JYW.ThesisMMO.Common.Codes;
using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.Events.EntityEvents {
    class NewSkillEntityEvent : NewEntityEvent{

        [DataMember(Code = (byte)ParameterCode.ActionCode)]
        public int ActionCode;

    }
}
