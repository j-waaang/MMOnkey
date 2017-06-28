namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using System;
    using System.Collections.Generic;

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using ExitGames.Logging;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.ContinueObjects;
    using Entities.Attributes.Modifiers;

    /// <summary> 
    /// A game action requested by the client to change the game state.
    /// </summary>
    internal abstract class ActionObject : Operation {

        #region DataContract
        public ActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
            ActionSource = actionSource;
            log.InfoFormat("Created {0} ActionObject", (ActionCode)Code);
        }

        public ActionObject(IRpcProtocol protocol, OperationRequest request) {
            this.protocol = protocol;
            this.request = request;
        }

        [DataMember(Code = (byte)ParameterCode.ActionCode)]
        public int Code { get; set; }
        #endregion DataContract

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private static int LastUsedID = 0;

        protected event Action<CallReason> ContinueEvent;
        public string ActionSource { get; protected set; }

        private List<ActionContinueCondition> m_ContinueConidtions = new List<ActionContinueCondition>();
        private IRpcProtocol protocol;
        private OperationRequest request;

        public virtual bool CheckPrerequesite() {
            return World.Instance.CanPerformAction(ActionSource);
        }

        public abstract void StartAction();

        protected void AddCondition(ActionContinueCondition condition) {
            m_ContinueConidtions.Add(condition);
            condition.ContinueEvent += OnConditionFullfilled;
        }

        protected void StartConditions() {
            foreach (ActionContinueCondition condition in m_ContinueConidtions) {
                condition.Start();
            }
        }

        private void OnConditionFullfilled(CallReason continueReason) {
            if(continueReason == CallReason.Interupted) { log.InfoFormat("{0} was interupted.", ActionSource); }
            foreach (ActionContinueCondition condition in m_ContinueConidtions) {
                condition.Dispose();
            }
            m_ContinueConidtions.Clear();

            ContinueEvent(continueReason);
        }

        public int GetNextID() {
            LastUsedID++;
            return LastUsedID;
        }

        protected void SetIdle(CallReason callreason) {
            ContinueEvent -= SetIdle;
            var stateModifier = new ActionStateModifier(ActionCode.Idle);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
        }
    }
}
