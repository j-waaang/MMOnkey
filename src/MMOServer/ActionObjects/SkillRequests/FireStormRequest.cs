namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities;
    using Entities.Attributes.Modifiers;
    using Common.Types;

    class FireStormRequest : ActionObject {

        #region DataContract
        public FireStormRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public Vector Target { get; set; }
        #endregion DataContract

        internal override bool CheckPrerequesite() {
            var target = new Target { TargetType = TargetType.Position, TargetPosition = Target };
            return World.Instance.CanPerformAction(m_ActionSource, ActionCode.AxeAutoAttack, target);
        }

        internal override void StartAction() {
            SetState();
        }

        private void SetState() {
            var stateModifier = new ActionStateModifier(ActionCode.FireStorm);
            World.Instance.ApplyModifier(m_ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 2)));

            ContinueEvent += CreateFireStorm;
            ActivateConditions();
        }

        private void CreateFireStorm(ContinueReason continueReason) {

            ContinueEvent -= CreateFireStorm;

        }
    }
}
