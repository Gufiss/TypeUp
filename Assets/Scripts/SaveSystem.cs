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
    }

    public object LoadData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetString(key) != "")
            {
                return PlayerPrefs.GetString(key);
            }

            if (PlayerPrefs.GetInt(key) != 0)
            {
                return PlayerPrefs.GetInt(key);
            }

            if (PlayerPrefs.GetFloat(key) != 0f)
            {
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
