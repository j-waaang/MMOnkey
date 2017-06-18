using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities.Attributes.Modifiers;
    using Common.Types;
    using Targets;

    internal class FireStormRequest : ActionObject {

        private int m_LastCreatedID = 0;

        #region DataContract
        public FireStormRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public Vector Target { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            var target = new CircleAreaTarget() { Center = Target, Radius = 7f };
            return World.Instance.CanPerformAction(m_ActionSource, ActionCode.AxeAutoAttack, target);
        }

        public override void StartAction() {
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
            m_LastCreatedID++;
            EntityFactory.Instance.CreateSkillEntity(m_LastCreatedID.ToString(), ActionCode.FireStorm, Target);
            ContinueEvent -= CreateFireStorm;
        }
    }
}
