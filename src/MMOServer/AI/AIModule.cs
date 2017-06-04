namespace JYW.ThesisMMO.MMOServer.AI {

    using System.Collections.Generic;
    using System.Threading;

    /// <summary> 
    /// Controlls the AI loop and holds AI Entities.
    /// </summary>
    class AIModule {

        private List<AIEntity> m_AIEntites = new List<AIEntity>();
        private Thread m_AIThread;
        private bool m_Running;
        private const int m_TickrateMilliSec = 300;

        /// <summary> 
        /// Starting the loop.
        /// </summary>
        internal void Start() {
            m_Running = true;
            m_AIThread = new Thread(AILoop);
            m_AIThread.Start();
        }

        /// <summary> 
        /// Stopping the loop.
        /// </summary>
        internal void Stop() {
            m_Running = false;
        }

        /// <summary> 
        /// Loop.
        /// </summary>
        private void AILoop() {
            while (m_Running) {
                foreach(AIEntity entity in m_AIEntites) {
                    entity.Update();
                }
                Thread.Sleep(m_TickrateMilliSec);
            }
        }

        /// <summary> 
        /// Adding a entity to the list and calls it in the loop.
        /// </summary>
        internal void AddEntity(AIEntity aiEntity) {
            m_AIEntites.Add(aiEntity);
        }
    }
}
