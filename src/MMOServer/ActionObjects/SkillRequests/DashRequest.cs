namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Common.Codes;
    using Entities.Attributes.Modifiers;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    class DashRequest : ActionObject {

        #region DataContract
        public DashRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        #endregion DataContract

        internal override bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(m_ActionSource, ActionCode.Dash);
        }

        internal override void StartAction() {
            SetState();
        }

        private void SetState() {
            var speedModifier = new FloatModifier(ModifyMode.Multiplication, AttributeCode.Speed, 1.5f);
            World.Instance.ApplyModifier(m_ActionSource, speedModifier);
            AddCondition(new TimedContinueCondition(this, new System.TimeSpan(0, 0, 0, 1)));

            ContinueEvent += ResetSpeed;
            ActivateConditions();
        }

        private void ResetSpeed(ContinueReason continueReason) {

            var speedModifier = new FloatModifier(ModifyMode.Divide, AttributeCode.Speed, 1.5f);
            World.Instance.ApplyModifier(m_ActionSource, speedModifier);
            ContinueEvent -= ResetSpeed;
        }
    }
}
