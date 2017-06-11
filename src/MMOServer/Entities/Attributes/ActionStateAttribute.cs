namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;

    internal class ActionStateAttribute : Attribute {

        private ActionState m_ActionState = ActionState.Idle;

        public ActionState ActionState {
            get {
                return m_ActionState;
            }
            set {
                m_ActionState = value;

                // TODO: Notify entity about change.
                log.DebugFormat("{0}'s actionState changed to {1}", m_Entity.Name, m_ActionState.ToString());
            }
        }

        internal ActionStateAttribute() : base(AttributeCode.ActionState) { }
    }
}
