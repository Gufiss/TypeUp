using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOver;
    public Heart_Manager heartManager; // Reference to Heart_Manager

    void Start()
    {
        gameOver.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (heartManager.lives <= 0) // Use heartManager's lives count
        {
            gameOver.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void TakeDamage()
    {
        heartManager.LoseLife(); // Reduce lives through Heart_Manager
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
