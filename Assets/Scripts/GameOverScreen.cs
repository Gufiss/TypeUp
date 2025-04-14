using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Heart_Manager heartManager;
    public AudioSource gameOverSound;
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI for displaying score

    private Game_Manager game_Manager;
    private bool gameOver = false;

    void Start()
    {
        game_Manager = GetComponent<Game_Manager>();
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!gameOver && heartManager.lives <= 0)
        {
            gameOver = true;
            gameOverPanel.SetActive(true);
            PlayGameOverSound();
            DisplayFinalScore(); // Display score when game over screen is shown
        }
    }

    public void ChangeSceneButton(int index)
    {
        int correctGuess = (game_Manager.saveSystem.LoadData("correct_guess") as int? ?? 0);
        int incorrectGuess = (game_Manager.saveSystem.LoadData("incorrect_guess") as int? ?? 0);
        int highscore = (game_Manager.saveSystem.LoadData("highscore") as int? ?? 0);
        int gameCount = (game_Manager.saveSystem.LoadData("gameCount") as int? ?? 0);
        float totalPlaytime = (game_Manager.saveSystem.LoadData("totalPlaytime") as float? ?? 0f);

        correctGuess += game_Manager.correct_guess;
        incorrectGuess += game_Manager.incorrect_guess;
        totalPlaytime += game_Manager.sessionPlaytime;
        gameCount++;

        if (highscore < game_Manager.score)
        {
            game_Manager.saveSystem.SaveData("highscore", game_Manager.score);
        }

        game_Manager.saveSystem.SaveData("correct_guess", correctGuess);
        game_Manager.saveSystem.SaveData("incorrect_guess", incorrectGuess);
        game_Manager.saveSystem.SaveData("gameCount", gameCount);
        game_Manager.saveSystem.SaveData("totalPlaytime", totalPlaytime);

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
}
