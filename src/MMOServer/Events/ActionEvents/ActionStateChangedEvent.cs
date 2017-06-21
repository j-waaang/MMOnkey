namespace JYW.ThesisMMO.MMOServer.Events.ActionEvents {

    using Photon.SocketServer.Rpc;

    using JYW.ThesisMMO.Common.Codes;
    using Common.Types;

    class ActionStateChangedEvent : ActionEvent {
        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.ActionState)]
        public int ActionState;

        [DataMember(Code = (byte)ParameterCode.LookDirection)]
        public Vector LookDirection;
    }
}
