using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : ObserveeMonoBehaviour
{
    public float targetSpeed;
    protected float lastSpeed;
    private Configuration _configuration;

    
    private void Awake() => _configuration = GetComponentInChildren<Configuration>();

    public void Start()
    {
        Debug.Log("Navigator Online");
        GetConfig();
    }

    protected virtual void Update()
    {
        GetConfig();
        if (lastSpeed != targetSpeed)
        {
            lastSpeed = targetSpeed;
            NotifyObservers(new NavigationEvent(targetSpeed));
        }
    }
    
    protected void GetConfig()
    {
        targetSpeed = _configuration.TargetSpeedInKmh / 3.6f;
    }
}
