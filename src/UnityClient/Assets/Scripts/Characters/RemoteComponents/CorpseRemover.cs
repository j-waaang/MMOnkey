using UnityEngine;
using System.Collections;

public class CorpseRemover : MonoBehaviour {

    private const float DeathDelay = 10F;

    private void OnDeath() {
        StartCoroutine(DeplayedSelfDestruction());
    }

    private IEnumerator DeplayedSelfDestruction() {
        yield return new WaitForSeconds(DeathDelay);
        Destroy(gameObject);
    }
}
