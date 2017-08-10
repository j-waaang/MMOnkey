namespace JYW.ThesisMMO.MMOServer.Skills {
    class DistractingShotData : SkillData {
        public override int CooldownInMs {
            get {
                return 8000;
            }
        }

        public override float MaxRange {
            get {
                return 8F; ;
            }
        }
    }
}
