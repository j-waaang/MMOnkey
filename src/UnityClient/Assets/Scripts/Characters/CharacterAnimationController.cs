namespace JYW.ThesisMMO.UnityClient.Characters {
    using Common.Codes;
    using Common.Entities;
    using UnityEngine;

    public class CharacterAnimationController : MonoBehaviour {

        private Animator m_Animator;

        private void Awake() {
            m_Animator = GetComponentInChildren<Animator>();
        }


        ////TODO: Optimize this.
        //private Animator m_Animator { get {
        //        return transform.GetChild(1).GetComponent<Animator>();
        //    }
        //}

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

        public void TriggerActionAnimation(ActionCode actionCode) {

            float m_AnimationSpeed = 1;

            switch (actionCode) {
                case ActionCode.Idle:
                case ActionCode.Dash:
                    return;
                case ActionCode.DistractingShot:
                    m_AnimationSpeed = 2f;
                    break;
                case ActionCode.FireStorm:
                    m_AnimationSpeed = 0.5f;
                    break;
            }

            m_Animator.SetFloat("AnimationSpeed", m_AnimationSpeed);
            m_Animator.SetTrigger("AutoAttack");
        }

        private void OnDeath() {
            m_Animator.SetTrigger("DeathTrigger");
        }
    }
}