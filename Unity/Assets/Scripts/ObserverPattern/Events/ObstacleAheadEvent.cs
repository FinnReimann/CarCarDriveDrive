using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAheadEvent : CCDDEvents
{
    public ObstacleAheadEvent(float distance)
    {
        Distance = distance;
    }
    public float Distance {set; get; }
}
