namespace JYW.ThesisMMO.UnityClient.CharacterSelection {

    using UnityEngine;

    public class CharacterCreationController : MonoBehaviour {

        public string Username { get; set; }
        public string Teamname { get; set; }
        [SerializeField] private SkillSelectionController m_SkillSelectionController;
        [SerializeField] private WeaponSelectionController m_WeaponSelectionController;

        internal CharacterSetting GetCharacterSetting() {
            if (string.IsNullOrEmpty(Username)) { return null; }
            if (string.IsNullOrEmpty(Teamname)) { return null; }

            var characterSetting = new CharacterSetting() {
                Name = Username,
                Team = Teamname,
                Weapon = m_WeaponSelectionController.GetSelectedWeapon(),
                Skills = m_SkillSelectionController.GetSelectedSkills()
            };

            UpdateGameData(characterSetting);

            return characterSetting;
        }

        private void UpdateGameData(CharacterSetting character) {
            GameData.characterSetting = character;
        }
    }
}
