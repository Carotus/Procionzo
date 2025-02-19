using UnityEngine;
using UnityEngine.UI;

public class SpottedBar : MonoBehaviour
{
    public Slider slider;

   
    public void SetMaxSpot(float spot)
    {
        slider.maxValue = spot;
        slider.value = spot;
    }

    public void SetCurrentSpot(float spot)
    {
        slider.value = spot;
    }
}
