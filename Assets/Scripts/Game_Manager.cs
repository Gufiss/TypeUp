using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game_Manager : MonoBehaviour
{
    [HideInInspector] public bool gameEnd = false;

    [HideInInspector] public SaveSystem saveSystem;

    public float cur_diff;
    float max_diff;
    float game_speed;
    float current_spawn_timer = 0;
    float next_spawn_timer = 3;

    [HideInInspector]
    public float sessionPlaytime = 0f;

    int lastIndex = -1;
    int lastSpawnIndex = -1;

    [HideInInspector] public int correct_guess = 0;
    [HideInInspector] public int incorrect_guess = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timer;

    [HideInInspector]
    public int score = 0;

    [HideInInspector] public List<GameObject> activePackages = new List<GameObject>();
    List<string> wordList = new List<string>();

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

    //Audio
    public AudioSource correctSound;


    void Start()
    {
        //Audio
        correctSound = gameObject.AddComponent<AudioSource>();
        correctSound.volume = 0.2f;
        correctSound.clip = Resources.Load<AudioClip>("Pickup_Box");

        game_speed = 1.0f;
        saveSystem = GetComponent<SaveSystem>();
        cur_diff = (saveSystem.LoadData("dif") as float? ?? 3f);
        LoadWords();
        Invoke(nameof(FocusInputField), 0.1f);
        UpdateScoreText();
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

                if (packageScript.toType.Trim().ToLower() == typing_field.text.Trim().ToLower())
                {
                    next_spawn_timer -= 0.01f;
                    removeIndex = i; 

                    AddPoint(typing_field.text.Trim().ToLower());
                    typing_field.text = string.Empty;
                    cur_diff += 0.2f;

                    game_speed += 0.01f;
                    UpdateGameSpeed();
                    break;
                }
            }

            if (removeIndex != -1)
            {
                //Audio
                correctSound.Play();

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
        if (!gameEnd)
        {
            sessionPlaytime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(sessionPlaytime / 60);
            int seconds = Mathf.FloorToInt(sessionPlaytime % 60);
            timer.text = $"{minutes:00}:{seconds:00}";

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

                packageScript.speed = game_speed;
                packageScript.endLoc = locations[spawnIndex].end;
                packageScript.toType = GenerateWord();
                packageScript.manager = this;

            TextMeshProUGUI packageText = newBox.GetComponentInChildren<TextMeshProUGUI>(); 
            if (packageText == null)
            {
                TextMeshPro tmp = newBox.GetComponentInChildren<TextMeshPro>();
                if (tmp != null)
                {
                    float savedFontSize = PlayerPrefs.GetFloat("FontSize", 7f);
                    tmp.fontSize = savedFontSize;
                }
            }
            else
            {
                float savedFontSize = PlayerPrefs.GetFloat("FontSize", 7f);
                packageText.fontSize = savedFontSize;
            }

                activePackages.Add(newBox);
            }

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

        int index;
        do
        {
            index = Random.Range(0, wordList.Count);
        } while (index == lastIndex || wordList[index].Length < cur_diff - 3 || wordList[index].Length > cur_diff + 1);

        lastIndex = index;
        return wordList[index].Trim().ToLower();
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

    public void AddPoint(string word)
    {
        score += word.Length;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void LoadWords()
    {
        TextAsset wordFile = Resources.Load<TextAsset>("words");
        if (wordFile != null)
        {
            string[] lines = wordFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            wordList.AddRange(lines);

            string longestWord = "";
            foreach (string word in wordList)
            {
                string trimmedWord = word.Trim();
                if (trimmedWord.Length > longestWord.Length)
                {
                    longestWord = trimmedWord;
                }
            }
            max_diff = longestWord.Length;
        }
        else
        {
            Debug.LogError("Could not find words.txt in Resources folder!");
        }
    }


}
