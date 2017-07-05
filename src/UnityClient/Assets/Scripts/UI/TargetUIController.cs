namespace JYW.ThesisMMO.UnityClient.UI {
    using Characters;
    using JYW.ThesisMMO.UnityClient;
    using UnityEngine;
    using UnityEngine.UI;

    public class TargetUIController : MonoBehaviour {

        [SerializeField] private GameObject m_NameUI;
        private Text m_TargetName;

        [SerializeField] private GameObject m_HealthUI;
        private Image m_HealthImage;

        [SerializeField] private GameObject m_SkillUI;
        private CastingBarController m_CastingBarController;

        private void Awake() {
            DisableUI();
            GameData.TargetChangedEvent += TargetChanged;
            m_TargetName = m_NameUI.GetComponentInChildren<Text>();
            m_HealthImage = m_HealthUI.transform.GetChild(0).GetComponent<Image>();
            m_CastingBarController = m_SkillUI.GetComponent<CastingBarController>();
        }

        private void TargetChanged(GameObject newTarget) {
            if (newTarget == null) {
                DisableUI();
                return;
            }

            // Name
            m_TargetName.text = newTarget.name;
            m_NameUI.SetActive(true);

            // Health bar
            var healthComponent = newTarget.GetComponent<HealthComponent>();
            m_HealthImage.fillAmount = (float)healthComponent.Health / healthComponent.MaxHealth;
            m_HealthUI.SetActive(true);
            healthComponent.HealthUpdatedEvent += OnHealthUpdated;

            // Action bar
            m_CastingBarController.TargetChanged(newTarget.GetComponent<ActionStateComponent>());
            m_SkillUI.SetActive(true);
        }

        private void OnHealthUpdated(int damage, int health, int maxHealth) {
            m_HealthImage.fillAmount = (float)health / maxHealth;
        }

        private void DisableUI() {
            m_NameUI.SetActive(false);
            m_HealthUI.SetActive(false);
            m_SkillUI.SetActive(false);
        }
    }
}