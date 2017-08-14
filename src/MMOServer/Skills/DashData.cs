using System;
using JYW.ThesisMMO.MMOServer.CSAIM;

namespace JYW.ThesisMMO.MMOServer.Skills {
    class DashData : SkillData {
        public override int CooldownInMs {
            get {
                return 10000;
            }
        }

        public override float MaxRange {
            get {
                return 0F;
            }
        }

        public override MsInInterval GetConsistencyRequirement() {
            return MsInInterval.Zero;
        }
    }
}
