using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChangeEvent : ChangeEvents
{
    public float CurrentSpeed {set; get; }
    public float ChangeDelta {set; get; }
    public float RecentAverageSpeed {set; get; }
    
}
