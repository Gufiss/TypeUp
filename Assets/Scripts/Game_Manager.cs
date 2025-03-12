using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Game_Manager : MonoBehaviour
{
    float spawn_timer = 0;
    int lastIndex = -1;
    List<string> wordList = new List<string>
{
    "adventure", "mystery", "quicksilver", "tapestry", "illusion",
    "echo", "voyage", "horizon", "enigma", "cryptic",
    "labyrinth", "alchemy", "arcane", "serenity", "eclipse",
    "phoenix", "stardust", "temporal", "astronaut", "paradox"
};

    List<(Vector2 start, Vector2 end)> locations = new List<(Vector2, Vector2)>
{
    (new Vector2(10, 2.5f), new Vector2(-10, 2.5f)),
    (new Vector2(-10, 1.5f), new Vector2(10, 1.5f))
};

    [SerializeField] List<GameObject> activePackages = new List<GameObject>();

    public TMP_InputField typing_field;
    public GameObject package;

    void Start()
    {
        Invoke(nameof(FocusInputField), 0.1f);
        GenerateWord();
    }

    void Update()
    {
        spawn_timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i < activePackages.Count; i++)
            {
                GameObject package = activePackages[i];
                if (package.GetComponent<Package>().toType.Trim().ToLower() == typing_field.text.Trim().ToLower())
                {
                    typing_field.text = string.Empty;
                    activePackages.RemoveAt(i);
                    Destroy(package);
                    break;
                }
            }
            FocusInputField();
        }

        if (spawn_timer <= 0)
        {
            spawn_timer = 3;
            int spawnIndex = Random.Range(0, locations.Count);

            GameObject newBox = Instantiate(package, (Vector3)locations[spawnIndex].start, Quaternion.identity);
            Package packageScript = newBox.GetComponent<Package>();

            packageScript.speed = 1.0f;
            packageScript.endLoc = locations[spawnIndex].end;
            packageScript.toType = GenerateWord();

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
