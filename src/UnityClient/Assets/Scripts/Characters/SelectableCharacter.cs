using cakeslice;
using JYW.ThesisMMO.UnityClient;
using UnityEngine;

public class SelectableCharacter : MonoBehaviour {

    private static Outline LastOutline;
    private Outline m_Outline;

    private void Awake() {
        m_Outline = GetComponent<Outline>();
    }

    private void OnMouseDown() {
        NotifyTargetSelected();
        UpdateOutline();
    }

    private void NotifyTargetSelected() {
        if(LastOutline == m_Outline) { return; }
        GameData.TargetChangedAction(transform.parent.gameObject);
    }

    private void UpdateOutline() {
        if (LastOutline != null) {
            LastOutline.eraseRenderer = true;
        }
        m_Outline.eraseRenderer = false;
        LastOutline = m_Outline;
    }
}
