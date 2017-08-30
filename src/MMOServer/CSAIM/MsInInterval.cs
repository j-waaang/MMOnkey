namespace JYW.ThesisMMO.MMOServer.CSAIM {
    using JYW.ThesisMMO.MMOServer.Skills;

    internal struct MsInInterval {
        
        public float MinDistance { get; }
        public float MaxDistance { get; }
        public int MilliSeconds { get; }
        public SkillTarget Target { get; }

        /// <summary> 
        /// Creates consistency interval between min and max with updaterate ms and targeted to target.
        /// </summary>
        public MsInInterval(float min, float max, int ms, SkillTarget target) {
            MinDistance = min;
            MaxDistance = max;
            MilliSeconds = ms;
            Target = target;
        }

        /// <summary> 
        /// Creates consistency interval between min and max with updaterate ms and targeted to all.
        /// </summary>
        public MsInInterval(float min, float max, int ms) {
            MinDistance = min;
            MaxDistance = max;
            MilliSeconds = ms;
            Target = SkillTarget.All;
        }

        public bool IsInInterval(float distance, SkillTarget targetFaction) {
            return
                MinDistance <= distance &&
                MaxDistance > distance &&
                (Target == SkillTarget.All || Target == targetFaction);
        }

        public bool IsZero {
            get {
                return MinDistance == MaxDistance;
            }
        }

        public static MsInInterval Zero {
            get {
                return new MsInInterval(0, 0, 0);
            }
        }

        public override string ToString() {
            return string.Format("MsInInterval - Min: {0}, Max: {1}, Freq: {2}, Group: {3}", MinDistance, MaxDistance, MilliSeconds, Target);
        }
    }
}
