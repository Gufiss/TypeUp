using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    List<string> wordList = new List<string> { "apple", "banana", "orange", "grape", "peach" };
    int lastIndex = -1;

    public TMP_InputField typing_field;
    public TextMeshProUGUI textBox;

    void Start()
    {
        Invoke(nameof(FocusInputField), 0.1f);
        GenerateWord();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (textBox.text.ToLower().Equals(typing_field.text.Trim().ToLower()))
            {
                typing_field.text = string.Empty;
                GenerateWord();
            }
            FocusInputField();
        }
    }

    public void FocusInputField()
    {
        typing_field.Select();
        typing_field.ActivateInputField();
    }

    void GenerateWord()
    {
        if (wordList.Count == 0)
        {
            Debug.LogError("Word list is empty!");
            return;
        }

        if (wordList.Count == 1)
        {
            textBox.text = wordList[0];
            return;
        }

        int index;
        do
        {
            index = Random.Range(0, wordList.Count);
        } while (index == lastIndex);

        lastIndex = index;
        textBox.text = wordList[index];
    }
}
