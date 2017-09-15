using JYW.ThesisMMO.Common.Codes;

namespace JYW.ThesisMMO.UnityClient.CharacterSelection {

    public class CharacterSetting {

        public string Name;
        public int Weapon;
        public int[] Skills;
        public string Team;

        public string GetWeaponAndSkills() {
            string str = ((ActionCode)Weapon).ToString();
            foreach(var skill in Skills) {
                str += " " + (ActionCode)skill;
            }
            return str;
        }
    }
}
