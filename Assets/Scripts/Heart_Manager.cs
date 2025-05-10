using UnityEngine;
using UnityEngine.UI;

public class Heart_Manager : MonoBehaviour
{
    public int lives = 3;

    public Sprite heart_on; 
    public Sprite heart_off;

    public Image h1;
    public Image h2; 
    public Image h3;

    //Audio
    public AudioSource loseLife;

    void Start()
    {
        UpdateHearts();

        //Audio
        loseLife = gameObject.AddComponent<AudioSource>();
        loseLife.volume = 0.01f;
        loseLife.clip = Resources.Load<AudioClip>("BuzzerLost");
        loseLife.loop = false;
    }

    public void LoseLife()
    {
        if (lives > 0)
        {
            //Audio
            loseLife.Play();

            lives--;
            UpdateHearts();
        }
    }

    public void GainLife()
    {
        if (lives < 3)
        {
            lives++;
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        h1.sprite = lives >= 1 ? heart_on : heart_off;
        h2.sprite = lives >= 2 ? heart_on : heart_off;
        h3.sprite = lives >= 3 ? heart_on : heart_off;
    }
}
