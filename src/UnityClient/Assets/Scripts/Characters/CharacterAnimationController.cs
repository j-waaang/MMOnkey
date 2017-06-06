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
    }
}