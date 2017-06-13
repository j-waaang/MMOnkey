using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour {

    public event Action<int, int, int> DamageHealthMaxHealthUpdatedEvent;

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public void Initialize(int health, int maxHealth) {
        Health = health;
        MaxHealth = maxHealth;
        EventOperations.HealthUpdateEvent += OnHealthUpdateEvent;
    }

    private void OnHealthUpdateEvent(string name, int damage, int newHealth) {
        if (gameObject.name != name) { return; }

        Debug.LogFormat("Update {0}'s health to {1}. Damage done {2}", name, newHealth, damage);

        Health = newHealth;
        if (DamageHealthMaxHealthUpdatedEvent != null) {
            DamageHealthMaxHealthUpdatedEvent(damage, Health, MaxHealth);
        }
    }

    private void OnDestroy() {
        EventOperations.HealthUpdateEvent -= OnHealthUpdateEvent;
    }
}
