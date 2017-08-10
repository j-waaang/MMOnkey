namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using Entities.Attributes.Modifiers;
    using Common.Codes;
    using Common.ContinueObjects;
    using Common.Types;
    using Targets;

    internal class DistractingShotRequest : CastActionObject {

        #region DataContract
        public DistractingShotRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.LookDirection)]
        public Vector LookDirection { get; set; }
        #endregion DataContract

        private const float ATTACKWIDTH = 3;
        private const float ATTACKDISTANCE = 8;

        public override void StartAction() {
            LookDirection = LookDirection.Normalized;
            FinishedCastingEvent += DoDamage;
            FinishedCastingEvent += SetIdle;
            FinishedCastingEvent += SetActionCooldown;
            StartCast(new System.TimeSpan(0, 0, 0, 0, 500), LookDirection);
        }

        private void DoDamage(CallReason continueReason) {
            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, -5);
            var sourcePos = World.Instance.GetEntity(ActionSource).Position;

            log.InfoFormat("DS LookDir is {0}", LookDirection);

            var LookDirP = new Vector(LookDirection.Z, -LookDirection.X);
            var P1 = sourcePos + LookDirP * 0.5f * ATTACKWIDTH;
            var P2 = sourcePos - LookDirP * 0.5f * ATTACKWIDTH;
            var P3 = P2 + LookDirection * ATTACKDISTANCE;

            var dmgArea = new RectangleAreaTarget(P1, P2, P3) {
                AreaTargetOption = AreaTargetOption.IgnoreSource,
                SourceName = ActionSource
            };

            World.Instance.ApplyModifier(dmgArea, healthModifier);

            var interupted = World.Instance.GetEntitesInArea(dmgArea);
            m_InteruptionHandler.OnInterupt(interupted);
        }
    }
}
