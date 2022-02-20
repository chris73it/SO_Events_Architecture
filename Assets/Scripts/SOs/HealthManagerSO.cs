using UnityEngine;
using UnityEngine.Events;

/*
 * The SO:
 * A. encapsulates the Health property, and
 * B. exposes it via its public APIs, and
 * C. propagates it through the healthChangedEvent event.
 *
 * There are 2 types of actors involved in the interaction with the health property:
 * 1. The subscribers, like the UIManager, that want to be notified when the avatar's health changes.
 * 2. The users, like the red and green zones, that execute APIs like DecreaseHealth and IncreaseHealth,
 *    that result in the invocation of the encapsulated healthChangedEvent event,
 *    that allows the subscribers to take care of their own businesses, like updating the position of the health slider.
 *
 * NOTE: Users own their resources: for instance, the UIManager owns the slider and the HeartbeatManager owns the heartbeat sound.
 */

[CreateAssetMenu(fileName = "HealthManagerSO", menuName = "Scriptable Objects/Health Manager")]
public class HealthManagerSO : ScriptableObject
{
    public int Health
    {
        get; private set;
    }

    [Tooltip("Max value of the player's lifeforce (must be greater than zero)")]
    [SerializeField, Range(1, 100)] int maxHealth; //100
    public int MaxHealth
    {
        get { return maxHealth; }
    }

    [Tooltip("Heartbeat threshold value of the player's lifeforce (must be greater than zero)")]
    [SerializeField, Range(1, 100)] int heartbeatThreshold; //25
    public int HeartbeatThreshold
    {
        get { return heartbeatThreshold; }
    }

    //Objects subscribe to this event to be notified of health changes.
    UnityEvent<int> healthChangedEvent;

    //Objects subscribe to this event to be notified of low heartbeat.
    UnityEvent hearetbeatThresholdCrossedEvent;

    //Awake is only called when a SO is created, or loaded, or selected (and Awake had not been called yet.)

    void OnEnable()
    {
        Health = maxHealth;

        if (healthChangedEvent == null)
        {
            healthChangedEvent = new UnityEvent<int>();
        }

        if (hearetbeatThresholdCrossedEvent == null)
        {
            hearetbeatThresholdCrossedEvent = new UnityEvent();
        }
    }

    /*
     * Health related APIs:
     */

    public void DecreaseHealth(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("HealthManagerSO :: DecreaseHealth (amount <= 0)");
            return;
        }

        int tmpHealth = Health;
        Health -= amount;
        if (Health < 0)
        {
            Health = 0;
        }
        if (tmpHealth > 0)
        {
            Debug.Log("HealthManagerSO :: DecreaseHealth (Health decreased)");
            healthChangedEvent.Invoke(Health);
        }

        if (Health <= heartbeatThreshold && Health + amount > heartbeatThreshold)
        {
            Debug.Log("HealthManagerSO :: DecreaseHealth (heartbeat crossed below threashold)");
            hearetbeatThresholdCrossedEvent.Invoke();
        }
    }

    public void IncreaseHealth(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("HealthManagerSO :: IncreaseHealth (amount <= 0)");
            return;
        }

        int tmpHealth = Health;
        Health += amount;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
        if (tmpHealth < maxHealth)
        {
            Debug.Log("HealthManagerSO :: IncreaseHealth (Health increased)");
            healthChangedEvent.Invoke(Health);
        }

        if (Health > heartbeatThreshold && Health - amount <= heartbeatThreshold)
        {
            Debug.Log("HealthManagerSO :: IncreaseHealth (heartbeat crossed above threashold)");
            hearetbeatThresholdCrossedEvent.Invoke();
        }
    }

    //Subscribe/Unsubscribe for Health Changed Event:
    public void SubscribeToHealthChangedEvent(UnityAction<int> fn)
    {
        healthChangedEvent.AddListener(fn);
    }
    public void UnsubscribeToHealthChangedEvent(UnityAction<int> fn)
    {
        healthChangedEvent.RemoveListener(fn);
    }

    //Subscribe/Unsubscribe for Low Heartbeat Event:
    public void SubscribeToLowHeartbeatEvent(UnityAction fn)
    {
        hearetbeatThresholdCrossedEvent.AddListener(fn);
    }
    public void UnsubscribeToLowHeartbeatEvent(UnityAction fn)
    {
        hearetbeatThresholdCrossedEvent.RemoveListener(fn);
    }
}
