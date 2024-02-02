using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _spawnPoint;
    [Space] 
    [SerializeField] private GameObject _carPrefab;

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

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are joined to room: "+PhotonNetwork.CurrentRoom.Name);
        
        // Spawn new player to world
        RCC_CarControllerV3 newVehicle = PhotonNetwork.Instantiate(_carPrefab.name, _spawnPoint.position, Quaternion.identity).GetComponent<RCC_CarControllerV3>();
        
        RCC.RegisterPlayerVehicle(newVehicle); // Register new player
        RCC.SetControl(newVehicle, true); // Set control to true

        if (RCC_SceneManager.Instance.activePlayerCamera)
            RCC_SceneManager.Instance.activePlayerCamera.SetTarget(newVehicle);
    }
}
