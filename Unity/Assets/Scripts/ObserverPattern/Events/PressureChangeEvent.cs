using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureChangeEvent : ChangeEvents
{
    public PressureChangeEvent(float currentPressure)
    {
        CurrentPressure = currentPressure;
    }
    public float CurrentPressure {set; get; }
    
}
