using UnityEngine;

public class GreenCollision : MonoBehaviour
{
    [Tooltip("How much should the player's health increase by when entering this trigger")]
    [SerializeField] int healthIncreaseAmount; //10

    [SerializeField] HealthManagerSO healthManagerSO;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("GreenCollision :: OnTriggerEnter other");
        if (other.CompareTag("Player"))
        {
            Debug.Log("GreenCollision :: OnTriggerEnter Player");
            healthManagerSO.IncreaseHealth(healthIncreaseAmount);
        }
    }
}
