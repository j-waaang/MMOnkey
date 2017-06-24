namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities.Attributes.Modifiers;

    class DashRequest : ActionObject {

        #region DataContract
        public DashRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        #endregion DataContract

        public override bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(ActionSource, ActionCode.Dash);
        }

        public override void StartAction() {
            SetState();
        }

        private void SetState() {
            var speedModifier = new FloatModifier(ModifyMode.Multiplication, AttributeCode.Speed, 1.5f);
            World.Instance.ApplyModifier(ActionSource, speedModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 1)));

            ContinueEvent += ResetSpeed;
            ActivateConditions();
        }

        private void ResetSpeed(CallReason continueReason) {

            var speedModifier = new FloatModifier(ModifyMode.Divide, AttributeCode.Speed, 1.5f);
            World.Instance.ApplyModifier(ActionSource, speedModifier);
            ContinueEvent -= ResetSpeed;
        }
    }
}
