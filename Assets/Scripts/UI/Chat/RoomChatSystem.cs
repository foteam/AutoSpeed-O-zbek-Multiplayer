using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomChatSystem : MonoBehaviour, IChatClientListener
{
    [Header("UI Elements")] 
    [Space] 
    [SerializeField] private TMP_InputField _messageInput;
    [SerializeField] private TextMeshProUGUI _messages;
    [SerializeField] private TextMeshProUGUI _lastMessages;

    private ChatClient _chatClient;
    protected internal ChatAppSettings _appSettings;

    private void Start()
    {
        _chatClient = new ChatClient(this);
        _chatClient.ChatRegion = "eu"; // Укажите свой регион
        _appSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        _chatClient.Connect(_appSettings.AppIdChat, _appSettings.AppVersion, new AuthenticationValues(PlayerPrefsManager.GetString("username")));
    }

    private void LateUpdate()
    {
        if (this._chatClient != null)
        {
            this._chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }
    }

    public void SendMessageToServer()
    {
        if (_messageInput.text == "")
        {
            return;
        }
        _chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, _messageInput.text);
        _messageInput.text = "";
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(String.Format("{0}-{1}", level, message));
    }

    private void ReconnectToChat()
    {
        if (_chatClient != null && _chatClient.State != ChatState.ConnectedToFrontEnd)
        {
            // Выполняем пересоединение только если мы не подключены
            _chatClient.Disconnect(); // Опционально отключаем текущее соединение
            
            _chatClient = new ChatClient(this);
            _chatClient.ChatRegion = "eu"; // Укажите свой регион
            _appSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
            _chatClient.Connect(_appSettings.AppIdChat, _appSettings.AppVersion, new AuthenticationValues(PlayerPrefsManager.GetString("username")));
        }
    }
    public void OnDisconnected()
    {
        Debug.Log("Chat app disconnected");
        ReconnectToChat();
    }

    public void OnConnected()
    {
        _chatClient.Subscribe(new string[] { PhotonNetwork.CurrentRoom.Name });
        Debug.Log("Chat app connected");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(PhotonNetwork.CurrentRoom.Name))
        {
            ShowMessages(channelName);
        }
    }

    private void ShowMessages(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = _chatClient.TryGetChannel(channelName, out channel);
        if (found)
        {
            string[] colors = new[] { "red", "blue", "yellow", "green", "purple" };
            string lastMessage = channel.Messages.Last().ToString();
            string lastSender = channel.Senders.Last().ToString();
            string msg = string.Format("<color={0}>{1}</color>: {2}\n", colors[Random.Range(0, colors.Length-1)], lastSender, lastMessage);
            
            _messages.text += msg;
            _lastMessages.text = msg;
            _lastMessages.gameObject.GetComponent<DOTweenAnimation>().DOPlay();
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to channel: " + PhotonNetwork.CurrentRoom.Name);
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }
}
