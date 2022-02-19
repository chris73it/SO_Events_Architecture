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
 */

[CreateAssetMenu(fileName = "HealthManagerSO", menuName = "Scriptable Objeects/Health Manager")]
public class HealthManagerSO : ScriptableObject
{
    public int Health
    {
        get; private set;
    }

    [Range(1, 100), Tooltip("Max value of the player's lifeforce (must be greater than zero)")]
    public int maxHealth; //100

    //Objects subscribe to this event to be notified of health changes.
    UnityEvent<int> healthChangedEvent;

    //Awake is only called when a SO is created, or loaded, or selected (and Awake had not been called yet.)

    void OnEnable()
    {
        Health = maxHealth;

        if (healthChangedEvent == null)
        {
            healthChangedEvent = new UnityEvent<int>();
        }
    }

    /*
     * APIs to interact with the SO.
     */

    public void SubscribeToHealthChangedEvent(UnityAction<int> fn)
    {
        healthChangedEvent.AddListener(fn);
    }
    public void UnsubscribeToHealthChangedEvent(UnityAction<int> fn)
    {
        healthChangedEvent.RemoveListener(fn);
    }

    public void DecreaseHealth(int amount)
    {
        Health -= amount;
        if (Health < 0)
        {
            Health = 0;
        }
        Debug.Log("HealthManagerSO :: DecreaseHealth");
        healthChangedEvent.Invoke(Health);
    }

    public void IncreaseHealth(int amount)
    {
        Health += amount;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
        Debug.Log("HealthManagerSO :: IncreaseHealth");
        healthChangedEvent.Invoke(Health);
    }
}
