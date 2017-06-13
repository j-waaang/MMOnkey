using UnityEngine;

public class MovementSpeedComponent : MonoBehaviour {
    
    public float MovementSpeed { get; protected set; }

    private const float InitialSpeed = 0.2f;

    private void Awake() {
        MovementSpeed = InitialSpeed;
    }
}
