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
    }
}
