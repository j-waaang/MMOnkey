using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.MMOServer.Events;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.CSAIM.EnterExitEvents {
    class DummyEnterExitFilter : EnterExitFilter {
        public DummyEnterExitFilter(Entity entity) : base(entity) {
        }

        public override void OnEntityEnter(Entity entity) {
            if (entity == m_AttachedEntity) { return; }
            var eventData = entity.GetEntitySnapshot();
            m_AttachedEntity.SendEvent(eventData.ToEventData());
        }

        public override void OnEntityExit(Entity entity) {
            if (entity == m_AttachedEntity) { return; }
            var ev = new EntityEvent() {
                Username = entity.Name,
            };
            IEventData eventData = new EventData((byte)EventCode.EntityExitRegion, ev);
            m_AttachedEntity.SendEvent(eventData);
        }
    }
}
