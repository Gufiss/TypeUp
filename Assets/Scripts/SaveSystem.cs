using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveData(string key, object data)
    {
        switch (data)
        {
            case string str:
                Debug.Log("Saving string data");
                PlayerPrefs.SetString(key, str);
                break;

            case int i:
                Debug.Log("Saving int data");
                PlayerPrefs.SetInt(key, i);
                break;

            case float f:
                Debug.Log("Saving float data");
                PlayerPrefs.SetFloat(key, f);
                break;

            default:
                Debug.Log("Unsupported data type");
                break;
        }

        PlayerPrefs.Save();
    }

    public object LoadData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetString(key) != "")
            {
                Debug.Log("Loading string data");
                return PlayerPrefs.GetString(key);
            }

            if (PlayerPrefs.GetInt(key) != 0)
            {
                Debug.Log("Loading int data");
                return PlayerPrefs.GetInt(key);
            }

            if (PlayerPrefs.GetFloat(key) != 0f)
            {
                Debug.Log("Loading float data");
                return PlayerPrefs.GetFloat(key);
            }
        }

        return null;
    }

    public void DeleteData(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
