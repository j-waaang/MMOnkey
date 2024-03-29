﻿namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {
    using Common.Types;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Entities;

    internal class ActionStateModifier : Modifier {

        protected ActionCode ActionState { get; private set; }

        public ActionStateModifier(ActionCode value) {
            ActionState = value;
            m_Attribute = AttributeCode.ActionState;
        }
        
        public override void ApplyEffect(Entity entity) {
            var actionState = entity.GetAttribute(m_Attribute) as ActionStateAttribute;
            actionState.SetActionState(ActionState);
        }
    }
}
