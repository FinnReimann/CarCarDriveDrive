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
    
    private LineDetector _lineDetector;

    private void Awake()
    {
        _lineDetector = GetComponent<LineDetector>();
    }

    private void Start()
    {
        _driveDirection = Vector3.forward;
        
        // Calculate Detector Angles
        ProcessMathRotation();
        
        // Apply Detector Angles
        RotateDetector();
    }

    private void ProcessMathRotation()
    {
        // Get distance to Ground
        // Debug.Log("Distance to road: " + _distanceToRoad);
        _distanceToRoad = _lineDetector.GetRayAsVector3(frontDetector).y;
        
        // Calculate Angles for SideDetectors
        // Debug.Log("Camera angle: " + _sideDetectorAngle);
        _sideDetectorAngle = (float)(Math.Atan(((roadWidth / 2) - margin) / _distanceToRoad) * 180 / Math.PI);
        
        // Calculate Angle for Front Detector
        _frontDetectorAngle = (float)(Math.Atan(frontDetection / _distanceToRoad) * 180 / Math.PI);
    }

    private void RotateDetector()
    {
        // Rotate to the Side
        rightDetector.transform.Rotate(_driveDirection, _sideDetectorAngle);
        leftDetector.transform.Rotate(_driveDirection, -_sideDetectorAngle);
        
        // Rotate to the Front
        frontDetector.transform.Rotate(Vector3.right, _frontDetectorAngle);
        //leftDetector.transform.Rotate(Vector3.right, _frontDetectorAngle);
        //rightDetector.transform.Rotate(Vector3.right, _frontDetectorAngle);
    }
}
