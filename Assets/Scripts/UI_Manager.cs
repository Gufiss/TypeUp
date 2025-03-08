using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
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
}
