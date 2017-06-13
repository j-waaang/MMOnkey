namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Entities;
    using Entities.Attributes.Modifiers;
    using Common.Entities;

    class AxeAutoAttackRequest : ActionObject {

        #region DataContract
        public AxeAutoAttackRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
            log.DebugFormat("Created axe aa");
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }
        #endregion DataContract

        internal override bool CheckPrerequesite() {
            var target = new Target { TargetType = TargetType.Entity, TargetName = Target };
            return World.Instance.CanPerformAction(m_ActionSource, ActionCode.AxeAutoAttack, target);
        }

        internal override void StartAction() {
            log.DebugFormat("Started axe aa");
            SetState();
        }

        private void SetState() {
            var stateModifier = new ActionStateModifier(ActionCode.AxeAutoAttack);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(this, new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent += DoDamage;
            ActivateConditions();
        }

        private void DoDamage(ContinueReason continueReason) {
            log.DebugFormat("Do damage");

            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, -20);
            World.Instance.ApplyModifier(Target, healthModifier);
            AddCondition(new TimedContinueCondition(this, new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent -= DoDamage;
            ContinueEvent += SetIdle;

            ActivateConditions();
        }

        private void SetIdle(ContinueReason continueReason) {
            log.DebugFormat("Set idle");
            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
        }
    }
}
