using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOver;
    public Heart_Manager heartManager;
    public AudioSource gameOverSound;
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI for displaying score

    private bool soundPlayed = false;

    void Start()
    {
        gameOver.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (heartManager.lives <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0f;
            PlayGameOverSound();
            DisplayFinalScore(); // Display score when game over screen is shown
        }
    }

    public void TakeDamage()
    {
        heartManager.LoseLife();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void PlayGameOverSound()
    {
        if (!soundPlayed && gameOverSound != null)
        {
            gameOverSound.Play();
            soundPlayed = true;
        }
    }

    void DisplayFinalScore()
    {
        // Access the score from ScoreManager and display it on the game over screen
        scoreText.text = "Score: " + ScoreManager.instance.GetScore().ToString();
    }
}
