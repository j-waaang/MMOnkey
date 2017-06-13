namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using Photon.SocketServer;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;

    internal class HealthAttribute : IntAttribute {

        internal override void SetValue(ModifyMode mode, int value) {

            var maxHealthAttribute = m_Entity.GetAttribute(AttributeCode.MaxHealth) as IntAttribute;
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

            var amtChanged = m_Value - beforeHealth;
            if (m_Value <= 0) {
                m_Value = 0;
                m_Entity.Die();
                return;
            }

            log.DebugFormat("{0} took {1} damage. Health left: {2}", m_Entity.Name, amtChanged.ToString(), m_Value);

            var ev = new HealthChangedEvent() {
                Username = m_Entity.Name,
                Damage = amtChanged,
                CurrenHealth = m_Value
            };
            IEventData evData = new EventData((byte)EventCode.HealthUpdate, ev);

            World.Instance.ReplicateMessage(m_Entity.Name, evData, BroadcastOptions.All);
        }

        internal HealthAttribute(int value)
            : base(value, AttributeCode.Health) { }
    }
}
