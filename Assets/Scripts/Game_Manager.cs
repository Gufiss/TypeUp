using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] List<GameObject> activePackages = new List<GameObject>();

    float current_spawn_timer = 0;
    [SerializeField] float next_spawn_timer = 3;

    int lastIndex = -1;
    int lastSpawnIndex = -1; 

    List<string> wordList = new List<string>
    {
        "adventure", "mystery", "quicksilver", "tapestry", "illusion",
        "echo", "voyage", "horizon", "enigma", "cryptic",
        "labyrinth", "alchemy", "arcane", "serenity", "eclipse",
        "phoenix", "stardust", "temporal", "astronaut", "paradox"
    };
    List<(Vector2 start, Vector2 end)> locations = new List<(Vector2, Vector2)>
    {
        (new Vector2(10, 3.5f), new Vector2(-10, 3.5f)),
        (new Vector2(10, 2.5f), new Vector2(-10, 2.5f)),
        (new Vector2(-10, 1.5f), new Vector2(10, 1.5f)),
        (new Vector2(-10, 0.5f), new Vector2(10, 0.5f))
    };

    public TMP_InputField typing_field;
    public GameObject package;

    void Start()
    {
        Invoke(nameof(FocusInputField), 0.1f);
    }

    void Update()
    {
        current_spawn_timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i < activePackages.Count; i++)
            {
                GameObject package = activePackages[i];
                if (package.GetComponent<Package>().toType.Trim().ToLower() == typing_field.text.Trim().ToLower())
                {
                    next_spawn_timer -= 0.08f;
                    typing_field.text = string.Empty;
                    activePackages.RemoveAt(i);
                    Destroy(package);
                    break;
                }
            }
            FocusInputField();
        }

        if (current_spawn_timer <= 0)
        {
            current_spawn_timer = next_spawn_timer;

            int spawnIndex;
            do
            {
                spawnIndex = Random.Range(0, locations.Count);
            } while (spawnIndex == lastSpawnIndex);

            lastSpawnIndex = spawnIndex; 

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
            return string.Empty;
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
