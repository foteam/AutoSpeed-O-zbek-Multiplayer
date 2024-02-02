using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.UI;

public class CarTuning : MonoBehaviour
{

    public void SetFrontWheelSuspension(Slider slider)
    {
        RCC_Customization.SetFrontSuspensionsDistances(RCC_SceneManager.Instance.activePlayerVehicle, slider.value);
    }
    public void SetBackWheelSuspension(Slider slider)
    {
        RCC_Customization.SetRearSuspensionsDistances(RCC_SceneManager.Instance.activePlayerVehicle, slider.value);
    }
}
