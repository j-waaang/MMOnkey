namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using Common.Types;
    using JYW.ThesisMMO.Common.Codes;

    internal class CastActionStateModifier : ActionStateModifier {

        private Vector m_LookDir;

        public CastActionStateModifier(ActionCode value, Vector lookDir)
            : base(value){
            m_LookDir = lookDir;
            m_Attribute = AttributeCode.ActionState;
        }

        public override void ApplyEffect(Entity entity) {
            var actionState = entity.GetAttribute(m_Attribute) as ActionStateAttribute;
            actionState.SetActionState(ActionState, m_LookDir);
        }
    }
}
