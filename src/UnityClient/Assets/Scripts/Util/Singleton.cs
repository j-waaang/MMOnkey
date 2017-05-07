namespace JYW.ThesisMMO.UnityClient.Util {
    public class Singleton<T> {
        private static T _instance;
        private static object _lock = new object();

        public static T Instance {
            get {
                lock (_lock) {
                    return _instance;
                }
            }

            protected set {
                lock (_lock) {
                    _instance = value;
                }
            }
        }
    }
}
