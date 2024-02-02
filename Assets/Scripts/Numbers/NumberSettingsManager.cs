using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using UnityEngine;
using TMPro;

public class NumberSettingsManager : MonoBehaviour
{
    [Serializable]
    public class NumberSettings
    {
        public TextMeshProUGUI regionText;
        public TextMeshProUGUI numberText;
    }
    [Header("Number settings:")]
    [Space]
    public NumberSettings numberSettings;

    [SerializeField] private NotificationManager _errorNotification;
    [Space] [SerializeField] private GameObject _numberPanel;
    
    public void SetRegion(string region)
    {
        MMCarDetector carDetector = FindObjectOfType<MMCarDetector>();
        Debug.Log("Okay region is: "+region);
        numberSettings.regionText.text = region;
        PlayerPrefsManager.SaveString(carDetector.carName+"_car_nm_region", numberSettings.regionText.text.ToString());
    }

    public void CheckNumber()
    {
        bool correctNumber = CheckStringFormat(numberSettings.numberText.text.ToString());
        if (correctNumber)
        {
            Debug.Log("Number is correct!");
            MMCarDetector carDetector = FindObjectOfType<MMCarDetector>();
            PlayerPrefsManager.SaveString(carDetector.carName+"_car_nm_number", numberSettings.numberText.text.ToString());
            _numberPanel.SetActive(false);
        }
        else
        {
            _errorNotification.title = "Nomerda xatolik!";
            _errorNotification.description = "Nomerni tog'ri kiriting!";
            _errorNotification.UpdateUI();
            _errorNotification.Open();
            Debug.Log("Numbers is not correct");
        }
    }
    
    bool CheckStringFormat(string input)
    {
        char[] chars = input.ToCharArray();
        if (char.IsLetter(chars[0]) &&
            char.IsDigit(chars[1]) &&
            char.IsDigit(chars[2]) &&
            char.IsDigit(chars[3]) &&
            char.IsLetter(chars[4]) &&
            char.IsLetter(chars[5]))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
