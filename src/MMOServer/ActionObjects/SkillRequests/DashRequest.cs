using System;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities.Attributes.Modifiers;

    class DashRequest : ActionObject {

        #region DataContract
        public DashRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        #endregion DataContract

        private readonly TimeSpan m_SkillDuration = new TimeSpan(0, 0, 0, 2);

        public override void StartAction() {
            SetState();
        }

        private void SetState() {
            var speedModifier = new FloatModifier(ModifyMode.Multiplication, AttributeCode.Speed, 1.5f);
            World.Instance.ApplyModifier(ActionSource, speedModifier);

            SetActionCooldown();

            AddCondition(new TimedContinueCondition(m_SkillDuration));

            ContinueEvent += ResetSpeed;
            StartConditions();
        }

        private void ResetSpeed(CallReason continueReason) {
            var speedModifier = new FloatModifier(ModifyMode.Divide, AttributeCode.Speed, 1.5f);
            World.Instance.ApplyModifier(ActionSource, speedModifier);
            ContinueEvent -= ResetSpeed;
        }
    }
}
