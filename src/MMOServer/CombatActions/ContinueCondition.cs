namespace JYW.ThesisMMO.MMOServer.CombatActions {

    using System;

    internal abstract class ContinueCondition {
        internal event Action<ContinueReason> ConditionFullfilledEvent;
    }
}
