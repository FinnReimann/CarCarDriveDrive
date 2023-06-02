using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Configuration : MonoBehaviour
{
    public GameObject frontRay;
    public GameObject backRay;
    public GameObject rightRay;
    public GameObject leftRay;


    public float redirectSpeed;
    public float movementSpeed;
    
    public float roadWidth;
    public float margin;
    public float forwardDetection;
    public Color detectionColor;
    public LayerMask raycastMask; // Layer-Maske für den Raycast

    private float _distanceToRoad;
    private float _sideCameraAngle;
    private Quaternion _frontCameraAngle;
    private Vector3 _driveDirection;
    
    private DirectionCalculator _directionCalculator;
    private RaycastLineDetector _raycastLineDetector;

    private void Awake()
    {
        _directionCalculator = GetComponent<DirectionCalculator>();
        _raycastLineDetector = GetComponent<RaycastLineDetector>();
    }

    private void Start()
    {
        _driveDirection = Vector3.forward;
        _distanceToRoad = _raycastLineDetector.getRayAsVector3(frontRay).y;
        Debug.Log("Distance to road: " + _distanceToRoad);
        _sideCameraAngle = (float)(Math.Atan(((roadWidth / 2) - margin) / _distanceToRoad) * 180 / Math.PI);
        Debug.Log("Camera angle: " + _sideCameraAngle);
        
        CameraRotator();
    }

    private void CameraRotator()
    {
        rightRay.transform.Rotate(_driveDirection, _sideCameraAngle);
        leftRay.transform.Rotate(_driveDirection, -_sideCameraAngle);
    }
}
