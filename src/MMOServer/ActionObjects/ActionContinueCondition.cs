namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using System;

    /// <summary> 
    /// A condition required for an action to continue.
    /// </summary>
    internal abstract class ActionContinueCondition : IDisposable {
        internal event Action<ContinueReason> ContinueEvent;

        protected ActionObject m_ActionObject;

        protected ActionContinueCondition(ActionObject actionObject) {
            m_ActionObject = actionObject;
        }

        protected void RaiseContinueEvent(ContinueReason reason) {
            ContinueEvent(reason);
        }

        internal abstract void Start();

        public void Dispose() {
            ContinueEvent = null;
        }
    }
}
