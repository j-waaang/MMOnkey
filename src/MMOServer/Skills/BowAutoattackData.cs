namespace JYW.ThesisMMO.MMOServer.Skills {
    class BowAutoAttackData : SkillData {
        public override int CooldownInMs {
            get {
                return 0;
            }
        }

        public override float MaxRange {
            get {
                return 8.5F; ;
            }
        }
    }
}
