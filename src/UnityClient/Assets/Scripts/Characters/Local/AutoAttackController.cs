namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;
    using System.Collections;

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
        //private float m_AttackRange;
        private ActionCode m_AutoAttackAction;

        //private const float MaxMeeleAARange = 1f;
        //private const float MaxRangedAARange = 6.0f;
        private static readonly TimeSpan AADURATION = new TimeSpan(0, 0, 1);

        private void Awake() {
            m_RotationController = GetComponent<RotationController>();
            m_ActionState = GetComponent<ActionStateComponent>();
            SetAutoAttackType();
            //SetAttackRange();
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

        ///// <summary>
        //// Sets the autoAttack range based on the selected weapon.
        ///// </summary>
        //private void SetAttackRange() {
        //    var weapon = GameData.characterSetting.Weapon;
        //    switch ((WeaponCode)weapon) {
        //        case WeaponCode.Axe:
        //            m_AttackRange = MaxMeeleAARange;
        //            m_AutoAttackAction = ActionCode.AxeAutoAttack;
        //            return;
        //        case WeaponCode.Bow:
        //            m_AttackRange = MaxRangedAARange;
        //            m_AutoAttackAction = ActionCode.BowAutoAttack;
        //            return;
        //    }
        //}

        private void Update() {
            if (m_ActionState.ActionState != ActionCode.Idle) { return; }

            if (Input.GetButtonDown("AutoAttack")) {
                var mousePoint = InputExtension.GetMouseHitGroundPoint();
                if (mousePoint == null) { return; }

                var forwardVec = mousePoint.Value - transform.position;
                forwardVec = forwardVec.normalized;
                PerformAutoAttack(forwardVec);
                //StartCoroutine(PerformAutoAttack());
            }
        }

        //// Lastupdate because target is set in update.
        //private void LateUpdate() {
        //    if (m_ActionState.ActionState == ActionCode.Idle) {
        //        if (Input.GetButtonDown("AutoAttack")) {
        //            //SetAATarget();
        //            if (!TestPrerequisite()) { return; }

        //            m_RotationController.LookAt(GameData.Target.transform.position - transform.position, 1f);

        //            StartCoroutine(PerformAutoAttack());
        //        }
        //    }
        //}

        ///// <summary>
        //// True if preconditions are passed. Else false.
        ///// </summary>
        //private bool TestPrerequisite() {
        //    if (m_ActionState.ActionState != ActionCode.Idle) { return false; }

        //    if (GameData.Target == null) { return false; }

        //    var distance = Mathf.Abs(GetTargetDistance(GameData.Target));
        //    if (distance > m_AttackRange) { return false; }

        //    return true;
        //}

        //private IEnumerator PerformAutoAttack() {
        //    m_ActionState.ActionState = m_AutoAttackAction;
        //    SendAAStartToServer();
        //    // TODO: Listen to interupt events.
        //    yield return new WaitForSeconds(AADURATION);

        //    if (m_ActionState.ActionState == m_AutoAttackAction) {
        //        m_ActionState.ActionState = ActionCode.Idle;
        //    }
        //}

        //private void SendAAStartToServer() {
        //    var targetName = GameData.Target.name;
        //    RequestOperations.AxeAutoAttackRequest(targetName);
        //}

        //private float GetTargetDistance(GameObject target) {
        //    var targetCollider = target.GetComponent<BoxCollider>();
        //    return Vector2.Distance(target.transform.position, transform.position) - targetCollider.size.x;
        //}

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
