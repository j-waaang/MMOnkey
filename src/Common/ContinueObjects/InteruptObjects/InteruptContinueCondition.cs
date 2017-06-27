using System;

namespace JYW.ThesisMMO.Common.ContinueObjects.InteruptObjects {
    public class InteruptContinueCondition : ActionContinueCondition, IInteruptable {

        private static InteruptionHandler m_InteruptionHandler;
        public static void SetInteruptionHandler(InteruptionHandler interuptionHandler) {
            m_InteruptionHandler = interuptionHandler;
        }

        private IDisposable m_Listener;
        private string m_Name;

        public InteruptContinueCondition(string name) {
            m_Name = name;
        }

        public void OnInterupt() {
            m_Listener.Dispose();
            RaiseContinueEvent(CallReason.Interupted);
        }

        public override void Start() {
            m_Listener = m_InteruptionHandler.AddListener(m_Name, this);
        }

        public override void Dispose() {
            m_Listener.Dispose();
            base.Dispose();
        }
    }
}
