using UnityEngine;
using UnityEngine.UI;
public class SatBar : MonoBehaviour
{

    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetMaxSat(float sat)
    {
        slider.maxValue = sat;
        slider.value = sat;
    }

    public void SetCurrentSat(float sat)
    {
        slider.value = sat;
    }
}
