namespace JYW.ThesisMMO.MMOServer.Skills {
    internal class FireStormData : SkillData {
        public override int CooldownInMs {
            get {
                return 8000;
            }
        }

        public override float MaxRange {
            get {
                return 7F;
            }
        }
    }
}
