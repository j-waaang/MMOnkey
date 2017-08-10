using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities.Attributes.Modifiers;
    using Targets;

    internal class OrisonOfHealingRequest : CastActionObject {

        private const float MaxDistance = 8F;

        #region DataContract
        public OrisonOfHealingRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Target { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(ActionSource, (ActionCode)Code, Target, MaxDistance);
        }

        public override void StartAction() {
            FinishedCastingEvent += DoHealing;
            FinishedCastingEvent += SetIdle;
            FinishedCastingEvent += SetActionCooldown;
            StartCast(new System.TimeSpan(0, 0, 0, 1), GetLookDir(ActionSource, Target));
        }

        private void DoHealing(CallReason continueReason) {
            if(continueReason == CallReason.Interupted) { return; }

            var healthModifier = new IntModifier(ModifyMode.Addition, AttributeCode.Health, 30);
            World.Instance.ApplyModifier(Target, healthModifier);
        }
    }
}
