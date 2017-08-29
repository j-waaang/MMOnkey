namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Characters.Local {
    using UnityClient.Characters;
    using UnityEngine;

    public class ModelLoader : MonoBehaviour {

        [SerializeField] GameObject m_Prefab;

        private void Awake() {
            var go = Instantiate(m_Prefab, transform);
        }
    }
}
