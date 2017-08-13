namespace JYW.ThesisMMO.MMOServer.CSAIM {
    internal struct MsInInterval {
        public float MinDistance { get; }
        public float MaxDistance { get; }
        public int MilliSeconds { get; }

        public MsInInterval(float min, float max, int ms) {
            MinDistance = min;
            MaxDistance = max;
            MilliSeconds = ms;
        }

        public bool IsInInterval(float distance) {
            return MinDistance <= distance && MaxDistance > distance;
        }

        public bool IsZero {
            get {
                return MinDistance == MaxDistance && MaxDistance == MinDistance && MilliSeconds == 0;
            }
        }

        public static MsInInterval Zero {
            get {
                return new MsInInterval(0, 0, 0);
            }
        }
    }
}
