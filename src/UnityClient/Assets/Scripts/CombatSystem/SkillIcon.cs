namespace JYW.ThesisMMO.UnityClient.CombatSystem {

    using Common.Codes;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillIcon : MonoBehaviour {

        [SerializeField] private int m_SlotNumber;
        private ActionCode m_Skill;

        internal void ChangeSkill(ActionCode skillname) {
            m_Skill = skillname;
            LoadSprite();
        }

        private void Awake() {
            LoadSkill();
            LoadSprite();
        }

        private void LoadSkill() {
            m_Skill = (ActionCode) GameData.characterSetting.Skills[m_SlotNumber];
        }

        private void LoadSprite() {
            var skillName = m_Skill.ToString();
            Debug.LogFormat("Try laoding skill with name {0}", skillName);
            var image = Resources.Load<Sprite>(skillName);
            GetComponent<Image>().sprite = image;
            gameObject.name = skillName;
        }

        public void ActivateSkill() {

        }
    }
}