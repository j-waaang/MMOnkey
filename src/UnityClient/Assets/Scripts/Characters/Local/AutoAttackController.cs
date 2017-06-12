namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;
    using JYW.ThesisMMO.Common.Entities;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
    using RemoteCharacters;
    using System.Collections;

    /// <summary>
    // Performs the autoattack on input.
    /// </summary>
    [RequireComponent(typeof(CharacterState))]
    public class AutoAttackController : MonoBehaviour {

        private CharacterState m_CharacterState;
        private float m_AttackRange;

        private const float MaxMeeleAARange = 1.5f;
        private const float MaxRangedAARange = 6.0f;
        private const float MeeleAADuration = 1f;

        private void Awake() {
            m_CharacterState = GetComponent<CharacterState>();
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
                    return;
                case WeaponCode.Bow:
                    m_AttackRange = MaxRangedAARange;
                    return;
            }
        }

        private void LateUpdate() {
            switch (m_CharacterState.ActionState) {
                case ActionState.Idle:
                    if (Input.GetButtonDown("AutoAttack")) {
                        //SetAATarget();
                        if (!TestPrerequisite()) { return; }

                        StartCoroutine(PerformAutoAttack());
                    }
                    break;
                case ActionState.Casting:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        // True if preconditions are passed. Else false.
        /// </summary>
        private bool TestPrerequisite() {
            if (m_CharacterState.ActionState == ActionState.Casting) { return false; }

            if (GameData.Target == null) { return false; }

            var distance = Mathf.Abs(GetTargetDistance(GameData.Target));
            if (distance > m_AttackRange) { return false; }

            return true;
        }

        private IEnumerator PerformAutoAttack() {
            m_CharacterState.ActionState = ActionState.Casting;
            SendAAStartToServer();
            // TODO: Listen to interupt events.
            yield return new WaitForSeconds(MeeleAADuration);
            if (m_CharacterState.ActionState == ActionState.Casting) {
                m_CharacterState.ActionState = ActionState.Idle;
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
