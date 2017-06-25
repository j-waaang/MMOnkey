﻿using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests {

    using Common.Codes;
    using Common.ContinueObjects;
    using Entities.Attributes.Modifiers;
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
            SetState();
        }

        private void SetState() {
            var lookDir = GetLookDir(ActionSource, Target);
            var stateModifier = new CastActionStateModifier(ActionCode.FireStorm, lookDir);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(new System.TimeSpan(0, 0, 0, 2)));

            ContinueEvent += CreateFireStormAndSetIdle;
            StartConditions();
        }

        private void CreateFireStormAndSetIdle(CallReason continueReason) {
            log.InfoFormat("FireStoremReq with pos {0}", Target);
            m_LastCreatedID++;
            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
            EntityFactory.Instance.CreateSkillEntity(ActionSource, m_LastCreatedID.ToString(), ActionCode.FireStorm, Target);
            ContinueEvent -= CreateFireStormAndSetIdle;
        }
    }
}
