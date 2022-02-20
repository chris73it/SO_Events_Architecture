using UnityEngine;

public class GreenCollision : MonoBehaviour
{
    [Tooltip("How much should the player's health increase by when entering this trigger")]
    [SerializeField] int healthIncreaseAmount; //10

    [SerializeField] HealthManagerSO healthManagerSO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("GreenCollision :: IncreaseHealth");
            healthManagerSO.IncreaseHealth(healthIncreaseAmount);
        }
    }
}
