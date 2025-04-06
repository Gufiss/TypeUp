using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    bool music_toggle = true;
    bool sfx_toggle = true;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public Sprite sfx_off;
    public Sprite sfx_on;
    public Sprite music_off;
    public Sprite music_on;

    public Slider musicSlider;
    public Slider sfxSlider;

    public void ChangeScene(int sceneIndex = 0)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Invalid scene index!");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Application.Quit()");
    }

    public void MuteMusic(Button obj)
    {
        music_toggle = !music_toggle;
        musicSource.enabled = music_toggle;
        if (music_toggle)
        {
            obj.image.sprite = music_on;
        }
        else
        {
            obj.image.sprite = music_off;
        }
    }

    public void MuteSfx(Button obj)
    {
        sfx_toggle = !sfx_toggle;
        sfxSource.enabled = sfx_toggle;
        if (sfx_toggle)
        {
            obj.image.sprite = sfx_on;
        }
        else
        {
            obj.image.sprite = sfx_off;
        }
    }

    public void PlayEffect(AudioClip audioclip)
    {
        sfxSource.clip = audioclip;
        sfxSource.Play();
    }

    public void ChangeMusicVolume()
    {
        musicSource.volume = musicSlider.value;
    }

    public void ChangeSFXVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
