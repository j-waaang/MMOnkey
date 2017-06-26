using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Common.Codes;
    using Common.ContinueObjects;
    using Common.Types;

    internal class FireStormRequest : CastActionObject {

        private static int m_LastCreatedID = 0;

        #region DataContract
        public FireStormRequest(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(actionSource, protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Target { get; set; }
        #endregion DataContract

        public override bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(ActionSource, ActionCode.FireStorm);
        }

        public override void StartAction() {
            StartCast(
                new System.TimeSpan(0, 0, 0, 2),
                ActionCode.FireStorm,
                GetLookDir(ActionSource, Target));

            FinishedCastingEvent += CreateFireStormEntity;
            FinishedCastingEvent += SetIdle;
        }

        private void CreateFireStormEntity(CallReason continueReason) {
            if(continueReason == CallReason.Interupted) { return; }

            ContinueEvent -= CreateFireStormEntity;
            m_LastCreatedID++;
            SetIdle(continueReason);
            EntityFactory.Instance.CreateSkillEntity(ActionSource, m_LastCreatedID.ToString(), ActionCode.FireStorm, Target);
        }
    }
}
