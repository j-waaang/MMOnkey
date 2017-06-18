namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities;
    using Entities.Attributes.Modifiers;
    using Targets;

    class OrisonOfHealingRequest : ActionObject {

        #region DataContract
        public OrisonOfHealingRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            var target = new EntityTarget() { TargetName = Target };
            return World.Instance.CanPerformAction(m_ActionSource, ActionCode.AxeAutoAttack, target);
        }

        public override void StartAction() {
            SetState();
        }

        private void SetState() {
            var stateModifier = new ActionStateModifier(ActionCode.OrisonOfHealing);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 1)));

            ContinueEvent += DoHealing;
            ActivateConditions();
        }

        private void DoHealing(ContinueReason continueReason) {
            ContinueEvent -= DoHealing;

            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, 30);
            World.Instance.ApplyModifier(Target, healthModifier);
            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
        }
    }
}
