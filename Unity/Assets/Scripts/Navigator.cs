using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : ObserveeMonoBehaviour
{
    public float targetSpeed;
    protected float lastSpeed;
    
    public void Start()
    {
        Debug.Log("Navigator Online");
    }

    protected virtual void Update()
    {
        if (lastSpeed != targetSpeed)
        {
            lastSpeed = targetSpeed;
            NotifyObservers(new NavigationEvent(targetSpeed));
        }
    }
}
