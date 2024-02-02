using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using WebSocketSharp;

public class CarManager : MonoBehaviourPun, IPunObservable
{
    [Serializable]
    public class CarInfo
    {
        public string name;
        public TextMeshProUGUI frontNumber, frontRegion, backRegion, backNumber;
        public Color carColor;
        public MeshRenderer carBody;
    }

    public CarInfo carInfo;

    private string localNumber;
    private string localRegion;


    private void Start()
    {
        StartCoroutine(LoadStats());
        Debug.Log(PlayerPrefs.GetString(carInfo.name + "_carColor"));
        if (PlayerPrefs.GetString(carInfo.name + "_carColor") != "")
        {
            Color loadedColor;
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(carInfo.name + "_carColor"), out loadedColor);
            carInfo.carColor = loadedColor;
            return;
        }
        carInfo.carColor = carInfo.carBody.material.color;
    }

    IEnumerator LoadStats()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (RCC_SceneManager.Instance.activePlayerVehicle)
            {
                RCC_Customization.LoadStats(RCC_SceneManager.Instance.activePlayerVehicle);
                Debug.Log("stats loaded");
                yield break;;
            }
        }
    }
    private void LateUpdate()
    {
        GetCarNumber();
        carInfo.carBody.material.color = carInfo.carColor;
        if (Input.GetKey(KeyCode.Space))
        {
            RCC_Customization.LoadStats(RCC_SceneManager.Instance.activePlayerVehicle);
        }
    }

    private void GetCarNumber()
    {
        string number = PlayerPrefsManager.GetString(carInfo.name+"_car_nm_number");
        string region = PlayerPrefsManager.GetString(carInfo.name+"_car_nm_region");

        if (PhotonNetwork.InRoom && photonView.IsMine)
        {
            photonView.RPC("UpdateNumber", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, number, region);
        }

        #region IN MAIN SCENE CHANGING NUMBER
        if (number != "" && !PhotonNetwork.InRoom)
        {
            char[] arr = number.ToCharArray();
            string parsedNumber = arr[0] + " " + arr[1] + arr[2] + arr[3] + " " + arr[4] + arr[5].ToString();
            carInfo.backNumber.text = parsedNumber;
            carInfo.frontNumber.text = parsedNumber;
            carInfo.backRegion.text = region;
            carInfo.frontRegion.text = region;
        }
        if (number == "" && !PhotonNetwork.InRoom)
        {
            carInfo.backNumber.text = "T 684 HA";
            carInfo.frontNumber.text = "T 684 HA";
            carInfo.backRegion.text = "01";
            carInfo.frontRegion.text = "01";
        }
        #endregion
    }
    
    [PunRPC]
    public void UpdateNumber(int userID, string number, string region)
    {
        char[] arr = number.ToCharArray();
        string parsedNumber = arr[0] + " " + arr[1] + arr[2] + arr[3] + " " + arr[4] + arr[5].ToString();
                    
        carInfo.backNumber.text = parsedNumber;
        carInfo.frontNumber.text = parsedNumber;
        carInfo.backRegion.text = region;
        carInfo.frontRegion.text = region;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        string number = PlayerPrefsManager.GetString(carInfo.name+"_car_nm_number");
        string region = PlayerPrefsManager.GetString(carInfo.name+"_car_nm_region");

        if (stream.IsWriting)
        {
            stream.SendNext(number);
            stream.SendNext(region);
            
            stream.SendNext(ColorUtility.ToHtmlStringRGB(carInfo.carColor));
        }
        else
        {
            localNumber = (string)stream.ReceiveNext();
            localRegion = (string)stream.ReceiveNext();

            Color color;
            ColorUtility.TryParseHtmlString("#" + (string)stream.ReceiveNext(), out color);
            carInfo.carColor = color;
            
            char[] arr = localNumber.ToCharArray();
            string parsedNumber = arr[0] + " " + arr[1] + arr[2] + arr[3] + " " + arr[4] + arr[5].ToString();

            carInfo.backNumber.text = parsedNumber;
            carInfo.frontNumber.text = parsedNumber;
            carInfo.backRegion.text = localRegion;
            carInfo.frontRegion.text = localRegion;
        }
    }
}
