using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomObjectInfo : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI players;

    public void JoinToThisRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        StartCoroutine(LoadingScene());
    }
    IEnumerator LoadingScene()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        yield return null;
    }
}