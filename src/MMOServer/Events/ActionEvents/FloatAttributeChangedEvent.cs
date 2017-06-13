namespace JYW.ThesisMMO.MMOServer.Events.ActionEvents {

    using Photon.SocketServer.Rpc;

    using JYW.ThesisMMO.Common.Codes;

    class FloatAttributeChangedEvent : AttributeChangedEvent {
        [DataMember(Code = (byte)ParameterCode.FloatValue)]
        public float FloatValue;
    }
}
