namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Cameras {
    using UnityEngine;
    /// <summary>
    // Follows the a target. Usually a player.
    // Angle and offset are currently hardcoded.
    /// </summary>
    internal class CameraController : MonoBehaviour {

        [SerializeField] private float m_InitViewAngle = 30;
        [SerializeField] private float m_InitCameraHeight = 10;

        private float m_ViewAngle;
        private float m_CameraHeight;

        [SerializeField] private Transform m_Target = null;
        private readonly Vector3 m_CameraToTargetOffset = new Vector3(0, -5f, -10f);

        private void Awake() {
            if (m_Target == null) {
                m_Target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        private void Update() {
            transform.position = m_Target.position + m_CameraToTargetOffset;
        }
    }
}
