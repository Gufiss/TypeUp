using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool Paused = false;
    public GameObject pauseMenu;

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
