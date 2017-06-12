namespace JYW.ThesisMMO.UnityClient.Characters {

    using Common.Entities;
    using UnityEngine;

    public class CharacterAnimationController : MonoBehaviour {

        private Animator m_Animator;

        private void Awake() {
            m_Animator = transform.GetChild(0).GetComponent<Animator>();
        }

        public void UpdateRunningAnimation(MovementState newState) {
            switch (newState) {
                case MovementState.Idle:
                    m_Animator.SetBool("Running", false);
                    break;
                case MovementState.Moving:
                    m_Animator.SetBool("Running", true);
                    break;
            }
        }

        public void TriggerAutoAttackAnimation(ActionState newState) {
            m_Animator.SetTrigger("AutoAttack");
        }
    }
}