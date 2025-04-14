using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class StatisticsPage : MonoBehaviour
{
    public SaveSystem saveSystem;

    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI highscore;
    public TextMeshProUGUI gameCount;
    public TextMeshProUGUI totalPlaytime;

    void Start()
    {
        accuracy.text = "Typing accuracy: " + calculateAcc().ToString("F2") + "%";
        highscore.text = "Highscore: " + (saveSystem.LoadData("highscore") as int? ?? 0).ToString();
        gameCount.text = "Games played: " + (saveSystem.LoadData("gameCount") as int? ?? 0).ToString();
        totalPlaytime.text = "Time spent writing: " + calculateHours().ToString("F2") + " hours";
    }

    float calculateAcc()
    {
        int correct = saveSystem.LoadData("correct_guess") as int? ?? 0;
        int incorrect = saveSystem.LoadData("incorrect_guess") as int? ?? 0;

        if ((correct + incorrect) == 0)
        {
            return 0;
        }
        else return (float)correct / (correct + incorrect) * 100;
    }

    float calculateHours()
    {
        return (saveSystem.LoadData("totalPlaytime") as float? ?? 0f) / 3600f;
    }
}
