namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;

    internal class ActionStateModifier : Modifier {

        private ActionState m_ActionState;

        internal ActionStateModifier(ActionState value) {
            m_ActionState = value;
            m_Attribute = AttributeCode.ActionState;
        }

        internal override void ApplyOnEntity(Entity entity) {
            var actionState = entity.GetAttribute(m_Attribute) as ActionStateAttribute;
            actionState.ActionState = m_ActionState;
        }
    }
}
