namespace JYW.ThesisMMO.MMOServer {
    using System;
    internal class World : IDisposable{
        public static World Instance { get; private set; }
        public readonly EntityCache entityCache;
        internal World() {
            if(Instance == null) {
                Instance = this;
            }
            else { return; }

            entityCache = new EntityCache();
        }
        public void Dispose() {
            Instance = null;
        }
    }
}
