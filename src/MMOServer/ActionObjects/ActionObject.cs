﻿namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using System;
    using System.Collections.Generic;

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using ExitGames.Logging;

    using JYW.ThesisMMO.Common.Codes;

    /// <summary> 
    /// A game action requested by the client to change the game state.
    /// </summary>
    internal abstract class ActionObject : Operation{

        #region DataContract
        internal ActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
            m_ActionSource = actionSource;
        }

        public ActionObject(IRpcProtocol protocol, OperationRequest request) {
            this.protocol = protocol;
            this.request = request;
        }

        [DataMember(Code = (byte)ParameterCode.ActionCode)]
        internal ActionCode actionCode { get; set; }
        #endregion DataContract

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected event Action<ContinueReason> ContinueEvent;
        protected string m_ActionSource;

        private List<ActionContinueCondition> m_ContinueConidtions = new List<ActionContinueCondition>();
        private IRpcProtocol protocol;
        private OperationRequest request;

        abstract internal bool CheckPrerequesite();

        abstract internal void StartAction();

        protected void AddCondition(ActionContinueCondition condition) {
            m_ContinueConidtions.Add(condition);
            condition.ContinueEvent += OnConditionFullfilled;
        }

        protected void ActivateConditions() {
            foreach (ActionContinueCondition condition in m_ContinueConidtions) {
                condition.Start();
            }
        }

        private void OnConditionFullfilled(ContinueReason continueReason) {
            foreach(ActionContinueCondition condition in m_ContinueConidtions) {
                condition.Dispose();
            }
            m_ContinueConidtions.Clear();
            
            ContinueEvent(continueReason);
        }
    }
}
