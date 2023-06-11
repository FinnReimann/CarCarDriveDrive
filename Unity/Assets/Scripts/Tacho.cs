using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tacho : ObserveeMonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Send Dummy Data
        SpeedChangeEvent speedChangeEvent = new SpeedChangeEvent();
        speedChangeEvent.CurrentSpeed = 10f;
        speedChangeEvent.ChangeDelta = 0f;
        speedChangeEvent.RecentAverageSpeed = 10f;
        //NotifyObservers(speedChangeEvent);
    }
}
