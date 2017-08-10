namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;

    using Entities.Attributes.Modifiers;
    using Common.Codes;
    using Common.ContinueObjects;
    using Common.Types;
    using Targets;

    internal class HammerBashRequest : CastActionObject {

        public HammerBashRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        private const float RADIUS = 2.75f;

        public override void StartAction() {
            FinishedCastingEvent += DoDamage;
            FinishedCastingEvent += SetIdle;
            FinishedCastingEvent += SetActionCooldown;
            StartCast(new System.TimeSpan(0, 0, 0, 1), new Vector(0, -1f));
        }

        private void DoDamage(CallReason continueReason) {
            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, -7);
            var sourcePos = World.Instance.GetEntity(ActionSource).Position;

            var dmgArea = new CircleAreaTarget(sourcePos, RADIUS) {
                AreaTargetOption = AreaTargetOption.IgnoreSource,
                SourceName = ActionSource
            };

            World.Instance.ApplyModifier(dmgArea, healthModifier);

            var interupted = World.Instance.GetEntitesInArea(dmgArea);
            m_InteruptionHandler.OnInterupt(interupted);
        }
    }
}
