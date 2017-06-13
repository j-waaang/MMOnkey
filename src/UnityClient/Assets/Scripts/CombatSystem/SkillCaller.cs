namespace JYW.ThesisMMO.UnityClient.CombatSystem {
    using JYW.ThesisMMO.Common.Codes;
    using System;
    using UnityEngine;

    public abstract class SkillCaller : MonoBehaviour {

        public event Action<ActionCode> SkillCalledEvent;

        protected void RaiseSkillCalledEvent(ActionCode skill) {
            if (SkillCalledEvent != null) {
                SkillCalledEvent(skill);
            }
        }
    }
}
