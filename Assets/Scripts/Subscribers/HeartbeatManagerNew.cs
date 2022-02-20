using UnityEngine;

public class HeartbeatManagerNew : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] HealthManagerSO healthManagerSO;

    void UpdateHeartbeatSound()
    {
        if (healthManagerSO.Health <= healthManagerSO.HeartbeatThreshold)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    void OnEnable()
    {
        Debug.Log("HeartbeatManagerNew :: OnEnable");
        healthManagerSO.SubscribeToLowHeartbeatEvent(UpdateHeartbeatSound);
    }

    void OnDisable()
    {
        Debug.Log("HeartbeatManagerNew :: OnDisable");
        healthManagerSO.UnsubscribeToLowHeartbeatEvent(UpdateHeartbeatSound);
    }
}
