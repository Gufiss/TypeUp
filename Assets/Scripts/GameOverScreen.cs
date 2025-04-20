using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOver;
    public Heart_Manager heartManager;
    public AudioSource gameOverSound;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI newHighScoreText; // Add this reference in the Inspector

    private Game_Manager game_Manager;
    private bool soundPlayed = false;

    void Start()
    {
        game_Manager = GetComponent<Game_Manager>();
        gameOver.SetActive(false);
        Time.timeScale = 1f;

        if (newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(false); // Hide it initially
        }
    }

    void Update()
    {
        if (!soundPlayed && heartManager.lives <= 0)
        {
            soundPlayed = true;
            gameOver.SetActive(true);
            PlayGameOverSound();
            DisplayFinalScore();
            CheckHighScore(); // Check if new high score was reached
        }
    }

    public void ChangeSceneButton(int index)
    {
        int correctGuess = (game_Manager.saveSystem.LoadData("correct_guess") as int? ?? 0);
        int incorrectGuess = (game_Manager.saveSystem.LoadData("incorrect_guess") as int? ?? 0);
        int highscore = (game_Manager.saveSystem.LoadData("highscore") as int? ?? 0);
        int gameCount = (game_Manager.saveSystem.LoadData("gameCount") as int? ?? 0);

        correctGuess += game_Manager.correct_guess;
        incorrectGuess += game_Manager.incorrect_guess;
        gameCount++;

        if (highscore < game_Manager.score)
        {
            game_Manager.saveSystem.SaveData("highscore", game_Manager.score);
        }

        game_Manager.saveSystem.SaveData("correct_guess", correctGuess);
        game_Manager.saveSystem.SaveData("incorrect_guess", incorrectGuess);
        game_Manager.saveSystem.SaveData("gameCount", gameCount);

        SceneManager.LoadScene(index);
    }

    void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            gameOverSound.Play();
        }
    }

    void DisplayFinalScore()
    {
        scoreText.text = game_Manager.scoreText.text;
    }

    void CheckHighScore()
    {
        int highscore = (game_Manager.saveSystem.LoadData("highscore") as int? ?? 0);
        if (game_Manager.score >= highscore && newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(true);
        }
    }
}
