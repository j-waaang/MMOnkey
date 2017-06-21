using UnityEngine;

public class MovementSpeedComponent : MonoBehaviour {
    
    public float MovementSpeed { get; protected set; }

    private const float InitialSpeed = 7f;

    private void Awake() {
        MovementSpeed = InitialSpeed;
    }
}
