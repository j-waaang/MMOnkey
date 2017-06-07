namespace JYW.ThesisMMO.UnityClient.Characters {

    using Common.Entities;
    using UnityEngine;

    [RequireComponent(typeof(CharacterState))]
    public class CharacterAnimationController : MonoBehaviour {

        private Animator m_Animator;

        private void Awake() {
            m_Animator = transform.GetChild(0).GetComponent<Animator>();

            var characterState = GetComponent<CharacterState>();
            characterState.MovementStateChangedEvent += UpdateRunningAnimation;

            characterState.ActionStateChangedEvent += PerformAutoAttackAnimation;
        }

        private void UpdateRunningAnimation(MovementState newState) {
            switch (newState) {
                case MovementState.Idle:
                    m_Animator.SetBool("Running", false);
                    break;
                case MovementState.Moving:
                    m_Animator.SetBool("Running", true);
                    break;
            }
        }

        private void PerformAutoAttackAnimation(ActionState newState) {
            switch (newState) {
                case ActionState.Idle:
                    break;
                case ActionState.Casting:
                    //m_Animator.SetBool("Attack", true);
                    m_Animator.SetTrigger("AutoAttack");
                    break;
                default:
                    break;
            }
        }
    }
}