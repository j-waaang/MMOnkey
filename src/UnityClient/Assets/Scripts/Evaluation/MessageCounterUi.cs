using UnityEngine;
using UnityEngine.UI;

public class MessageCounterUi : MonoBehaviour {

    [SerializeField]
    private Text m_Text;
    [SerializeField]
    private GameObject m_Panel;
    private const string PanelToggleButton = "ToggleMsgCounter";

    private void Awake() {
        EvaluationManager.UPSevent += EvaluationManager_UPSevent;
    }

    private void EvaluationManager_UPSevent(int pos, int filPos) {
        m_Text.text = string.Format("Updates/s with CSAIM: {0}\nUpdates/s without CSAIM: {1}\nPercent Filtered: {2}",
            pos,
            pos + filPos,
            (int)(100F * filPos / (pos + filPos)));
    }

    private void Update() {
        if (Input.GetButtonDown(PanelToggleButton)) {
            m_Panel.SetActive(!m_Panel.activeSelf);
        }
    }
}
