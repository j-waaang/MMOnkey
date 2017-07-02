using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;

public class HealthComponent : MonoBehaviour {

    public delegate void HealthUpdate(int damage, int health, int maxHealth);
    public event HealthUpdate HealthUpdatedEvent;

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    private void Awake() {
        Health = 100;
        MaxHealth = 100;
        EventOperations.HealthUpdatedEvent += HealthChangedEvent;
    }

    public void Initialize(int health, int maxHealth) {
        Health = health;
        MaxHealth = maxHealth;
    }

    private void HealthChangedEvent(string name, int damage, int newHealth) {
        if (gameObject.name != name) { return; }
        if (HealthUpdatedEvent == null) { return; }

        Health = newHealth;
        HealthUpdatedEvent(damage, Health, MaxHealth);
    }

    private void OnDestroy() {
        EventOperations.HealthUpdatedEvent -= HealthChangedEvent;
    }

    public void OnDeath() {
        EventOperations.HealthUpdatedEvent -= HealthChangedEvent;
    }
}
