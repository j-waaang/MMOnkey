namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using System;

    /// <summary> 
    /// A condition required for an action to continue.
    /// </summary>
    internal abstract class ActionContinueCondition {
        internal event Action<ContinueReason> ContinueEvent;

        protected void RaiseContinueEvent(ContinueReason reason) {
            ContinueEvent(reason);
        }
    }
}
