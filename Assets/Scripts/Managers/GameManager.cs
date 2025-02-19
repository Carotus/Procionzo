using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverUI;

    public GameObject pauseUI;

    [Header("Score")]
    public int CurrentScore;

    public int NeededScore;

    public int TotalScore;

   private  bool paused;

    private  DoorExit door;

    void Start()
    {
        Time.timeScale = 1;
        door = FindAnyObjectByType<DoorExit>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !paused)
        {
            Debug.Log("Pause");
            pauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            paused = true;
        }
        else if (Input.GetButtonDown("Pause") && paused)
        {
            Debug.Log("Unpause");
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            paused = false;
        }
        
        if (CurrentScore >= NeededScore)
        {
            door.canExit = true;
        }

        if (CurrentScore == TotalScore)
        {
            Debug.Log("EXTRA STAMINA UNLOCKED");
        }
    }

    public void LoseScreen()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        gameOverUI.SetActive(true);

    }

}
