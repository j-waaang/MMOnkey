namespace JYW.ThesisMMO.MMOServer.CombatActions {

    using JYW.ThesisMMO.MMOServer.Requests;

    /// <summary> 
    /// An ongoing action bound by conditions.
    /// </summary>
    internal class ActionObject {
        private ContinueCondition m_ContinueCondition;
        private CharacterActionRequest m_ActionData;

        internal ActionObject(CharacterActionRequest request, ContinueCondition continueCondition) {
            m_ActionData = request;
            m_ContinueCondition = continueCondition;
            m_ContinueCondition.ConditionFullfilledEvent += ContinueAction;
        }

        private void ContinueAction(ContinueReason continueReason) {

        }
    }
}
