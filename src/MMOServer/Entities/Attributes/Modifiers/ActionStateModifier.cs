namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;

    internal class ActionStateModifier : Modifier {

        private ActionCode m_ActionState;

        public ActionStateModifier(ActionCode value) {
            m_ActionState = value;
            m_Attribute = AttributeCode.ActionState;
        }

        public override void ApplyEffect(Entity entity) {
            var actionState = entity.GetAttribute(m_Attribute) as ActionStateAttribute;
            actionState.ActionState = m_ActionState;
        }
    }
}
