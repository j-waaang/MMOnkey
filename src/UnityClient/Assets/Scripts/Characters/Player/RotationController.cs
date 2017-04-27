namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Player {
    using UnityEngine;
    using UnityStandardAssets.CrossPlatformInput;
    public class RotationController : MonoBehaviour {
        private Rigidbody2D m_Rigidbody;

        private void Awake() {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        //private void FixedUpdate() {
        //    var inputVector = new Vector3(CrossPlatformInputManager.GetAxis("LookHorizontal"), CrossPlatformInputManager.GetAxis("LookVertical"), 0);
        //    if (inputVector != Vector3.zero) {
        //        transform.up = inputVector;
        //    }
        //}
    }
}
