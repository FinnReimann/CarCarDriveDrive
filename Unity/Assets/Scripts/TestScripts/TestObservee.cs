using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObservee : ObserveeMonoBehaviour
{
    [SerializeField] private bool changeValues = false;
    [SerializeField] float acceleration = 0f;
    [SerializeField] float breaking = 0f;
    [SerializeField] float steering = 0f;

    void Update()
    {
        if (changeValues)
        {
            DriveControllEvent e = new DriveControllEvent(acceleration, breaking, steering);

            NotifyObservers(e);
            Debug.Log("TestObservee: Notifyed Observers(driver)" );
            changeValues = false;
        }
    }
}
