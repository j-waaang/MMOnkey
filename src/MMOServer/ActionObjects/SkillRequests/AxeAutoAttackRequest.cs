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

        private const float ATTACKANGLE = 150f;
        public const float ATTACKDISTANCE = 3.0f;

        public override void StartAction() {
            // In case the client did not normalize
            LookDirection = LookDirection.Normalized;
            FinishedCastingEvent += DoDamage;
            StartCast(new System.TimeSpan(0, 0, 0, 0, 500), LookDirection);
        }

        private void DoDamage(CallReason continueReason) {
            if (continueReason == CallReason.Interupted) {
                SetIdle(continueReason);
                return;
            }

            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, -5);
            var sourcePos = World.Instance.GetEntity(ActionSource).Position;

            var dmgArea = new Cone2DAreaTarget(sourcePos, LookDirection, ATTACKANGLE, ATTACKDISTANCE) {
                AreaTargetOption = AreaTargetOption.IgnoreSource,
                SourceName = ActionSource
            };

            World.Instance.ApplyModifier(dmgArea, healthModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 0, 500)));

            ContinueEvent -= DoDamage;
            ContinueEvent += SetIdle;
            StartConditions();
        }
    }
}
