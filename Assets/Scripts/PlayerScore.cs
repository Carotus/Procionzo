using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScore : MonoBehaviour
{

    public int Score { get; set; }
    public int neededScore;

    public bool canExit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Score >= neededScore)
        {
          canExit = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering");
        if (canExit == true && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
}
