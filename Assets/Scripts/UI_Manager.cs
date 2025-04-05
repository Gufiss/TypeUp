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

    public Button musicButton;
    public Button sfxButton;


    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float volume = PlayerPrefs.GetFloat("MusicVolume");
            musicSource.volume = volume;
            musicSlider.value = volume;
        }

        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            float volume = PlayerPrefs.GetFloat("SfxVolume");
            sfxSource.volume = volume;
            sfxSlider.value = volume;
        }

        if (PlayerPrefs.HasKey("MusicEnabled"))
        {
            int musicEnabled = PlayerPrefs.GetInt("MusicEnabled");
            music_toggle = (musicEnabled == 1);
            musicSource.enabled = music_toggle;
            musicButton.image.sprite = music_toggle ? music_on : music_off;
        }

        if (PlayerPrefs.HasKey("SfxEnabled"))
        {
            int musicEnabled = PlayerPrefs.GetInt("SfxEnabled");
            sfx_toggle = (musicEnabled == 1);
            sfxSource.enabled = sfx_toggle;
            sfxButton.image.sprite = sfx_toggle ? sfx_on : sfx_off;
        }
    }

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
            PlayerPrefs.SetInt("MusicEnabled", 1); 
        }
        else
        {
            obj.image.sprite = music_off;
            PlayerPrefs.SetInt("MusicEnabled", 0); 
        }

        PlayerPrefs.Save(); 
    }


    public void MuteSfx(Button obj)
    {
        sfx_toggle = !sfx_toggle;
        sfxSource.enabled = sfx_toggle;
        if (sfx_toggle)
        {
            obj.image.sprite = sfx_on;
            PlayerPrefs.SetInt("SfxEnabled", 1);
        }
        else
        {
            obj.image.sprite = sfx_off;
            PlayerPrefs.SetInt("SfxEnabled", 0);
        }

        PlayerPrefs.Save();
    }

    public void PlayEffect(AudioClip audioclip)
    {
        sfxSource.clip = audioclip;
        sfxSource.Play();
    }

    public void ChangeMusicVolume()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
    }

    public void ChangeSfxVolume()
    {
        sfxSource.volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
