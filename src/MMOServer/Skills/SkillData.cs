using System;
using System.Diagnostics;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.Skills {

    using Common.ContinueObjects;
    using JYW.ThesisMMO.MMOServer.CSAIM;
    using Properties;

    internal abstract class SkillData {

        public event Action SkillConsistencyUpdated;
        public abstract float MaxRange { get; }
        public abstract int CooldownInMs { get; }

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        protected int ConsistencyCooldownOffsetInMs = Settings.Default.SkillCooldownIMReactivationOffsetInMs;
        private long m_LastCastTime = long.MinValue;
        private bool m_ConsistencyOnCooldown;

        public bool CanCast {
            get {
                return m_LastCastTime + CooldownInMs < GameTime.TimeMs;
            }
        }

        public virtual MsInInterval GetConsistencyRequirement() {
            if (m_ConsistencyOnCooldown) {
                return MsInInterval.Zero;
            }
            return new MsInInterval(0, MaxRange, 0);
        }

        public void SetOnCooldown() {
            Debug.Assert(m_LastCastTime + CooldownInMs < GameTime.TimeMs);
            m_LastCastTime = GameTime.TimeMs;

            var offConsistencyCooldownIn = CooldownInMs - ConsistencyCooldownOffsetInMs;
            if(offConsistencyCooldownIn < Settings.Default.MinimumConsistencyCooldownInMs) { return; }

            SetConsistencyOnCooldown(offConsistencyCooldownIn);
        }

        private void SetConsistencyOnCooldown(int cooldown) {
            m_ConsistencyOnCooldown = true;
            Debug.Assert(SkillConsistencyUpdated != null);
            SkillConsistencyUpdated();

            var reactivator = new TimedContinueCondition(new TimeSpan(0, 0, 0, 0, cooldown));

            reactivator.ContinueEvent += (CallReason c) => {
                m_ConsistencyOnCooldown = false;
                if(SkillConsistencyUpdated == null) { return; }
                SkillConsistencyUpdated();
            };
            reactivator.Start();
        }
    }
}
