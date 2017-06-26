using System;

namespace JYW.ThesisMMO.Common.ContinueObjects.InteruptObjects {
    internal class UnsubscribeObject : IDisposable {

        private Action m_UnsubscribeAction;

        public UnsubscribeObject(Action unsubscribeAction) {
            m_UnsubscribeAction = unsubscribeAction;
        }

        public void Dispose() {
            m_UnsubscribeAction();
        }
    }
}
