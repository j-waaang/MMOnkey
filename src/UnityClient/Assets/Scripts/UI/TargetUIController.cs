namespace JYW.ThesisMMO.UnityClient.UI {

    using JYW.ThesisMMO.UnityClient;
    using JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters;
    using UnityEngine;
    using UnityEngine.UI;

    public class TargetUIController : MonoBehaviour {

        [SerializeField]
        GameObject m_NameUI;
        [SerializeField]
        GameObject m_HealthUI;
        [SerializeField]
        GameObject m_SkillUI;
        private Text m_TargetName;

        private void Awake() {
            GameData.TargetChangedEvent += TargetChanged;
            m_TargetName = m_NameUI.GetComponentInChildren<Text>();
        }

        private void TargetChanged(GameObject newTarget) {
            if (newTarget == null) {
                m_NameUI.SetActive(false);
                m_HealthUI.SetActive(false);
                m_SkillUI.SetActive(false);
                return;
            }

            var remoteChar = newTarget.GetComponent<RemoteCharacterController>();
            m_TargetName.text = remoteChar.CharacterName;
            m_NameUI.SetActive(true);
            m_HealthUI.SetActive(true);
            m_SkillUI.SetActive(false);
        }
    }
}