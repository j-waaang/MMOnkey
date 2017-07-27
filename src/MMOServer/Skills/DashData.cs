using System;
using JYW.ThesisMMO.MMOServer.CSAIM;

namespace JYW.ThesisMMO.MMOServer.Skills {
    class DashData : SkillData {
        public override float MaxRange {
            get {
                return 0F;
            }
        }

        public override MsInInterval GetConsistencyRequirement() {
            return new MsInInterval(0, 0, 0);
        }
    }
}
