using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Configuration : MonoBehaviour
{
    [Header("Demo Settings")]
    [SerializeField]
    private bool demoMode;

    [Header("Navigation Settings")] 
    [SerializeField][Tooltip("The target speed the car trys to reach in km/h. Internally calculated in m/s.")]
    private float targetSpeedInKmh;

    // Ray Settings
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

    // GAcceleration and Brake Behavior Settings
    [Header("Acceleration and Brake Behavior Settings")]
    //(Acceleration Response): Leisurely, Dynamic
    [SerializeField][Range(0f, 1f)][Tooltip("0 is Leisurely(Exp), 0.5 is average (Lin) ,1 is Dynamic(Log).")]
    private float accelerationResponse = 0.5f;
    //(Starting Behavior): Gentle, Powerful
    [SerializeField][Range(0f, 1f)][Tooltip("Close to 0 is Gentle, 1 is Powerful. 0 is no Acceleration at all")]
    private float startingBehabior = 0.75f;
    //(Breaking Response): Early, Late
    [SerializeField][Range(0f, 1f)][Tooltip("Close to 0 is Early Response, 1 is late Response")]
    private float breakingResponse = 0.5f;

    // Obstacle detection 
    [Header("Obstacle Detection")] 
    [SerializeField] private float maxDetectionLength = 10f;
    [SerializeField] private float obstacleEmergancyBreakDistance = 2f;
    [SerializeField] private float obstacleBrakeFaktor = 1f;
    [SerializeField] private Vector3 obstacleRayOffset = new Vector3(0f,-1f,2f);
    
    
    // Layer Configuration
    [Header("Layermask")]
    [SerializeField] private LayerMask streetBorderlayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;
    
    // Getter and Setter for every Variable
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
    

    public float DetectionAngle
    {
        get => detectionAngle;
        set => detectionAngle = value;
    }
    

    public LayerMask LayerMask
    {
        get => streetBorderlayerMask;
        set => streetBorderlayerMask = value;
    }
    public float TargetSpeedInKmh
    {
        get => targetSpeedInKmh;
        set => targetSpeedInKmh = value;
    }
    
    public float AccelerationResponse
    {
        get => accelerationResponse;
        set => accelerationResponse = value;
    }

    public float StartingBehabior
    {
        get => startingBehabior;
        set => startingBehabior = value;
    }

    public float BreakingResponse
    {
        get => breakingResponse;
        set => breakingResponse = value;
    }
    
    public float MaxDetectionLength
    {
        get => maxDetectionLength;
        set => maxDetectionLength = value;
    }
    
    public float ObstacleEmergancyBreakDistance
    {
        get => obstacleEmergancyBreakDistance;
        set => obstacleEmergancyBreakDistance = value;
    }
    
    public float ObstacleBrakeFaktor
    {
        get => obstacleBrakeFaktor;
        set => obstacleBrakeFaktor = value;
    }
    
    public Vector3 ObstacleRayOffset
    {
        get => obstacleRayOffset;
        set => obstacleRayOffset = value;
    }

    public LayerMask ObstacleLayerMask
    {
        get => obstacleLayerMask;
        set => obstacleLayerMask = value;
    }
}
