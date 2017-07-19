using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using JYW.ThesisMMO.Common.Codes;
    using Common.Types;

    internal class ActionStateAttribute : Attribute {

        private ActionCode m_ActionState = ActionCode.Idle;

        public ActionCode GetActionState() {
            return m_ActionState;
        }

        public void SetActionState(ActionCode newState) {
            m_ActionState = newState;

            log.DebugFormat("{0}'s actionState changed to {1}", m_Entity.Name, m_ActionState.ToString());

            var ev = new ActionStateChangedEvent() {
                Username = m_Entity.Name,
                ActionState = (int)m_ActionState
            };
            IEventData evData = new EventData((byte)EventCode.ActionStateUpdate, ev);
            PublishChange(evData, BroadcastOptions.IgnoreOwner);
        }

        public void SetActionState(ActionCode newState, Vector lookDir) {
            m_ActionState = newState;

            log.DebugFormat("{0}'s actionState changed to {1}", m_Entity.Name, m_ActionState.ToString());

            var ev = new ActionStateChangedEvent() {
                Username = m_Entity.Name,
                ActionState = (int)m_ActionState,
                LookDirection = lookDir
            };
            IEventData evData = new EventData((byte)EventCode.ActionStateUpdate, ev);
            PublishChange(evData, BroadcastOptions.IgnoreOwner);
        }

        internal ActionStateAttribute() : base(AttributeCode.ActionState) { }
    }
}
