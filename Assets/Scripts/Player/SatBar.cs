using UnityEngine;
using UnityEngine.UI;
public class SatBar : MonoBehaviour
{

    public Slider slider;
    

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
