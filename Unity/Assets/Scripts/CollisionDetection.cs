using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionDetection : ObserveeMonoBehaviour
{
    // Variable to see the value in the inspector
    [Header("Exposed Variables")]
    [SerializeField] private float distance;
    
    // Cached Config Variables
    private float _rayLength;
    private float _rayAngle;
    private Vector3 _rayOffset;
    private LayerMask _layerMask;
    
    // List of objects from which raycasts are sent
    private List<GameObject> _rayCastObjects;
    // Variables to hold the ray
    private Ray _ray;
    private bool _hasHit;

    // Link to Configuration
    private Configuration _configuration;
    
    // Debug Variables for test use, without other Components
    [Header("Debug")]
    [SerializeField] private bool useDebug = false;
    
    // Get the configuration from children
    private void Awake() => _configuration = GetComponentInChildren<Configuration>();
    
    // Get the Configs and creat the ray holding objects
    private void OnEnable()
    {
        GetConfig();
        _rayCastObjects = new List<GameObject>();
        
        Transform tempTransform = transform;
        GameObject rayCastObj = new GameObject("FrontDetection");
        Transform tempRayTransform = rayCastObj.transform;
        tempRayTransform.position = tempTransform.position;
        tempRayTransform.rotation = tempTransform.rotation;
        tempRayTransform.SetParent(tempTransform);
        tempRayTransform.Translate(_rayOffset,Space.Self);
        _rayCastObjects.Add(rayCastObj);

        GameObject leftRay = Instantiate(rayCastObj, tempTransform);
        leftRay.name = "LeftDetection";
        leftRay.transform.Rotate(0f,-_rayAngle,0f);
        _rayCastObjects.Add(leftRay);
        
        GameObject rightRay = Instantiate(rayCastObj, tempTransform);
        rightRay.name = "RightDetection";
        rightRay.transform.Rotate(0f,_rayAngle,0f);
        _rayCastObjects.Add(rightRay);
    }

    // Clear list of ray holding objects
    private void OnDisable()
    {
        _rayCastObjects.Clear();
    }

    
    private void Update()
    {
        // Check if the ray has hit, and if send the distance as event
        if (GetRayDistance())
        {
            NotifyObservers(new ObstacleAheadEvent(distance));

        } else if (_hasHit)
        { // If not hit, send -1 (indicates no hit) once, to inform the observer
            NotifyObservers(new ObstacleAheadEvent(-1f));
            _hasHit = false;
        }
    }

    // Read Config from Configclass and cache them in variables
    private void GetConfig()
    {
        _rayLength = _configuration.MaxDetectionLength;
        _rayAngle = -_configuration.MinAngle;
        _rayOffset = _configuration.ObstacleRayOffset;
        _layerMask = _configuration.ObstacleLayerMask;
    }
    
    // Send a ray for each object in the list. If hit, the distance is saved in the class variable and it returns true.
    private bool GetRayDistance()
    {
        foreach (var tempTransform in _rayCastObjects.Select(rayCastObject => rayCastObject.transform))
        {
            _ray = new Ray(tempTransform.position,tempTransform.forward);
            if (Physics.Raycast(_ray, out RaycastHit hit, _rayLength, _layerMask))
            {
                distance = Vector3.Distance(_ray.origin, hit.point);
                if (useDebug) Debug.DrawLine(_ray.origin, hit.point, Color.green);
                _hasHit = true;
                return true;
            }
            if(useDebug) Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * _rayLength, Color.black);
        }
        return false;
    } 
}
