using JYW.ThesisMMO.MMOServer.CSAIM;

namespace JYW.ThesisMMO.MMOServer.Skills {
    internal abstract class SkillData {
        public abstract float MaxRange { get; }
        public virtual MsInInterval GetConsistencyRequirement() {
            return new MsInInterval(0, MaxRange, 0);
        }
    }
}
