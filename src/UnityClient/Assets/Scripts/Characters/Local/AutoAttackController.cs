namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
    using Common.ContinueObjects;
    using System;

    /// <summary>
    // Performs the autoattack on input.
    /// </summary>
    [RequireComponent(typeof(ActionStateComponent))]
    public class AutoAttackController : MonoBehaviour {

        private ActionStateComponent m_ActionState;
        private RotationController m_RotationController;
        private ActionCode m_AutoAttackAction;

        private static readonly TimeSpan AADURATION = new TimeSpan(0, 0, 1);

        private void Awake() {
            m_RotationController = GetComponent<RotationController>();
            m_ActionState = GetComponent<ActionStateComponent>();
            SetAutoAttackType();
        }

        private void SetAutoAttackType() {
            var weapon = GameData.characterSetting.Weapon;
            switch ((WeaponCode)weapon) {
                case WeaponCode.Axe:
                    m_AutoAttackAction = ActionCode.AxeAutoAttack;
                    return;
                case WeaponCode.Bow:
                    m_AutoAttackAction = ActionCode.BowAutoAttack;
                    return;
            }
        }

        private void Update() {
            if (m_ActionState.ActionState != ActionCode.Idle) { return; }

            if (Input.GetButtonDown("AutoAttack")) {
                var mousePoint = InputExtension.GetMouseHitGroundPoint();
                if (mousePoint == null) { return; }
                var forwardVec = mousePoint.Value - transform.position;
                forwardVec = forwardVec.normalized;
                PerformAutoAttack(forwardVec);
            }
        }
        
        private void PerformAutoAttack(Vector3 lookDirection) {
            m_ActionState.ActionState = m_AutoAttackAction;
            m_RotationController.LookAt(lookDirection, AADURATION);

            switch (m_AutoAttackAction) {
                case ActionCode.AxeAutoAttack:
                    RequestOperations.AxeAutoAttackRequest(lookDirection);
                    break;
                case ActionCode.BowAutoAttack:
                    RequestOperations.BowAutoAttackRequest(lookDirection);
                    break;
            }

            var setIdleCondition = new TimedContinueCondition(AADURATION);
            setIdleCondition.ContinueEvent += (CallReason cr) => m_ActionState.ActionState = ActionCode.Idle;
            setIdleCondition.Start();
        }
    }
}
