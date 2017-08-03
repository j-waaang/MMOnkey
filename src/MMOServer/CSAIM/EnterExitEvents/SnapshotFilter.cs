using System.Collections.Generic;

namespace JYW.ThesisMMO.MMOServer.CSAIM.EnterExitEvents {

    using JYW.ThesisMMO.MMOServer.Events;

    class SnapshotFilter : EnterExitFilter {

        private readonly Dictionary<string, NewEntityEvent> m_SavedSnapshots = new Dictionary<string, NewEntityEvent>();

        public SnapshotFilter(Entity entity) : base(entity) {
        }

        public override void OnEntityEnter(Entity entity) {
            if (entity == m_AttachedEntity) { return; }

            var snapshot = entity.GetEntitySnapshot();
            if (m_SavedSnapshots.ContainsKey(entity.Name) && m_SavedSnapshots[entity.Name].SnapshotEquals(snapshot)) {
                return;
            }
            m_AttachedEntity.SendEvent(snapshot.ToEventData());
            m_SavedSnapshots[entity.Name] = snapshot;
        }

        public override void OnEntityExit(Entity entity) {
            //Yes this should be empty
        }
    }
}
