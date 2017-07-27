namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Cameras {

    using UnityEngine;

    /// <summary>
    // Follows the a target. Usually a player.
    // Angle and offset are currently hardcoded.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    internal class CameraController : MonoBehaviour {

        [SerializeField] private Transform m_Target = null;
        [SerializeField] private Vector3 m_CameraToTargetOffset = new Vector3(0, -5f, -10f);
        private Camera m_Camera;

        private void Awake() {
            if (m_Target == null) {
                m_Target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            m_Camera = GetComponent<Camera>();
        }

        private void Update() {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            m_Camera.orthographicSize -= scroll;
            transform.position = m_Target.position + m_CameraToTargetOffset;
        }
    }
}
