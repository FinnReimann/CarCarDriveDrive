using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Configuration : MonoBehaviour
{
    public bool demoMode;
    
    public int rayCount;
    public float minAngle;
    public float maxAngle;

    public float maxRayLength;
    public float maxSpeed;
    public float steeringBehavior;

    public float detectionAngle;
    public float accelerationBehavior;
    
    public LayerMask raycastMask; // Layer-Maske für den Raycast
}
