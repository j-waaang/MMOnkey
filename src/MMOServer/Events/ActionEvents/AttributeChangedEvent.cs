namespace JYW.ThesisMMO.MMOServer.Events.ActionEvents {

    using Common.Codes;
    using Photon.SocketServer.Rpc;

    class AttributeChangedEvent {
        [DataMember(Code = (byte)ParameterCode.AttributeCode)]
        public int AttributeCode;

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Entity;
    }
}
