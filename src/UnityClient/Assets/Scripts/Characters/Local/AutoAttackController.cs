namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;
    using JYW.ThesisMMO.Common.Entities;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
    using RemoteCharacters;

    /// <summary>
    // Performs the autoattack on input.
    /// </summary>
    [RequireComponent(typeof(CharacterState))]
    public class AutoAttackController : MonoBehaviour {

        private CharacterState m_CharacterState;
        private float m_AttackRange;
        //private GameObject m_Target;

        private const float MaxMeeleAARange = 1.5f;
        private const float MaxRangedAARange = 6.0f;

        private void Awake() {
            m_CharacterState = GetComponent<CharacterState>();
            SetAttackRange();
        }

        /// <summary>
        // Sets the autoAttack range based on the selected weapon.
        /// </summary>
        private void SetAttackRange() {
            var weapon = GameData.characterSetting.Weapon;
            switch ((AutoAttackCodes)weapon) {
                case AutoAttackCodes.Meele:
                    m_AttackRange = MaxMeeleAARange;
                    return;
                case AutoAttackCodes.Ranged:
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
                        StartAutoAttack();
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
            if(m_CharacterState.ActionState == ActionState.Casting) { return false; }
            
            if(GameData.Target == null) { return false; }

            var distance = Mathf.Abs(GetTargetDistance(GameData.Target));
            if(distance > m_AttackRange) { return false; }

            return true;
        }

        private void StartAutoAttack() {
            m_CharacterState.ActionState = ActionState.Casting;
            SendAAStartToServer();
        }

        private void SendAAStartToServer() {
            var targetName = GameData.Target.GetComponent<RemoteCharacterController>().CharacterName;
            RequestOperations.AutoAttackRequest(targetName);
        }

        ///// <summary>
        //// Look for a target unter the mouse position and returns it.
        ///// </summary>
        //private void SetAATarget() {
        //    var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    var target = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

        //    if (target.collider != null && target.collider.tag == "Enemy") {
        //        m_Target = target.collider.gameObject;
        //    }
        //    else {
        //        m_Target = null;
        //    }
        //}

        private float GetTargetDistance(GameObject target) {
            var targetCollider = target.GetComponent<CircleCollider2D>();
            return Vector2.Distance(target.transform.position, transform.position) - targetCollider.radius;
        }
    }
}
