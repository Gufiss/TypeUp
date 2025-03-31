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
    "apple", "banana", "ocean", "river", "forest",
    "cloud", "breeze", "happy", "garden", "morning",
    "sunset", "laughter", "puzzle", "window", "blanket",
    "whisper", "shadow", "friend", "smile", "butterfly",
    "castle", "sunshine", "flower", "melody", "gentle",
    "spark", "candle", "glimmer", "pebble", "snowflake",
    "warmth", "feather", "butter", "honey", "pencil",
    "painter", "lighthouse", "harbor", "lantern", "storybook",
    "meadow", "rainbow", "firefly", "treasure", "whistle",
    "echo", "riverbank", "starlight", "moonlight", "cottage",
    "fountain", "horizon", "voyage", "daydream", "seashell",
    "guitar", "sailing", "twinkle", "cotton", "balloon",
    "parade", "silver", "evening", "summer", "autumn",
    "daisy", "coconut", "drizzle", "tulip", "bubble",
    "peach", "harvest", "glow", "twilight", "moonbeam",
    "nest", "hammock", "ripple", "puddle", "raindrop",
    "sunbeam", "cozy", "swan", "mist", "fireplace",
    "comet", "button", "cushion", "velvet", "marble",
    "canvas", "cinnamon", "sprout", "whimsy", "jigsaw",
    "sparkle", "flicker", "cobblestone", "sugar", "fable",
    "chime", "carnival", "giggle", "picnic", "skylark",
    "tender", "dew", "bamboo", "murmur", "tinker",
    "sprinkle", "symphony", "petal", "twirl", "cherry",
    "saffron", "serene", "harmony", "buttercup", "treat",
    "pajamas", "fluffy", "honeycomb", "carousel", "popsicle",
    "peppermint", "whisk", "snuggle", "jellybean", "marshmallow",
    "dandelion", "cupcake", "ribbon", "cuddle", "pitter-patter",
    "snowball", "lullaby", "chocolate", "waffle", "pancake",
    "butterscotch", "truffle", "sugarplum", "gingerbread", "blueberry",
    "strawberry", "whipped", "meringue", "parfait", "cheesecake",
    "macaroon", "butterfly", "glow", "ginger", "sugarcube",
    "snicker", "stardust", "dream", "frost", "delight",
    "satin", "honeydew", "moonlit", "whistle", "jungle",
    "orchard", "starfish", "maple", "lavender", "clover",
    "hazel", "ember", "tangerine", "plum", "raisin",
    "caramel", "mulberry", "fern", "willow", "raven",
    "topaz", "pearl", "meadowlark", "emberglow", "tidal",
    "rosy", "charcoal", "brisk", "glade", "marigold",
    "evergreen", "gentian", "moonrise", "crimson", "butterflyfish",
    "cobweb", "cinnamonstick", "clifftop", "dawnlight", "brook",
    "seabreeze", "goldfinch", "straw", "seaglass", "driftwood"
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
