using ExitGames.Logging;
using System;

namespace JYW.ThesisMMO.MMOServer.AI {

    /// <summary> 
    /// Base class for AIs to deerive from.
    /// </summary>
     abstract internal class AIEntity {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        public Entity Entity { get; private set; }

        public AIEntity(Entity entity) {
            //log.InfoFormat("Created ai {0} from {1}.", GetType().Name, entity.Name);
            Entity = entity;
            AILooper.Instance.AddEntity(this);
        }

        /// <summary> 
        /// Called by AIModule's loop.
        /// </summary>
        public abstract void Update(TimeSpan deltaTime);
    }
}
