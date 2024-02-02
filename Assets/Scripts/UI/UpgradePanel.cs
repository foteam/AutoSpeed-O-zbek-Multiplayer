using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Color _selectedColor;

    [SerializeField] private GameObject[] _buttons;
    

    public void Selected(Image thisButton)
    {
        thisButton.color = _selectedColor;
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].gameObject != thisButton.gameObject)
            {
                _buttons[i].GetComponent<Image>().color = _unselectedColor;
            }
        }
    }

    public void SetFrontPodveska(Slider slider)
    {
        RCC_Customization.SetFrontSuspensionsDistances(RCC_SceneManager.Instance.activePlayerVehicle, slider.value);
    }
    public void SetRearPodveska(Slider slider)
    {
        RCC_Customization.SetRearSuspensionsDistances(RCC_SceneManager.Instance.activePlayerVehicle, slider.value);
    }

    public void SetColor(Image img)
    {
        RCC_CarControllerV3 vehicle = RCC_SceneManager.Instance.activePlayerVehicle;
        CarManager carManager = vehicle.GetComponent<CarManager>();
        carManager.carInfo.carColor = img.color;
        PlayerPrefs.SetString(carManager.carInfo.name + "_carColor", ColorUtility.ToHtmlStringRGB(img.color));
        Debug.Log(carManager.carInfo.name + "_carColor");
    }

    public void SaveStat()
    {
        RCC_Customization.SaveStats(RCC_SceneManager.Instance.activePlayerVehicle);
    }
}
