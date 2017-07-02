using UnityEngine;
using UnityEngine.UI;

public class LocalHealthUI : MonoBehaviour {

    private Image m_Image;

    private void Start () {
        m_Image = transform.GetChild(0).GetComponent<Image>();

        var player = GameObject.FindGameObjectWithTag("Player");
        var healthComp = player.GetComponent<HealthComponent>();
        healthComp.HealthUpdatedEvent += HealthUpdate;
	}

    private void HealthUpdate(int dmg, int health, int maxHealth) {
        m_Image.fillAmount = (float)health / maxHealth;
    }
}
