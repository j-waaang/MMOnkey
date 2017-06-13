namespace JYW.ThesisMMO.UnityClient.CombatSystem {
    using System;
    using Common.Codes;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillIcon : SkillCaller {

        [SerializeField] private int m_SlotNumber;
        [SerializeField] private string m_InputAxis;
        private Button m_Button;
        private ActionCode m_Skill;

        internal void ChangeSkill(ActionCode skillname) {
            m_Skill = skillname;
            LoadSprite();
        }

        private void Awake() {
            LoadSkill();
            LoadSprite();

            m_Button = GetComponent<Button>();
        }

        private void Update() {
            if (Input.GetButtonDown(m_InputAxis)) {
                m_Button.onClick.Invoke();
            }
        }

        private void LoadSkill() {
            m_Skill = (ActionCode) GameData.characterSetting.Skills[m_SlotNumber];
        }

        private void LoadSprite() {
            var skillName = m_Skill.ToString();
            var image = Resources.Load<Sprite>(skillName);
            GetComponent<Image>().sprite = image;
            gameObject.name = skillName;
        }

        public void ActivateSkill() {
            RaiseSkillCalledEvent(m_Skill);
        }
    }
}