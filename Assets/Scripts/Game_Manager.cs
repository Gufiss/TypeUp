using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    int lastIndex = -1;
    float timer = 0;
    List<string> wordList = new List<string> { "apple", "banana", "orange", "grape", "peach" };
    List<(Vector2 start, Vector2 end)> locations = new List<(Vector2, Vector2)>
{
    (new Vector2(10, 2.5f), new Vector2(-10, 2.5f)),
    (new Vector2(-10, 1.5f), new Vector2(10, 1.5f))
};

    [SerializeField]List<GameObject> activePackages = new List<GameObject>();

    public TMP_InputField typing_field;
    public GameObject package;

    void Start()
    {
        Invoke(nameof(FocusInputField), 0.1f);
        GenerateWord();
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        if (timer <= 0)
        {
            timer = 3;
            int spawnIndex = Random.Range(0, locations.Count);

            GameObject newBox = Instantiate(package, (Vector3)locations[spawnIndex].start, Quaternion.identity);
            Package packageScript = newBox.GetComponent<Package>();

            packageScript.speed = 1.0f;
            packageScript.endLoc = locations[spawnIndex].end;

            activePackages.Add(newBox);
        }
    }

    public void FocusInputField()
    {
        typing_field.Select();
        typing_field.ActivateInputField();
    }

    string GenerateWord()
    {
        if (wordList.Count == 0)
        {
            Debug.LogError("Word list is empty!");
            return "null";
        }

        if (wordList.Count == 1)
        {
            return wordList[0];
        }

        int index;
        do
        {
            index = Random.Range(0, wordList.Count);
        } while (index == lastIndex);

        lastIndex = index;
        return wordList[index];
    }
}
