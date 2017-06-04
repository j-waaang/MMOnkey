namespace JYW.ThesisMMO.UnityClient.CombatSystem {

    using Common.Codes;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillIcon : MonoBehaviour {

        [SerializeField] private int m_SlotNumber;
        private CombatActionCodes m_Skill;

        internal void ChangeSkill(CombatActionCodes skillname) {
            m_Skill = skillname;
            LoadSprite();
        }

        private void Awake() {
            LoadSkill();
            LoadSprite();
        }

        private void LoadSkill() {
            m_Skill = (CombatActionCodes) GameData.characterSetting.Skills[m_SlotNumber];
        }

        private void LoadSprite() {
            var skillName = m_Skill.ToString();
            var image = Resources.Load<Sprite>(skillName);
            GetComponent<Image>().sprite = image;
            gameObject.name = skillName;
        }
    }
}