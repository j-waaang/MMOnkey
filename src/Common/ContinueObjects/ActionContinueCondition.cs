namespace JYW.ThesisMMO.Common.ContinueObjects {

    using System;

    /// <summary> 
    /// A condition required for an action to continue.
    /// </summary>
    public abstract class ActionContinueCondition : IDisposable {
        public event Action<CallReason> ContinueEvent;

        protected void RaiseContinueEvent(CallReason reason) {
            ContinueEvent(reason);
        }

        public abstract void Start();

        public virtual void Dispose() {
            ContinueEvent = null;
        }
    }
}
