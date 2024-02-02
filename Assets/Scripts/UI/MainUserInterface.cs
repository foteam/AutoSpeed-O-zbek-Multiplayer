using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUserInterface : MonoBehaviour
{
    [Serializable]
    public class UIElements
    {
        public TextMeshProUGUI userID;
        public TextMeshProUGUI username;
        public TextMeshProUGUI balance;
        public GameObject loadingScreen;
    }

    [Header("UI Elements")] [Space] [SerializeField]
    private UIElements _uiElements;

    async private void Awake()
    {
        #region set USERNAME and BALANCE

        _uiElements.username.text = PlayerPrefsManager.GetString("username").ToString();
        _uiElements.balance.text = PlayerPrefsManager.GetInt("balance").ToString();

        #endregion
        
        #region set USERID

        string username = PlayerPrefsManager.GetString("username");
        string userID = await GetUserID(username);
        _uiElements.userID.text = userID;

        #endregion
        
    }

    async Task<string> GetUserID(string username)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("username", username);
        var result = await MongoDBManager.UserFindDocument(filter);
        if (result != null)
        {
            return result["userID"].ToString();
        }
        else
        {
            return "err01";
        }
    }

    public void MPPlay()
    {
        StartCoroutine(LoadMPMode());
    }

    IEnumerator LoadMPMode()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        _uiElements.loadingScreen.SetActive(true);
        yield return null;
    }
}
