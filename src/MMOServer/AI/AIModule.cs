namespace JYW.ThesisMMO.MMOServer.AI {

    using System.Collections.Generic;
    using System.Threading;

    /// <summary> 
    /// Controlls the AI loop.
    /// </summary>
    class AIModule {

        private List<AIEntity> m_AIEntites = new List<AIEntity>();
        private Thread m_AIThread;
        private bool m_Running;
        private const int m_TickrateMilliSec = 300;

        private void StartLoop() {
            m_Running = true;
            m_AIThread = new Thread(AILoop);
            m_AIThread.Start();
        }

        private void StopLoop() {
            m_Running = false;
        }

        private void AILoop() {
            while (m_Running) {
                foreach(AIEntity entity in m_AIEntites) {
                    entity.Tick();
                }
                Thread.Sleep(m_TickrateMilliSec);
            }
        }
    }
}
