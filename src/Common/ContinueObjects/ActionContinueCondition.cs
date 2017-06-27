namespace JYW.ThesisMMO.Common.ContinueObjects {

    using System;

    /// <summary> 
    /// A condition required for an action to continue.
    /// </summary>
    public abstract class ActionContinueCondition : IDisposable {
        public event Action<CallReason> ContinueEvent;
        protected bool m_Disposed = false;

        protected void RaiseContinueEvent(CallReason reason) {
            if (m_Disposed) { return; }
            ContinueEvent(reason);
        }

        public abstract void Start();

        public virtual void Dispose() {
            m_Disposed = true;
        }
    }
}
