namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Entities.Attributes.Modifiers;

    using Common.Codes;
    using Common.ContinueObjects;
    using Targets;

    class AxeAutoAttackRequest : ActionObject {

        #region DataContract
        public AxeAutoAttackRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
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
            var stateModifier = new ActionStateModifier(ActionCode.AxeAutoAttack);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent += DoDamage;
            ActivateConditions();
        }

        private void DoDamage(CallReason continueReason) {
            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, -20);
            World.Instance.ApplyModifier(Target, healthModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent -= DoDamage;
            ContinueEvent += SetIdle;

            ActivateConditions();
        }

        private void SetIdle(CallReason continueReason) {
            ContinueEvent -= SetIdle;
            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
        }
    }
}
