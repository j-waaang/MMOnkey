namespace JYW.ThesisMMO.UnityClient.CombatSystem {
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillIcon : MonoBehaviour {

        [SerializeField]
        private string m_Skillname;

        internal void ChangeSkill(string skillname) {
            m_Skillname = skillname;
            LoadSkill();
        }

        private void Awake() {
            LoadSkill();
        }

        private void LoadSkill() {
            var image = GetSkillIcon(m_Skillname);
            GetComponent<Image>().sprite = image;
            gameObject.name = m_Skillname;
        }

        private static Sprite GetSkillIcon(string skillName) {
            return Resources.Load<Sprite>(skillName);
        }
    }
}