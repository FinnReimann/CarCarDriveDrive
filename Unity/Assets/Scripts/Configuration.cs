using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Configuration : MonoBehaviour
{
    [SerializeField]
    [Header("Demo Settings")]
    private bool demoMode;

// Ray-Einstellungen
    [SerializeField]
    [Header("Ray Settings")]
    private int rayCount;
    [SerializeField]
    private int increasingRays;
    [SerializeField] 
    [Range(0f, 90f)]
    private float minAngle;
    [SerializeField] 
    [Range(0f, 90f)]
    private float maxAngle;
    [SerializeField] 
    [Range(1f, 100f)]
    private float maxRayLength;
    [SerializeField] 
    [Range(0f, 90f)]
    private float detectionAngle;

// Geschwindigkeit und Steuerungsverhalten
    [SerializeField]
    [Header("Speed and Steering Behavior Settings")]
    private float maxSpeed;
    [SerializeField] private float steeringBehavior;
    [SerializeField] private float accelerationBehavior;

// Erkennungseinstellungen
    [FormerlySerializedAs("raycastMask")]
    [SerializeField]
    [Header("Layermask")]
    private LayerMask layerMask; 

    public bool DemoMode
    {
        get => demoMode;
        set => demoMode = value;
    }

    public int RayCount
    {
        get => rayCount;
        set => rayCount = value;
    }
    
    public int IncreasingRays
    {
        get => increasingRays;
        set => increasingRays = value;
    }

    public float MinAngle
    {
        get => minAngle;
        set => minAngle = value;
    }

    public float MaxAngle
    {
        get => maxAngle;
        set => maxAngle = value;
    }

    public float MaxRayLength
    {
        get => maxRayLength;
        set => maxRayLength = value;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    public float SteeringBehavior
    {
        get => steeringBehavior;
        set => steeringBehavior = value;
    }

    public float DetectionAngle
    {
        get => detectionAngle;
        set => detectionAngle = value;
    }

    public float AccelerationBehavior
    {
        get => accelerationBehavior;
        set => accelerationBehavior = value;
    }

    public LayerMask LayerMask
    {
        get => layerMask;
        set => layerMask = value;
    }
}
