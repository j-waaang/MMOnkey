using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.AI {

    /// <summary> 
    /// Controlls the AI loop and holds AI Entities.
    /// </summary>
    internal class AILooper : IDisposable {

        public static AILooper Instance = new AILooper();

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly List<AIEntity> m_AIEntites = new List<AIEntity>();
        private readonly TimeSpan m_TickTimeout = new TimeSpan(0, 0, 0, 0, 33);
        private readonly Thread m_AIThread;

        private bool m_Running;

        private AILooper() {
            m_Running = true;
            m_AIThread = new Thread(AILoop);
        }

        public void Start() {
            m_AIThread.Start();
        }

        /// <summary> 
        /// Loop.
        /// </summary>
        private void AILoop() {
            while (m_Running) {
                //copy in case list gets modified. this might take long if we have many ai entities.
                var entities = m_AIEntites.ToArray();
                foreach (AIEntity entity in entities) {
                    if (entity == null) { continue; }
                    entity.Update(m_TickTimeout);
                }
                Thread.Sleep(m_TickTimeout);
            }
        }

        /// <summary> 
        /// Adding a entity to the list and calls it in the loop.
        /// </summary>
        internal void AddEntity(AIEntity aiEntity) {
            m_AIEntites.Add(aiEntity);
        }

        internal void RemoveEntity(AIEntity aiEntity) {
            m_AIEntites.Remove(aiEntity);
        }

        internal void RemoveEntity(Entity entity) {
            AIEntity aiEntity = m_AIEntites.Single(a => a.Entity == entity);
            m_AIEntites.Remove(aiEntity);
        }

        public void Dispose() {
            m_Running = false;
        }
    }
}
