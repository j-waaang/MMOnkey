using System;
using System.Collections.Generic;
using System.Threading;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.AI {

    /// <summary> 
    /// Controlls the AI loop and holds AI Entities.
    /// </summary>
    internal class AILooper : IDisposable{

        private List<AIEntity> m_AIEntites = new List<AIEntity>();
        private Thread m_AIThread;
        private bool m_Running;
        private const int m_TickrateMilliSec = 300;

        private static AILooper m_Instance = null;
        private static readonly object m_Lock = new object();
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static AILooper Instance {
            get {
                if (m_Instance == null) {
                    lock (m_Lock) {
                        if (m_Instance == null) {
                            m_Instance = new AILooper();
                        }
                    }
                }
                return m_Instance;
            }
        }

        private AILooper() {
            m_Running = true;
            m_AIThread = new Thread(AILoop);
            m_AIThread.Start();
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

        internal void RemoveEntity(AIEntity aiEntity) {
            m_AIEntites.Remove(aiEntity);
        }

        public void Dispose() {
            m_Running = false;
        }
    }
}
