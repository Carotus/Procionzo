using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject canvas;
    public playerScore PlayerScore;

    public void Resume()
    {
        PlayerScore.hasPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        canvas.SetActive(false);
    }

    public void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("restart");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Debug.Log("quit");
    }
}
