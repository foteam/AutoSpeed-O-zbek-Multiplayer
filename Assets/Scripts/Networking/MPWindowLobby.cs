using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.MUIP;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MPWindowLobby : MonoBehaviourPunCallbacks
{
    [Header("Room name input field")] 
    public TMP_InputField roomNameInputField;
    
    [Header("List parent & room prefab")]
    public Transform listContent;
    public GameObject roomPrefab;
    
    [Space]
    
    [Header("Messsages")]
    public GameObject roomsHavent;
    public NotificationManager errorNotification;
    public GameObject loadingScreen;

    [Space]
    
    public GameObject[] allRooms;
    
    private void Awake()
    {
        Debug.Log("Connecting to master/server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master/server...");
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        string roomName = roomNameInputField.text;
        
        List<GameObject> roomList = allRooms.ToList();
        GameObject room  = roomList.FirstOrDefault(r => r.GetComponentInChildren<TextMeshProUGUI>().text == roomName);
        bool exists = (room != null);
        
        if (exists)
        {
            #region -- Error Notification --
            errorNotification.title = "Bunday nom band!";
            errorNotification.description = roomName + " nomli server mavjud, boshqa nom kiriting!";
            errorNotification.UpdateUI();
            errorNotification.Open();
            #endregion
            Debug.Log("Room with name " + roomName + " already exists!");
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() {MaxPlayers = 10});
            StartCoroutine(LoadingScene());
            Debug.Log("Room created!");
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count >= 1)
        {
            roomsHavent.SetActive(false);
        } else if (roomList.Count == 0)
        {
            roomsHavent.SetActive(true);
        }
        for (var i = 0; i < allRooms.Length; i++)
        {
            if (allRooms[i] != null)
            {
                Destroy(allRooms[i]);
            }
        }

        allRooms = new GameObject[roomList.Count];
        for (var i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].IsOpen && roomList[i].IsVisible && roomList[i].PlayerCount >= 1)
            {
                GameObject room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity,
                    listContent.transform);
                RoomObjectInfo roomObjectInfo = room.GetComponent<RoomObjectInfo>();
                roomObjectInfo.roomName.text = roomList[i].Name;
                roomObjectInfo.players.text = roomList[i].PlayerCount + "/" + roomList[i].MaxPlayers.ToString();

                allRooms[i] = room;
            }
        }
    }

    IEnumerator LoadingScene()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("SampleScene");
        yield return null;
    }
}
