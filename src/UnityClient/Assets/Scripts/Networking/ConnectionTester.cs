using UnityEngine;

public class ConnectionTester : MonoBehaviour {

    private void Start() {
        ChatClient.Start();
    }

    private void OnDestroy() {
        ChatClient.Stop();
    }
}
