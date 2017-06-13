using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient;
using JYW.ThesisMMO.UnityClient.Characters;
using JYW.ThesisMMO.UnityClient.CombatSystem;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
using UnityEngine;

public class SkillEventListener : MonoBehaviour {

    [SerializeField] private Collider m_GroundCollider;
    [SerializeField] private ActionStateComponent m_PlayerActionState;
    private TargetSelection m_TargetSelector;

    void Start() {
        m_TargetSelector = FindObjectOfType<TargetSelection>();
        var skillCallers = FindObjectsOfType<SkillCaller>();
        foreach (SkillCaller skillCaller in skillCallers) {
            skillCaller.SkillCalledEvent += SkillCalled;
        }
    }

    private void SkillCalled(ActionCode skill) {
        // TODO: Refactor with strategy pattern.

        //No target skills
        switch (skill) {
            case ActionCode.Dash:
                RequestOperations.DashRequest();
                break;
            case ActionCode.HammerBash:
                RequestOperations.HammerBashRequest();
                m_PlayerActionState.SetActionStateForSeconds(skill, 1);
                break;
        }

        //Character target skills
        switch (skill) {
            case ActionCode.DistractingShot:
                if (GameData.Target != null || m_TargetSelector.SetTarget()) {
                    RequestOperations.DistractingShotRequest(GameData.Target.name);
                    m_PlayerActionState.SetActionStateForSeconds(skill, 0.5f);

                }
                break;
            case ActionCode.OrisonOfHealing:
                if (GameData.Target != null || m_TargetSelector.SetTarget()) {
                    RequestOperations.OrisonOfHealingRequest(GameData.Target.name);
                    m_PlayerActionState.SetActionStateForSeconds(skill, 1);
                }
                break;
        }

        //Mouse position as target skills
        switch (skill) {
            case ActionCode.FireStorm:
                var screenPos = Input.mousePosition;
                var screenRay = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (m_GroundCollider.Raycast(screenRay, out hit, 20)) {
                    RequestOperations.FireStormRequest(hit.point);
                    m_PlayerActionState.SetActionStateForSeconds(skill, 2);
                }
                break;
        }

    }
}
