using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;


public class ChatSystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _messages;

    [SerializeField] private TMP_InputField _messageText;
    public void SendMessage()
    {
        if (_messageText.text == "")
        {
            return;
        }

        string message = _messageText.text;
        string username = PlayerPrefsManager.GetString("username");
        _messageText.text = "";
    }
    
    

}
