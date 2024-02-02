using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Michsky.MUIP;
using MongoDB.Bson;
using TMPro;
using UnityEngine;
using MongoDB.Driver;
using UnityEngine.SceneManagement;

public class UserRegistration : MonoBehaviour
{
    [Header("Input fields:")]
    [SerializeField] private TMP_InputField _username;
    [SerializeField] private TMP_InputField _promocode;

    [Space] [Header("Notifications:")]
    [SerializeField] private NotificationManager _doneNotification;
    [SerializeField] private NotificationManager _errorNotification;

    [Space] [Header("Connection status:")] [SerializeField]
    private GameObject _connectionSpinner;

    [SerializeField] private GameObject _sumbitButton;

    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        if (PlayerPrefsManager.GetString("username") != "")
        {
            StartCoroutine(LoadingScene());
        }
    }

    private void Start()
    {
        StartCoroutine(ConnectionStatus());
    }

    IEnumerator LoadingScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            Debug.Log("Loading: " + operation.progress);
            yield return null;
        }
    }

    async public void Registration()
    {
        string username = _username.text;
        string promocode = _promocode.text;

        #region -- Check --

        if (username == "" || username == null)
        {
            _errorNotification.title = "Taxallus kiritmagansiz!!";
            _errorNotification.description = "Siz taxallus kiritmadingiz, davom etish uchun iltimos taxallus kiriting!";
            _errorNotification.UpdateUI();
            _errorNotification.Open();
            _errorNotification.timer = 5;
            return;
        }
        
        if (!await FindUser(username)) // username already
        {
            _errorNotification.title = "Taxallusingizni ozgartiring!";
            _errorNotification.description = "Bunday taxallus o'yinda mavjud, davom etish uchun boshqa taxallus oylab toping!";
            _errorNotification.UpdateUI();
            _errorNotification.Open();
            _errorNotification.timer = 6;
            return;
        }

        #endregion

        #region -- New User --
        
        _doneNotification.Open();
        BsonDocument newUser = new BsonDocument {{"userID", GenerateUserID()},{"username", username}, {"balance", 500}, {"promocode", promocode}};
        await MongoDBManager.UserInsertDocument(newUser);
        PlayerPrefsManager.SaveString("username", username);
        PlayerPrefsManager.SaveInt("balance", 500);
        Debug.Log("New user added!");
        StartCoroutine(LoadingScene());

        #endregion
    }

    IEnumerator ConnectionStatus()
    {
        while (true)
        {
            bool status = MongoDBManager.CheckConnectionStatus();
            yield return new WaitForSeconds(2);
            if (status = true)
            {
                _sumbitButton.SetActive(true);
                _connectionSpinner.SetActive(false);
                yield break;
            }
        }
    }
    async Task<bool> FindUser(string username)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("username", username);
        var result = await MongoDBManager.UserFindDocument(filter);
        if (result != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string GenerateUserID()
    {
        return System.Guid.NewGuid().ToString();
    }
}
