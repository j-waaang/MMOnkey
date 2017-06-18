using ExitGames.Logging;
using System;

namespace JYW.ThesisMMO.MMOServer.AI {

    /// <summary> 
    /// Base class for AIs to deerive from.
    /// </summary>
     abstract internal class AIEntity : IDisposable {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        protected Entity m_Entity;


        public AIEntity(Entity entity) {
            m_Entity = entity;
            AILooper.Instance.AddEntity(this);
        }

        public void Dispose() {
            AILooper.Instance.RemoveEntity(this);
        }

        /// <summary> 
        /// Called by AIModule's loop.
        /// </summary>
        public abstract void Update();
    }
}
