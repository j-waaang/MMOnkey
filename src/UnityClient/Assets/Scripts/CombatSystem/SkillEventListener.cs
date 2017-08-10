using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient;
using JYW.ThesisMMO.UnityClient.Characters;
using JYW.ThesisMMO.UnityClient.CombatSystem;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Responses;
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
                var hbAction = new Action(() => CastSkill(skill, Vector3.back, 1f));
                ResponseOperations.AddActionToResponseWaitinglist(skill, hbAction);
                RequestOperations.HammerBashRequest();
                break;
        }

        //Character target skills
        switch (skill) {
            case ActionCode.DistractingShot:
                var mousePoint = InputExtension.GetMouseHitGroundPoint();
                if (mousePoint == null) { return; }

                var forwardVec = mousePoint.Value - m_Player.transform.position;
                forwardVec = forwardVec.normalized;
                var dsAction = new Action(() => CastSkill(skill, mousePoint.Value, 0.5f));
                ResponseOperations.AddActionToResponseWaitinglist(skill, dsAction);
                RequestOperations.DistractingShotRequest(forwardVec);
                break;

            case ActionCode.OrisonOfHealing:
                if (GameData.Target != null || m_TargetSelector.SetTarget()) {
                    var oohAction = new Action(() => CastSkill(skill, GameData.Target.transform.position, 1f));
                    ResponseOperations.AddActionToResponseWaitinglist(skill, oohAction);
                    RequestOperations.OrisonOfHealingRequest(GameData.Target.name);
                    //CastSkill(skill, GameData.Target.transform.position, 1f);
                }
                break;
        }

        //Mouse position as target skills
        switch (skill) {
            case ActionCode.FireStorm:
                var mousePoint = InputExtension.GetMouseHitGroundPoint();
                if (mousePoint == null) { return; }

                var action = new Action(() => CastSkill(skill, mousePoint.Value, 2f));
                ResponseOperations.AddActionToResponseWaitinglist(skill, action);
                RequestOperations.FireStormRequest(mousePoint.Value);
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
