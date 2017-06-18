using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using JYW.ThesisMMO.Common.Codes;

    internal class ActionStateAttribute : Attribute {

        private ActionCode m_ActionState = ActionCode.Idle;

        public ActionCode ActionState {
            get {
                return m_ActionState;
            }
            set {
                m_ActionState = value;

                log.DebugFormat("{0}'s actionState changed to {1}", m_Entity.Name, m_ActionState.ToString());

                var ev = new ActionStateChangedEvent() {
                    Username = m_Entity.Name,
                    ActionState = (int)m_ActionState
                };
                IEventData evData = new EventData((byte)EventCode.ActionStateUpdate, ev);

                World.Instance.ReplicateMessage(m_Entity.Name, evData, BroadcastOptions.AllExceptMsgOwner);
            }
        }

        internal ActionStateAttribute() : base(AttributeCode.ActionState) { }
    }
}
