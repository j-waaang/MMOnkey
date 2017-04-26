namespace JYW.ThesisMMO.MMOServer {
    using Common.Entities;
    using System.Collections.Generic;
    class EntityCache {
        private Dictionary<string, EntityBase> m_Entities;

        internal EntityCache() {
            m_Entities = new Dictionary<string, EntityBase>();
        }
        internal void AddEntity(EntityBase entity) {
            m_Entities[entity.Identifier] = entity;
        }
        internal void RemoveEntity(string id) {
            m_Entities.Remove(id);
        }
    }
}
