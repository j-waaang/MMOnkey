using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient.Characters;

public class CastingBarController : MonoBehaviour {

    [SerializeField] private Image m_CastBar;
    [SerializeField] private Text m_CastText;

    private float m_StartCastTime;
    private float m_CastDuration;
    private INotifyPropertyChanged m_LastTarget;

    private void Awake() {
        m_CastText.text = "";
        m_CastBar.enabled = false;
    }
	
    public void TargetChanged(INotifyPropertyChanged target) {
        UnsubscribeFromLastTarget();
        target.PropertyChanged += ActionStateChanged;
        m_LastTarget = target;
    }

    private void UnsubscribeFromLastTarget() {
        if(m_LastTarget != null) {
            m_LastTarget.PropertyChanged -= ActionStateChanged;
        }
    }

    private void ActionStateChanged(object sender, PropertyChangedEventArgs e) {
        if(e.PropertyName != "ActionState") {
            Debug.LogErrorFormat("Subscribed to wrong property {0}.", e.PropertyName);
            return;
        }

        var actionState = ((ActionStateComponent)sender).ActionState;
        m_CastDuration = 0;

        switch (actionState) {
            case ActionCode.AxeAutoAttack:
            case ActionCode.BowAutoAttack:
            case ActionCode.DistractingShot:
                m_CastDuration = 0.5f;
                break;
            case ActionCode.Idle:
                InteruptCasting();
                return;
            case ActionCode.FireStorm:
                m_CastDuration = 2f;
                break;
            case ActionCode.HammerBash:
            case ActionCode.OrisonOfHealing:
                m_CastDuration = 1f;
                break;
            default:
                return;
        }
        CastSkill(actionState, m_CastDuration);
    }

    private void CastSkill(ActionCode skill, float durationInSeconds) {
        m_CastBar.enabled = true;
        m_CastBar.fillAmount = 0;
        m_StartCastTime = Time.time;

        m_CastText.enabled = true;
        m_CastText.text = skill.ToString();
    }

    private void InteruptCasting() {
        m_CastBar.enabled = false;
        m_CastText.enabled = false;
    }

    private void Update() {
        if (m_CastBar.enabled) {
            FillCastBar();
        }
    }

    private void FillCastBar() {
        var passedCastTime = Time.time - m_StartCastTime;
        m_CastBar.fillAmount = passedCastTime / m_CastDuration;
    }
}
