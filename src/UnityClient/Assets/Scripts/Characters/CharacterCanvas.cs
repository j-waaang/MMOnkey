using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class CharacterCanvas : MonoBehaviour {

    private DamageHint m_HintPrefab;
    private Transform m_CanvasT;

    private void Awake() {
        CreateCanvas();
        LoadPrefabs();
        GetComponent<HealthComponent>().DamageTakenEvent += DisplayDamage;
    }

    private void CreateCanvas() {
        m_CanvasT = (Instantiate(Resources.Load("CharacterCanvas", typeof(GameObject)), transform) as GameObject).transform;
    }

    private void LoadPrefabs() {
        m_HintPrefab = Resources.Load("DamageHint", typeof(DamageHint)) as DamageHint;
    }

    private void DisplayDamage(int damage) {
        Instantiate(m_HintPrefab, m_CanvasT).Initialize(damage);
    }
}
