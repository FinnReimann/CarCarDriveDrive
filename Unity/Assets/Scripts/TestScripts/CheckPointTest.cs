using System;
using UnityEngine;

public class CheckPointTest : MonoBehaviour
{
    [SerializeField] private bool showDebug;
    [SerializeField] private String CheckPointName;
    [SerializeField] private String targetName = "car";
    private bool wasHit;

    public bool WasHit
    {
        get => wasHit;
        set => wasHit = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == targetName)
        {
            if(showDebug)
                Debug.Log(CheckPointName + " was hit by " + other.name);
            wasHit = true;
        }
        
    }
}
