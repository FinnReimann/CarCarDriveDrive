using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveControllEvent : CCDDEvents
{
    public float Accelerate {set; get; }
    public float Break {set; get; }
    public float Steer {set; get; }
}