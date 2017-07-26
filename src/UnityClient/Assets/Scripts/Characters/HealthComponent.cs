using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using System;
using System.ComponentModel;
using UnityEngine;

public class HealthComponent : MonoBehaviour, INotifyPropertyChanged {

    // TODO: Use PropertyChanged event to notify about new health.

    public delegate void HealthUpdate(int damage, int health, int maxHealth);
    public event HealthUpdate HealthUpdatedEvent;
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<int> DamageTakenEvent;

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
        Health = newHealth;

        if (HealthUpdatedEvent != null) {
            HealthUpdatedEvent(damage, Health, MaxHealth);
        }
        if(DamageTakenEvent != null) {
            DamageTakenEvent(damage);
        }
    }

    private void OnDeath() {
        EventOperations.HealthUpdatedEvent -= HealthChangedEvent;
    }

    private void OnDestroy() {
        EventOperations.HealthUpdatedEvent -= HealthChangedEvent;
    }
}
