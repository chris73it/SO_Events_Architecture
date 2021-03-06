using UnityEngine;

public class RedCollision : MonoBehaviour
{
    [Tooltip("How much should the player's health decrease by when entering this trigger")]
    [SerializeField] int healthDecreaseAmount; //10

    [SerializeField] HealthManagerSO healthManagerSO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("RedCollision :: DecreaseHealth");
            healthManagerSO.DecreaseHealth(healthDecreaseAmount);
        }
    }
}
