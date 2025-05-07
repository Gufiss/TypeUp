using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool Paused = false;
    public GameObject pauseMenu;
    public TextMeshProUGUI countdown;

    private Game_Manager game_Manager;

    void Start()
    {
        game_Manager = GetComponent<Game_Manager>();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    public void Stop()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Play()
    {
        pauseMenu.SetActive(false);
        StartCoroutine(ResumeWithCountdown());
    }

    private IEnumerator ResumeWithCountdown()
    {
        countdown.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        countdown.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        countdown.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        countdown.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);
        countdown.text = "";

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        Paused = false;
    }

    public void MainMenuButton()
    {
        int correctGuess = (game_Manager.saveSystem.LoadData("correct_guess") as int? ?? 0);
        int incorrectGuess = (game_Manager.saveSystem.LoadData("incorrect_guess") as int? ?? 0);
        float totalPlaytime = (game_Manager.saveSystem.LoadData("totalPlaytime") as float? ?? 0f);

        correctGuess += game_Manager.correct_guess;
        incorrectGuess += game_Manager.incorrect_guess;
        totalPlaytime += game_Manager.sessionPlaytime;

        game_Manager.saveSystem.SaveData("correct_guess", correctGuess);
        game_Manager.saveSystem.SaveData("incorrect_guess", incorrectGuess);
        game_Manager.saveSystem.SaveData("totalPlaytime", totalPlaytime);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
