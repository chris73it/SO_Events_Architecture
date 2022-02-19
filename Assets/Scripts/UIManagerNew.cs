using UnityEngine;
using UnityEngine.UI;

public class UIManagerNew : MonoBehaviour
{
    //See https://forum.unity.com/threads/how-to-change-the-colour-of-a-slider-based-on-its-current-value.363686/
    [SerializeField] Slider slider;
    [SerializeField] HealthManagerSO healthManagerSO;

    void Start()
    {
        Debug.Log("UIManagerNew :: Start");

        //HealthManagerSO::OnEnable is expected to initialize healthManagerSO.Health,
        //  and because it is executed before this Start function,
        //  we know that by now we can use healthManagerSO.Health to initialize the slider position.
        ChangeSliderValue(healthManagerSO.Health);
    }

    void ChangeSliderValue(int amount)
    {
        slider.value = ConvertIntToFloatDecimal(amount);
    }

    float ConvertIntToFloatDecimal(int amount)
    {
        return (float)amount / healthManagerSO.maxHealth;
    }

    void OnEnable()
    {
        Debug.Log("UIManagerNew :: OnEnable");
        healthManagerSO.SubscribeToHealthChangedEvent(ChangeSliderValue);
    }

    void OnDisable()
    {
        Debug.Log("UIManagerNew :: OnDisable");
        healthManagerSO.UnsubscribeToHealthChangedEvent(ChangeSliderValue);
    }
}
