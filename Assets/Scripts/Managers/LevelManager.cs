using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public int level;

    public GameObject LSS = null;

    public GameObject thisPanel = null;

    void Awake()
    {
        if (LSS == null)
        {
            LSS = GameObject.Find("PlayerCanvas");
        }

        if (thisPanel == null)
        {
            thisPanel = GameObject.Find("PauseMenu");
        }

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        thisPanel.SetActive(false);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(level);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenLSS()
    {
        LSS.SetActive(true);
        thisPanel.SetActive(false);
    }
}
