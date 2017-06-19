using System;
using UnityEngine;
using JYW.ThesisMMO.Common.ContinueObjects;

public class FireStorm : MonoBehaviour {

    private const int FireStormLifeTime = 5;
    private ActionContinueCondition m_SelfDestroyTrigger;

    private bool m_SelfDestroy = false;

    private void Awake() {
        m_SelfDestroyTrigger = new TimedContinueCondition(new TimeSpan(0, 0, FireStormLifeTime));
        m_SelfDestroyTrigger.ContinueEvent += SelfDestroy;
        m_SelfDestroyTrigger.Start();
    }

    private void Update() {
        if (m_SelfDestroy) {
            Destroy(gameObject);
            return;
        }
    }

    private void SelfDestroy(CallReason r) {
        m_SelfDestroy = true;
    }
}
