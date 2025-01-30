using UnityEngine;
using UnityEngine.UI;
public class staminaBar : MonoBehaviour
{

    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }
}
