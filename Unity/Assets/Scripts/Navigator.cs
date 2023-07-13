using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : ObserveeMonoBehaviour
{
    // Cached Variables
    public float targetSpeed;
    protected float lastSpeed;
    
    private Configuration _configuration;

    // Get the configuration from children
    private void Awake() => _configuration = GetComponentInChildren<Configuration>();

    public void Start()
    {
        Debug.Log("Navigator Online");
        GetConfig();
    }

    // Update the Config in every frame, and if the target speed was changed send the new speed as event
    protected virtual void Update()
    {
        GetConfig();
        if (lastSpeed != targetSpeed)
        {
            lastSpeed = targetSpeed;
            NotifyObservers(new NavigationEvent(targetSpeed));
        }
    }
    // Read Config from Configclass and cache them in variables
    protected void GetConfig()
    {
        targetSpeed = _configuration.TargetSpeedInKmh / 3.6f;
    }
}
