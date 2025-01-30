using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScore : MonoBehaviour
{
    public int playerScoreCap;
    public int currentPlayerScore;
    public GameObject pauseCanvas;
    public GameObject winCanvas;
    public bool hasPaused;

    void Update()
    {
        if(hasPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if(Input.GetButtonDown("Pause"))
        {
            pauseCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            hasPaused = true;
                   
        }

        if (currentPlayerScore >= playerScoreCap)
        {
            Cursor.lockState = CursorLockMode.None;
            winCanvas.SetActive(true);
            hasPaused = true;
            Debug.Log("You win");
            
        }
    }
}
