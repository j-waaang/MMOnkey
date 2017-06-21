using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient;
using JYW.ThesisMMO.UnityClient.Characters;
using JYW.ThesisMMO.UnityClient.CombatSystem;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
using System;
using System.Collections;
using UnityEngine;

public class SkillEventListener : MonoBehaviour {

    [SerializeField]
    private Collider m_GroundCollider;
    [SerializeField]
    private GameObject m_Player;

    private ActionStateComponent m_PlayerActionState;
    private RotationController m_PlayerRotationController;
    private TargetSelection m_TargetSelector;

    void Start() {
        m_PlayerActionState = m_Player.GetComponent<ActionStateComponent>();
        m_PlayerRotationController = m_Player.GetComponent<RotationController>();

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
                CastSkill(skill, Vector3.back, 1f);
                break;
        }

        //Character target skills
        switch (skill) {
            case ActionCode.DistractingShot:
                if (GameData.Target != null || m_TargetSelector.SetTarget()) {
                    RequestOperations.DistractingShotRequest(GameData.Target.name);
                    CastSkill(skill, GameData.Target.transform.position, 0.5f);
                }
                break;
            case ActionCode.OrisonOfHealing:
                if (GameData.Target != null || m_TargetSelector.SetTarget()) {
                    RequestOperations.OrisonOfHealingRequest(GameData.Target.name);
                    CastSkill(skill, GameData.Target.transform.position, 1f);
                }
                break;
        }

        //Mouse position as target skills
        switch (skill) {
            case ActionCode.FireStorm:
                var screenPos = Input.mousePosition;
                var screenRay = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit hit;
                if (m_GroundCollider.Raycast(screenRay, out hit, 20f)) {
                    RequestOperations.FireStormRequest(hit.point);
                    CastSkill(skill, hit.point, 2f, () => SkillEntitySpawner.CreateSkillEntity(skill, hit.point));
                }
                break;
        }
    }

    IEnumerator WaitAndCall(float waitTime, Action method) {
        yield return new WaitForSeconds(waitTime);
        // TODO: check for incoming interupt events
        method();
    }

    private void CastSkill(ActionCode action, Vector3 lookPoint, float duration) {
        m_PlayerActionState.SetActionStateForSeconds(action, duration);
        m_PlayerRotationController.LookAtPoint(lookPoint, duration);
    }

    private void CastSkill(ActionCode action, Vector3 lookPoint, float duration, Action onFinishCast) {
        CastSkill(action, lookPoint, duration);
        StartCoroutine(WaitAndCall(2, onFinishCast));
    }
}
