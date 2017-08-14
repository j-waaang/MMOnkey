using JYW.ThesisMMO.MMOServer.Skills;
using System;

namespace JYW.ThesisMMO.MMOServer.CSAIM {
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
    }
}
