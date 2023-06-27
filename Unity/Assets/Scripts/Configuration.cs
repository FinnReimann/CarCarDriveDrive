using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Configuration : MonoBehaviour
{
    [Header("Demo Settings")]
    [SerializeField]
    private bool demoMode;

    // Ray-Einstellungen
    [Header("Ray Settings")]
    [SerializeField][Tooltip("Amount of rays")]
    private int rayCount = 8;
    [SerializeField][Range(1f, 5f)][Tooltip("Amount of pressure calculated from 1 = Linear, to 2 = exponential falling.")]
    private float calculationCurve = 3f;
    [SerializeField][Range(0f, 90f)][Tooltip("Angle of the innermost ray.")]
    private float minAngle = 10f;
    [SerializeField][Range(0f, 90f)][Tooltip("Angle of the outermost ray.")]
    private float maxAngle = 45f;
    [SerializeField][Range(1f, 100f)][Tooltip("Maximal length of the rays.")]
    private float maxRayLength = 8f;
    [SerializeField][Range(0f, 90f)][Tooltip("Angle that describes how far the Rays will look to the front.")]
    private float detectionAngle = 0f;

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
    
    public float CalculationCurve
    {
        get => calculationCurve;
        set => calculationCurve = value;
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
