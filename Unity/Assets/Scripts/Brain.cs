using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class Brain : ObserveeMonoBehaviour, Observer
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
        CCDDEvents e = new DriveControllEvent();
        NotifyObservers(e);
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
/*
        if (e is NavigationEvent navigationEvent)
        {
            
        }
  */      
    }

    private void calculateDriveControll()
    {
        CCDDEvents e = new DriveControllEvent();
        NotifyObservers(e);
    }
}
