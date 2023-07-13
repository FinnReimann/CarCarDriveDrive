using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChangeEvent : ChangeEvents
{
    public SpeedChangeEvent(float speed)
    {
        CurrentSpeed = speed;
        RecentAverageSpeed = speed;
    }

    public SpeedChangeEvent(float currentSpeed, float recentAverageSpeed)
    {
        CurrentSpeed = currentSpeed;
        RecentAverageSpeed = recentAverageSpeed;
    }
    
    public float CurrentSpeed {set; get; }
    public float RecentAverageSpeed {set; get; }
    
    public float Speed {set; get; }
    
}
