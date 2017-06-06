namespace JYW.ThesisMMO.UnityClient.Characters {

    using System;
    using UnityEngine;
    using JYW.ThesisMMO.Common.Entities;

    /// <summary>  
    ///  Contains the state of a character and fires onChanged events.
    /// </summary>  
    public class CharacterState : MonoBehaviour {

        internal Action<ActionState> ActionStateChangedEvent;
        internal Action<MovementState> MovementStateChangedEvent;

        private ActionState m_ActionState;
        private MovementState m_MovementState;

        internal ActionState ActionState {
            get {
                return m_ActionState;
            }

            set {
                var oldState = m_ActionState;
                m_ActionState = value;
                if (oldState != value && ActionStateChangedEvent != null) {
                    ActionStateChangedEvent(value);
                }
            }
        }

        internal MovementState MovementState {
            get {
                return m_MovementState;
            }

            set {
                var oldState = m_MovementState;
                m_MovementState = value;
                if (oldState != value && MovementStateChangedEvent != null) {
                    MovementStateChangedEvent(value);
                }
            }
        }
    }
}