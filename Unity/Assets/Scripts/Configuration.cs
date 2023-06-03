using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Configuration : MonoBehaviour
{
    public GameObject frontDetector;
    public GameObject backDetector;
    public GameObject rightDetector;
    public GameObject leftDetector;
    
    public float movementSpeed;
    public float steeringSpeed;
    
    public float roadWidth;
    public float margin;
    public float frontDetection;
    public Color detectionColor;
    public LayerMask raycastMask; // Layer-Maske für den Raycast

    private float _distanceToRoad;
    private float _sideDetectorAngle;
    private float _frontDetectorAngle;
    private Vector3 _driveDirection;
    
    private RaycastLineDetector _raycastLineDetector;

    private void Awake()
    {
        _raycastLineDetector = GetComponent<RaycastLineDetector>();
    }

    private void Start()
    {
        _driveDirection = Vector3.forward;
        
        // Calculate Detector Angles
        ProcessMathRotation();
        
        // Apply Detector Angles
        CameraRotator();
    }

    private void ProcessMathRotation()
    {
        // Get distance to Ground
        // Debug.Log("Distance to road: " + _distanceToRoad);
        _distanceToRoad = _raycastLineDetector.getRayAsVector3(frontDetector).y;
        
        // Calculate Angles for SideDetectors
        // Debug.Log("Camera angle: " + _sideDetectorAngle);
        _sideDetectorAngle = (float)(Math.Atan(((roadWidth / 2) - margin) / _distanceToRoad) * 180 / Math.PI);
        
        // Calculate Angle for Front Detector
        _frontDetectorAngle = (float)(Math.Atan(frontDetection / _distanceToRoad) * 180 / Math.PI);
    }

    private void CameraRotator()
    {
        rightDetector.transform.Rotate(_driveDirection, _sideDetectorAngle);
        leftDetector.transform.Rotate(_driveDirection, -_sideDetectorAngle);
        frontDetector.transform.Rotate(Vector3.right, _frontDetectorAngle);
    }
}
