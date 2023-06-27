using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveControllEvent : CCDDEvents
{
    public DriveControllEvent(float accelerate, float brake, float steer)
    {
        Accelerate = accelerate;
        Brake = brake;
        Steer = steer;
    }


    public float Accelerate {set; get; }
    public float Brake {set; get; }
    public float Steer {set; get; }
}