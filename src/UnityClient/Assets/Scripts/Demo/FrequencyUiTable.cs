using System.Collections.Generic;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;
using UnityEngine.UI;

public class FrequencyUiTable : MonoBehaviour {
    [SerializeField] private GameObject m_Panel;
    [SerializeField] private Text m_Text;

    private const string ToggleHideKey = "ToggleFreqTable";

    private void Awake() {
        EventOperations.FrequencyTableEvent += OnNewTable;
    }

    private void OnNewTable(IEnumerable<FrequencyEntry> table) {
        var text = "";
        foreach (var element in table) {
            text += string.Format("{0}-{1}, MS: {2}, Team: {3}\n", element.MinDistance, element.MaxDistance, element.MilliSeconds, element.Target);
        }
        m_Text.text = text;
    }

    private void Update() {
        if (Input.GetButtonDown(ToggleHideKey)) {
            m_Panel.SetActive(!m_Panel.activeSelf);
        }
    }
}
