namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;
    using Photon.SocketServer;

    internal class FloatAttribute : Attribute {

        protected float m_Value;

        internal float GetValue() {
            return m_Value;
        }

        internal virtual void SetValue(ModifyMode mode, float value) {
            var oldVal = m_Value;

            switch (mode) {
                case ModifyMode.Addition:
                    m_Value += value;
                    break;
                case ModifyMode.Multiplication:
                    m_Value *= value;
                    break;
                case ModifyMode.Divide:
                    m_Value /= value;
                    break;
            }

            log.DebugFormat("{0}'s {1} changed from {2} to {3}.", m_Entity.Name, AttributeCode, oldVal, m_Value);

            var ev = new FloatAttributeChangedEvent() {
                AttributeCode = (int)AttributeCode,
                Entity = m_Entity.Name,
                FloatValue = m_Value
            };
            IEventData evData = new EventData((byte)EventCode.AttributeChangedEvent, ev);

            World.Instance.ReplicateMessage(m_Entity.Name, evData, BroadcastOptions.All);
        }

        internal FloatAttribute(float value, AttributeCode attributeCode)
            : base(attributeCode) {
            m_Value = value;
        }
    }
}
