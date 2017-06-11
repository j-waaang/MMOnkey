namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;

    internal class IntAttribute : Attribute {

        protected int m_Value;

        internal int GetValue() {
            return m_Value;
        }

        internal virtual void SetValue(ModifyMode mode, int value) {
            var oldVal = m_Value;

            switch (mode) {
                case ModifyMode.Addition:
                    m_Value += value;
                    break;
                case ModifyMode.Multiplication:
                    m_Value *= value;
                    break;
            }

            log.DebugFormat("{0}'s {1} changed from {2} to {3}.", m_Entity.Name, AttributeCode, oldVal, m_Value);
        }

        internal IntAttribute(int value, AttributeCode attributeCode)
            : base(attributeCode) {
            m_Value = value;
        }
    }
}
