namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using JYW.ThesisMMO.Common.Codes;

    internal class FloatModifier : Modifier {

        private ModifyMode m_Mode;
        private float m_Value;

        internal FloatModifier(ModifyMode mode, AttributeCode attribute, float value) {
            m_Attribute = attribute;
            m_Mode = mode;
            m_Value = value;
        }

        internal override void ApplyOnEntity(Entity entity) {
            var attribute = entity.GetAttribute(m_Attribute) as FloatAttribute;
            attribute.SetValue(m_Mode, m_Value);
        }
    }
}
