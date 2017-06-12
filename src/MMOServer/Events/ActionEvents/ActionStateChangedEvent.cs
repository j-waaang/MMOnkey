namespace JYW.ThesisMMO.MMOServer.Events.ActionEvents {

    using Photon.SocketServer.Rpc;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;

    class ActionStateChangedEvent : ActionEvent {
        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.ActionState)]
        public int ActionState;
    }
}
