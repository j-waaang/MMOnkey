using System;
using System.Collections.Generic;

namespace JYW.ThesisMMO.Common.ContinueObjects.InteruptObjects {
    public class InteruptionHandler {
        private Dictionary<string, IInteruptable> InteruptSubscribers = new Dictionary<string, IInteruptable>();

        internal IDisposable AddListener(string name, IInteruptable listener) {
            InteruptSubscribers.Add(name, listener);
            return new UnsubscribeObject(() => RemoveListener(name));
        }

        private void RemoveListener(string name) {
            InteruptSubscribers.Remove(name);
        }

        public void OnInterupt(string name) {
            if(InteruptSubscribers.ContainsKey(name) == false) { return; }

            InteruptSubscribers[name].OnInterupt();
        }

        public void OnInterupt(IEnumerable<string> names) {
            foreach(string name in names) {
                OnInterupt(name);
            }
        }
    }
}
