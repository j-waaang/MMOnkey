namespace JYW.ThesisMMO.UnityClient.CharacterSelection {

    using UnityEngine;

    public class CharacterCreationController : MonoBehaviour {

        public string Username { get; set; }
        [SerializeField] SkillSelectionController m_SkillSelectionController;
        [SerializeField] WeaponSelectionController m_WeaponSelectionController;

        internal CharacterSetting GetCharacterSetting() {
            if (string.IsNullOrEmpty(Username)) { return null; }

            var characterSetting = new CharacterSetting() {
                Name = Username,
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
