namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities.Attributes.Modifiers;
    using Targets;

    class OrisonOfHealingRequest : CastActionObject {

        #region DataContract
        public OrisonOfHealingRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            var target = new EntityTarget() { TargetName = Target };
            return World.Instance.CanPerformAction(ActionSource, (ActionCode)Code, target);
        }

        public override void StartAction() {
            FinishedCastingEvent += DoHealing;
            FinishedCastingEvent += SetIdle;
            StartCast(new System.TimeSpan(0, 0, 0, 1), GetLookDir(ActionSource, Target));
        }

        private void DoHealing(CallReason continueReason) {
            if(continueReason == CallReason.Interupted) { return; }

            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, 30);
            World.Instance.ApplyModifier(Target, healthModifier);
        }
    }
}
