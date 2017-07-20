using System.Diagnostics;
using System.Collections.Generic;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Types;

    internal class PositionFilter {

        protected readonly Entity m_AttachedEntity;

        private static Stopwatch Time = new Stopwatch();
        private readonly Dictionary<Entity, long> PositionTimestamps = new Dictionary<Entity, long>();

        static PositionFilter() {
            Time.Start();
        }

        public PositionFilter(Entity entity) {
            m_AttachedEntity = entity;
        }

        public void UpdateFilter() {

        }

        public bool FilterPosition(Vector position) {
            throw new System.NotImplementedException();
        }

        protected long TimeSinceLastUpdate(Entity entity) {
            if (!PositionTimestamps.ContainsKey(entity)) { return -1L; }
            return Time.ElapsedMilliseconds - PositionTimestamps[entity];
        }

        protected void UpdateTimestamp(Entity entity) {
            PositionTimestamps[entity] = Time.ElapsedMilliseconds;
        }
    }
}
