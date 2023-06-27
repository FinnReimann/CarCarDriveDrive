using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationEvent : CCDDEvents
{
    public NavigationEvent(float targetSpeed)
    {
        TargetSpeed = targetSpeed;
    }

    public float TargetSpeed { set; get; }
}
