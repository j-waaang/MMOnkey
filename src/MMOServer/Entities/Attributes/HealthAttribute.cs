namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;

    internal class HealthAttribute : IntAttribute {

        internal override void SetValue(ModifyMode mode, int value) {

            var maxHealthAttribute = m_Entity.GetAttribute(Common.Codes.AttributeCode.MaxHealth) as IntAttribute;
            var maxHealth = maxHealthAttribute.GetValue();
            var beforeHealth = m_Value;

            switch (mode) {
                case ModifyMode.Addition:
                    if (m_Value + value > maxHealth) {
                        m_Value = maxHealth;
                    }
                    else {
                        m_Value += value;
                    }
                    break;
                case ModifyMode.Multiplication:
                    if (m_Value * value > maxHealth) {
                        m_Value = maxHealth;
                    }
                    else {
                        m_Value *= value;
                    }
                    break;
            }

            var amtChanged = beforeHealth - m_Value;
            if (m_Value <= 0) {
                m_Value = 0;
                m_Entity.Die();
                return;
            }

            // TODO: Notify about change
            log.DebugFormat("{0} took {1} damage. Health left: {2}", m_Entity.Name, amtChanged.ToString(), m_Value);
        }

        internal HealthAttribute(int value)
            : base(value, Common.Codes.AttributeCode.Health) { }
    }
}
