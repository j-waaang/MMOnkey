using JYW.ThesisMMO.MMOServer.CSAIM;

namespace JYW.ThesisMMO.MMOServer.Skills {
    class OrisonOfHealingData : SkillData {
        public override float MaxRange {
            get {
                return 8F;
            }
        }

        public override MsInInterval GetConsistencyRequirement() {
            return new MsInInterval(MaxRange-1F, MaxRange+1F, 0);
        }
    }
}
