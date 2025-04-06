using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game_Manager : MonoBehaviour
{
    [HideInInspector] public bool gameEnd = false;

    SaveSystem saveSystem;

    float game_speed;
    float current_spawn_timer = 0;
    float next_spawn_timer = 3;

    int lastIndex = -1;
    int lastSpawnIndex = -1;

    int correct_guess = 0;
    int incorrect_guess = 0;

    [HideInInspector] public List<GameObject> activePackages = new List<GameObject>();
    List<string> wordList = new List<string>
    {
        "adventure", "mystery", "quicksilver", "tapestry", "illusion",
        "echo", "voyage", "horizon", "enigma", "cryptic",
        "labyrinth", "alchemy", "arcane", "serenity", "eclipse",
        "phoenix", "stardust", "temporal", "astronaut", "paradox",

        "legend", "shadow", "memory", "journey", "secret",
        "future", "origin", "presence", "gravity", "signal",
        "tempest", "planetary", "galaxy", "universe", "scientific",
        "hazard", "rescue", "expedition", "tracker", "escapee",
        "liberation", "timeline", "exploration", "visionary", "reflection",
        "citadel", "territory", "jungle", "wilderness", "fortress",
        "conflict", "commander", "adversary", "champion", "operation",
        "arsenal", "barrier", "sorcery", "strength", "curse",
        "enigma", "map", "treasure", "clue", "passage",
        "revelation", "ambition", "hopeful", "decision", "message",
        "frequency", "gadget", "portal", "radiation", "artifact",
        "satellite", "rotation", "spaceship", "rocket", "cosmos",
        "radiance", "obscurity", "risky", "illusion", "uncertainty",
        "outsider", "companion", "foe", "vanished", "recovered",
        "gateway", "dimension", "keyhole", "cipher", "unlock",
        "evidence", "investigation", "operative", "document", "verification",
        "query", "response", "strategy", "objective", "crew",
        "symbolic", "transmission", "identity", "ambition", "command",
        "opportunity", "snare", "threat", "disturbance", "chronos",
        "mechanism", "interval", "duration", "sequence", "momentum",
        "site", "locale", "sector", "zone", "province",
        "access", "departure", "platform", "structure", "threshold",
        "portal", "barrier", "summit", "elevation", "pathway",
        "vessel", "stream", "element", "ember", "gust",
        "atmosphere", "fog", "downpour", "blizzard", "tempest",
        "frost", "blaze", "crystal", "fluctuation", "obscurity",
        "figure", "design", "palette", "signal", "panorama",
        "incident", "turning", "opportunity", "maneuver", "advance",
        "proceed", "depart", "leap", "descend", "ascend",
        "unseal", "conceal", "reveal", "construct", "fracture",
        "transform", "monitor", "pursue", "guide", "navigate",
        "remain", "retreat", "recover", "erase", "recall",
        "formulate", "eliminate", "deliver", "acquire", "transport",
        "grasp", "release", "propel", "withdraw", "manipulate",
        "compose", "examine", "illustrate", "craft", "forge",
        "combat", "triumph", "retreat", "rescue", "investigate",
        "glance", "observe", "detect", "perceive", "encounter",
        "advance", "suspend", "obstruct", "navigate", "engage",
        "shield", "patrol", "pursue", "conceal", "expose",
        "dread", "valiant", "resilient", "fragile", "secure",
        "mute", "deafening", "rapid", "gradual", "lofty",
        "remote", "radiant", "dim", "adjacent", "distant",
        "initial", "ultimate", "forthcoming", "uniform", "distinct",
        "authentic", "counterfeit", "intelligent", "witty", "accurate",
        "misleading", "rigid", "flexible", "dense", "weightless",
        "swift", "precise", "polished", "textured", "immaculate",
        "grimy", "ancient", "novel", "youthful", "historic",
        "futuristic", "straightforward", "intricate", "fundamental", "evident",
        "profound", "vacant", "occupied", "concealed", "motionless",
        "isolated", "unified", "confidential", "transparent", "accessible",
        "sealed", "secured", "monitored", "damaged", "repaired",
        "protected", "hazardous", "living", "deceased", "misplaced",
        "retrieved", "recognized", "obscured", "accurate", "inaccurate"
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
    public GameObject package_rem;
    public Heart_Manager heartManager;
    public Tilemap tilemap;


    void Start()
    {
        saveSystem = GetComponent<SaveSystem>();
        Invoke(nameof(FocusInputField), 0.1f);
        game_speed = 1.0f;
    }

    void Update()
    {
        current_spawn_timer -= Time.deltaTime;


        // Word typing
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int removeIndex = -1;

            for (int i = 0; i < activePackages.Count; i++)
            {
                Package packageScript = activePackages[i].GetComponent<Package>();

                if (packageScript.toType.Trim().ToLower() == typing_field.text)
                {
                    removeIndex = i;

                    ScoreManager.instance.AddPoint();

                    next_spawn_timer -= 0.08f;
                    game_speed += 0.08f;
                    typing_field.text = string.Empty;
                    UpdateGameSpeed();
                    break;
                }
            }

            if (removeIndex != -1)
            {
                GameObject packageToRemove = activePackages[removeIndex];
                Instantiate(package_rem, packageToRemove.transform.position, packageToRemove.transform.rotation);
                activePackages.RemoveAt(removeIndex);
                Destroy(packageToRemove);
                correct_guess++;
            }
            else if (typing_field.text != string.Empty)
            {
                incorrect_guess++;
            }

            FocusInputField();
        }

        // Word spawning
        if (current_spawn_timer <= 0 && !gameEnd)
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

            packageScript.speed = game_speed;
            packageScript.endLoc = locations[spawnIndex].end;
            packageScript.toType = GenerateWord();
            packageScript.manager = this;

            activePackages.Add(newBox);
        }

        if (gameEnd)
        {
            Debug.Log($"Correct: {correct_guess}   Incorrect: {incorrect_guess}   Proc: {(float)correct_guess / (correct_guess + incorrect_guess) * 100:N2}%");

            saveSystem.SaveData("correct_guess", saveSystem.LoadData("correct_guess") as int? ?? 0 + correct_guess);
            saveSystem.SaveData("incorrect_guess", saveSystem.LoadData("incorrect_guess") as int? ?? 0 + incorrect_guess);
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

    void UpdateAnimationSpeed()
    {
        tilemap.animationFrameRate = game_speed;
        Debug.Log("New Animation Speed: " + tilemap.animationFrameRate);
    }

    void UpdateGameSpeed()
    {
        foreach (GameObject obj in activePackages)
        {
            Package package = obj.GetComponent<Package>();
            package.speed = game_speed;
        }
        UpdateAnimationSpeed();
    }
}
