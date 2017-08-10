namespace JYW.ThesisMMO.MMOServer.Skills {

    using JYW.ThesisMMO.MMOServer.CSAIM;
    using System.Diagnostics;

    internal abstract class SkillData {
        public abstract float MaxRange { get; }
        public abstract int CooldownInMs { get; }

        private long m_LastCastTime = long.MinValue;

        public bool CanCast {
            get {
                return m_LastCastTime + CooldownInMs < GameTime.TimeMs;
            }
        }
        public virtual MsInInterval GetConsistencyRequirement() {
            return new MsInInterval(0, MaxRange, 0);
        }

        public void SetOnCooldown() {
            Debug.Assert(m_LastCastTime + CooldownInMs < GameTime.TimeMs);
            m_LastCastTime = GameTime.TimeMs;
        }
    }
}
