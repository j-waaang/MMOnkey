using UnityEngine;

public class RegionHintCreator : MonoBehaviour {

    [SerializeField] private GameObject m_HintPrefab;
    private static readonly Vector3 m_Start = new Vector3(-45F, 0.5F, -45F);
    private const float Offset = 10F;
    private const int Dimension = 10;

	private void Awake () {
		for(var x = 0; x < Offset; ++x) {
            for(var z = 0; z < Offset; z++) {
                var pos = m_Start + Vector3.right * x * Offset + Vector3.forward * z * Offset;
                Instantiate(m_HintPrefab, transform).transform.position = pos;
            }
        }
	}
}
