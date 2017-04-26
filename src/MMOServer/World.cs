namespace JYW.ThesisMMO.MMOServer {
    using Common.Entities;
    using System;

    internal class World : IDisposable{

        public static World Instance { get; private set; }

        public EntityCache EntityCache { get; private set; }

        internal World() {
            if(Instance == null) {
                Instance = this;
            }
            else { return; }

            EntityCache = new EntityCache();
        }

        internal void AddEntity(EntityBase entity) {
        }

        public void Dispose() {
            Instance = null;
        }
    }
}
