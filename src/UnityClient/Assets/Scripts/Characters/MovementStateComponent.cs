using JYW.ThesisMMO.Common.Entities;
using JYW.ThesisMMO.UnityClient.Characters;
using UnityEngine;

public class MovementStateComponent : MonoBehaviour {

    private CharacterAnimationController m_CharacterAnimationController;

    private void Awake() {
        m_CharacterAnimationController = GetComponent<CharacterAnimationController>();
    }

    private MovementState m_MovementState;
    public MovementState MovementState {
        get {
            return m_MovementState;
        }

        set {
            if (value == m_MovementState) { return; }

            m_MovementState = value;
            m_CharacterAnimationController.UpdateRunningAnimation(value);
        }
    }
}
