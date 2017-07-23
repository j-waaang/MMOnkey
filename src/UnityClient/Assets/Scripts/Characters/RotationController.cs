using System;
using UnityEngine;

/// <summary>  
///  Lets the character look at a direction.
///  Also provides methods to lock/unlock rotations.
/// </summary>  
public class RotationController : MonoBehaviour {

    private bool m_LockedRotation = false;
    private float m_LockEnd;

    public void LookAt(Vector3 forwardVector, TimeSpan duration) {
        LookAt(forwardVector, ((float)duration.TotalMilliseconds) * 0.001f);
    }

    /// <summary>  
    ///  Lets the character look at a direction and locks that rotation for a durtaion.
    ///  A new lock request overrides the old lock request.
    /// </summary>  
    public void LookAt(Vector3 forwardVector, float duration) {
        transform.forward = forwardVector;
        m_LockEnd = Time.time + duration;
        m_LockedRotation = true;
    }

    /// <summary>  
    ///  Lets the character look at a direction.
    /// </summary>  
    public void LookAt(Vector3 lookDirection) {
        if (lookDirection == Vector3.zero) { return; }

        if (m_LockedRotation) {
            if(m_LockEnd > Time.time) { return; }
            m_LockedRotation = false;
        }
        transform.forward = lookDirection;
    }

    /// <summary>  
    ///  Lets the character look at a point and locks that rotation for a durtaion.
    ///  A new lock request overrides the old lock request.
    /// </summary>  
    public void LookAtPoint(Vector3 point, float duration) {
        LookAt(point - transform.position, duration);
    }

    /// <summary>  
    ///  Lets the character look at a point.
    /// </summary>  
    public void LookAtPoint(Vector3 point) {
        LookAt(point - transform.position);
    }

    /// <summary>  
    ///  Suspend the rotation lock.
    /// </summary>  
    public void InteruptLookLock() {
        m_LockedRotation = false;
    }
}
