namespace JYW.ThesisMMO.UnityClient.Characters {

    using UnityEngine;
    using Common.Codes;
    using System.Collections;
    using System;
    using System.Collections.Generic;

    /// <summary>  
    ///  Contains the state of a character and fires onChanged events.
    /// </summary>  
    public class ActionStateComponent : MonoBehaviour {

        [SerializeField] private ActionCode m_ActionState = ActionCode.Idle;
        private CharacterAnimationController m_CharacterAnimationController;
        private AOEHintCreator m_AOEHintCreator;
        private Queue<Action> m_MainThreadJobs = new Queue<Action>();

        private void Awake() {
            m_CharacterAnimationController = GetComponent<CharacterAnimationController>();
            m_AOEHintCreator = GetComponent<AOEHintCreator>();
        }
        
        internal ActionCode ActionState {
            get {
                return m_ActionState;
            }

            set {
                var oldState = m_ActionState;
                m_ActionState = value;
                if (oldState != value && oldState != m_ActionState) {
                    m_CharacterAnimationController.TriggerActionAnimation(m_ActionState);
                    m_MainThreadJobs.Enqueue(() => m_AOEHintCreator.PlayDelayedAttackShape(m_ActionState));
                }
            }
        }

        private void Update() {
            while(m_MainThreadJobs.Count > 0) {
                m_MainThreadJobs.Dequeue()();
            }
        }

        internal void SetActionStateForSeconds(ActionCode newState, float seconds) {
            ActionState = newState;
            StartCoroutine(WaitAndIdle(seconds));
        }

        IEnumerator WaitAndIdle(float duration) {
            yield return new WaitForSeconds(duration);
            m_ActionState = ActionCode.Idle;
        }
    }
}