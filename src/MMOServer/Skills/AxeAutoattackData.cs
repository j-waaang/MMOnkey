namespace JYW.ThesisMMO.MMOServer.Skills {
    class AxeAutoAttackData : SkillData {
        public override int CooldownInMs {
            get {
                return 0;
            }
        }

        public override float MaxRange {
            get {
                return 3.5F; ;
            }
        }
    }
}
