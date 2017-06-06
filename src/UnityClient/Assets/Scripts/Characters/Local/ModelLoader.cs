namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Local {

    using UnityEngine;

    public class ModelLoader : MonoBehaviour {

        [SerializeField]
        GameObject m_Ork;
        private Vector3 m_DefaultPosition = new Vector3(0, 0, -0.8f);
        private Quaternion m_DefaultRotation = Quaternion.Euler(new Vector3(-90f, 0, 0));

        private void Awake() {
            var go = Instantiate(m_Ork, transform);
            go.transform.position += m_DefaultPosition;
            go.transform.rotation = m_DefaultRotation;
        }
    }
}
