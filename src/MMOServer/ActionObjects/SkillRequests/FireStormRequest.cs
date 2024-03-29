﻿using Photon.SocketServer;
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

        public override void StartAction() {
            FinishedCastingEvent += CreateFireStormEntity;
            FinishedCastingEvent += SetIdle;
            FinishedCastingEvent += SetActionCooldown;
            StartCast(new System.TimeSpan(0, 0, 0, 2), GetLookDir(ActionSource, Target));
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
