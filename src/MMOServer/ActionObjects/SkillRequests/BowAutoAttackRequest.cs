namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using Entities.Attributes.Modifiers;
    using Common.Codes;
    using Common.ContinueObjects;
    using Targets;
    using Common.Types;

    internal class BowAutoAttackRequest : CastActionObject {

        #region DataContract
        public BowAutoAttackRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.LookDirection)]
        public Vector LookDirection { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(ActionSource, ActionCode.BowAutoAttack);
        }

        public override void StartAction() {
            SetState();
        }

        private void SetState() {
            var stateModifier = new CastActionStateModifier(ActionCode.BowAutoAttack, LookDirection);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent += CreateArrow;
            ActivateConditions();
        }

        private void CreateArrow(CallReason continueReason) {
            ContinueEvent -= CreateArrow;

            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(ActionSource, stateModifier);

            var startPos = World.Instance.GetEntity(ActionSource).Position;
            EntityFactory.Instance.CreateSkillEntity(this, startPos);
        }
    }
}
