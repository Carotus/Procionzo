using UnityEngine;

public class playerInteraction : MonoBehaviour
{

    private float currentStamina;

    public bool canRegenStamina;
    public float maxStamina;

    public staminaBar staminabar;

    public GameObject canvas;



    void Start()
    {
        currentStamina = maxStamina;
        staminabar.SetMaxStamina((float)maxStamina);
    }

    void Update()
    {
        if(currentStamina >= 0)
        {
            currentStamina -= Time.deltaTime;
            staminabar.SetStamina((float)currentStamina);
        }
    }
}
