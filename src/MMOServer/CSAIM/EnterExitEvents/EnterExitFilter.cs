using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.MMOServer.Events;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.CSAIM.EnterExitEvents {
    internal abstract class EnterExitFilter {

        protected readonly Entity m_AttachedEntity;

        public EnterExitFilter(Entity entity) {
            m_AttachedEntity = entity;
        }

        public abstract void OnEntityEnter(Entity entity);
        public abstract void OnEntityExit(Entity entity);
        
    }
}
