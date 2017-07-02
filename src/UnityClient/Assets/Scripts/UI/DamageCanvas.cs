using JYW.ThesisMMO.UnityClient.Characters.RemoteCharacters;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;
using UnityEngine.UI;

public class DamageCanvas : MonoBehaviour {

    [SerializeField] private Text m_DamageHintPrefab;
    [SerializeField] private Text m_HealHintPrefab;

    public void Start() {
        EventOperations.HealthUpdatedEvent += OnHealthUpdate;
    }

    private void OnHealthUpdate(string name, int damage, int health) {
        if(CharacterSpawner.RemoteCharacters.ContainsKey(name) == false) { return; }

        var pos = CharacterSpawner.RemoteCharacters[name].transform.position;
        pos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        CreateDamageHint(damage, pos);
    }

    private void CreateDamageHint(int damage, Vector3 screenPos) {
        var hint = damage < 0 ? Instantiate(m_DamageHintPrefab) : Instantiate(m_HealHintPrefab);
        hint.transform.position = screenPos;
        hint.transform.SetParent(transform, false);
        hint.text = damage.ToString();
    }
}
