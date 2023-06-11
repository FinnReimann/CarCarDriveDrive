using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour, Observer, Observee
{
    private float _currentSpeed;
    private float _currentPressur;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    

    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is SpeedChangeEvent speedChangeEvent)
        {
            _currentSpeed = speedChangeEvent.CurrentSpeed;
        }
        
        if (e is PressureChangeEvent pressureChangeEvent)
        {
            _currentPressur = pressureChangeEvent.CurrentPressure;
        }
        
    }

    void NotifyObservers(Event e)
    {
        throw new NotImplementedException();
    }
}
