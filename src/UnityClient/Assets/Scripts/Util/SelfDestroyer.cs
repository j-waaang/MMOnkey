using UnityEngine;

public class SelfDestroyer : MonoBehaviour {

    [SerializeField] private float destroyInSeconds = 1f;

    private void Start() {
        StartCoroutine(ActionHelper.WaitAndDo(destroyInSeconds, () => Destroy(gameObject)));
    }
}
