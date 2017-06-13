namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Local {

    using UnityEngine;

    public class ModelLoader : MonoBehaviour {

        [SerializeField] GameObject m_Prefab;

        private void Awake() {
            Instantiate(m_Prefab, transform);
        }
    }
}
