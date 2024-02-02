using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string PlayerNameKey = "PlayerName";
    private const string PlayerScoreKey = "PlayerScore";
    
    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    
    public static string GetString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    
    public static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    
    public static int GetInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    
    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}