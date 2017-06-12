namespace JYW.ThesisMMO.UnityClient.Characters {

    using UnityEngine;
    using JYW.ThesisMMO.Common.Entities;

    /// <summary>  
    ///  Contains the state of a character and fires onChanged events.
    /// </summary>  
    public class CharacterState : MonoBehaviour {

        private ActionState m_ActionState;
        private CharacterAnimationController m_CharacterAnimationController;

        private void Awake() {
            m_CharacterAnimationController = GetComponent<CharacterAnimationController>();
        }
        
        internal ActionState ActionState {
            get {
                return m_ActionState;
            }

            set {
                var oldState = m_ActionState;
                m_ActionState = value;
                if (oldState != value) {
                    m_CharacterAnimationController.TriggerAutoAttackAnimation(m_ActionState);
                }
            }
        }
    }
}