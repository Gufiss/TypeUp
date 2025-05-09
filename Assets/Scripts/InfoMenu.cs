using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InfoMenu : MonoBehaviour
{
    public bool Paused = false;
    public GameObject infoMenu;
    public TextMeshProUGUI countdown;

    private Game_Manager game_Manager;

    void Start()
    {
        game_Manager = GetComponent<Game_Manager>();
        infoMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        if (Paused)
        {
            StartCoroutine(ResumeWithCountdown());
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        infoMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Play()
    {
        infoMenu.SetActive(false);
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
}
