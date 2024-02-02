using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCarDetector : MonoBehaviour
{
    public string carName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarManager>())
        {
            carName = other.gameObject.GetComponentInParent<CarManager>().carInfo.name;
        }
    }
}
