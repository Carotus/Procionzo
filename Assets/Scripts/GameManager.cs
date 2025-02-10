using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverUI;

    public GameObject pauseUI;
    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("Pause");
            pauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void LoseScreen()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        gameOverUI.SetActive(true);

    }

}
