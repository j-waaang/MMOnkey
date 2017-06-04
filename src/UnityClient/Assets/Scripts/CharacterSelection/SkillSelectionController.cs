namespace JYW.ThesisMMO.UnityClient.CharacterSelection {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Common.Codes;

    public class SkillSelectionController : MonoBehaviour {

        [SerializeField] private Dropdown[] m_SkillDropDowns;
        private List<Dropdown.OptionData> m_DropDownDatas = new List<Dropdown.OptionData>();

        private void Awake() {
            LoadSkills();
            FillDropDowns();
        }

        private void LoadSkills() {
            foreach (CombatActionCodes skill in System.Enum.GetValues(typeof(CombatActionCodes))) {
                m_DropDownDatas.Add(new Dropdown.OptionData(skill.ToString()));
            }
        }

        private void FillDropDowns() {
            foreach(Dropdown dropdown in m_SkillDropDowns) {
                dropdown.options = m_DropDownDatas;
            }
        }

        internal int[] GetSelectedSkills() {
            var skills = new List<int>();
            foreach (Dropdown dropdown in m_SkillDropDowns) {
                skills.Add(dropdown.value);
            }
            return skills.ToArray();
        }
    }
}
