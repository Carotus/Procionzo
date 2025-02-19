using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorExit : MonoBehaviour
{
    public bool canExit;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering");
        if (canExit == true && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
}
