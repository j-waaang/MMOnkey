namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;
    using System.Collections;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

    /// <summary>
    // Performs the autoattack on input.
    /// </summary>
    [RequireComponent(typeof(ActionStateComponent))]
    public class AutoAttackController : MonoBehaviour {

        private ActionStateComponent m_ActionState;
        private float m_AttackRange;
        private ActionCode m_AutoAttackAction;

        private const float MaxMeeleAARange = 1.5f;
        private const float MaxRangedAARange = 6.0f;
        private const float AADuration = 1f;

        private void Awake() {
            m_ActionState = GetComponent<ActionStateComponent>();
            SetAttackRange();
        }

        /// <summary>
        // Sets the autoAttack range based on the selected weapon.
        /// </summary>
        private void SetAttackRange() {
            var weapon = GameData.characterSetting.Weapon;
            switch ((WeaponCode)weapon) {
                case WeaponCode.Axe:
                    m_AttackRange = MaxMeeleAARange;
                    m_AutoAttackAction = ActionCode.AxeAutoAttack;
                    return;
                case WeaponCode.Bow:
                    m_AttackRange = MaxRangedAARange;
                    m_AutoAttackAction = ActionCode.BowAutoAttack;
                    return;
            }
        }

        private void LateUpdate() {
            if(m_ActionState.ActionState == ActionCode.Idle) {
                if (Input.GetButtonDown("AutoAttack")) {
                    //SetAATarget();
                    if (!TestPrerequisite()) { return; }

                    StartCoroutine(PerformAutoAttack());
                }
            }
        }

        /// <summary>
        // True if preconditions are passed. Else false.
        /// </summary>
        private bool TestPrerequisite() {
            if (m_ActionState.ActionState != ActionCode.Idle) { return false; }

            if (GameData.Target == null) { return false; }

            var distance = Mathf.Abs(GetTargetDistance(GameData.Target));
            if (distance > m_AttackRange) { return false; }

            return true;
        }

        private IEnumerator PerformAutoAttack() {
            m_ActionState.ActionState = m_AutoAttackAction;
            SendAAStartToServer();
            // TODO: Listen to interupt events.
            yield return new WaitForSeconds(AADuration);

            if (m_ActionState.ActionState == m_AutoAttackAction) {
                m_ActionState.ActionState = ActionCode.Idle;
            }
        }

        private void SendAAStartToServer() {
            var targetName = GameData.Target.name;
            RequestOperations.AutoAttackRequest(targetName);
        }

        private float GetTargetDistance(GameObject target) {
            var targetCollider = target.GetComponent<CircleCollider2D>();
            return Vector2.Distance(target.transform.position, transform.position) - targetCollider.radius;
        }
    }
}
