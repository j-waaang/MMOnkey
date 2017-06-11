namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    using System;
    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class BowAutoAttackRequest : ActionObject {

        public BowAutoAttackRequest(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {

            AddCondition(new TimedContinueCondition(this, new System.TimeSpan(0, 0, 0, 0, 500)));
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }

        internal override bool CheckPrerequesite() {
            throw new NotImplementedException();
        }

        internal override void StartAction() {
            throw new NotImplementedException();
        }
    }
}
