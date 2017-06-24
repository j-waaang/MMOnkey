namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Entities.Attributes.Modifiers;

    using Common.Codes;
    using Common.ContinueObjects;
    using Targets;
    using Common.Types;

    internal class AxeAutoAttackRequest : CastActionObject {

        #region DataContract
        public AxeAutoAttackRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.LookDirection)]
        public Vector LookDirection { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(ActionSource, ActionCode.AxeAutoAttack);
        }

        public override void StartAction() {
            // In case the client did not normalize
            LookDirection = LookDirection.Normalized;
            SetState();
        }

        private void SetState() {
            var stateModifier = new CastActionStateModifier(ActionCode.AxeAutoAttack, LookDirection);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent += DoDamage;
            ActivateConditions();
        }

        private void DoDamage(CallReason continueReason) {
            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, -20);
            var sourcePos = World.Instance.GetEntity(ActionSource).Position;
            var LookDirP = new Vector(LookDirection.Z, -LookDirection.X);

            var dmgArea = new RectangleAreaTarget() {
                AreaTargetOption = AreaTargetOption.IgnoreSource,
                A = sourcePos + LookDirP * 0.7f,
                B = sourcePos - LookDirP * 0.7f,
                C = sourcePos - LookDirP * 0.7f + LookDirection * 2,
                SourceName = ActionSource
            };

            World.Instance.ApplyModifier(dmgArea, healthModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent -= DoDamage;
            ContinueEvent += SetIdle;

            ActivateConditions();
        }

        private void SetIdle(CallReason continueReason) {
            ContinueEvent -= SetIdle;
            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
        }
    }
}
