using System;
using System.Collections.Generic;
using UnityEngine;

public class SidePressureCalculator : ObserveeMonoBehaviour
{
    
    [Header("Debug")]
    [Tooltip("Show DebugLines")]public bool tommysKaeferLines;
    [Tooltip("Show DebugLogs")]public bool tommysKaeferPressureLogs;
    
    // Store Calculated Angles
    private Vector3[] _angles;
    // Store ray holding game objects
    private List<GameObject> _leftRayCasterObjects;
    private List<GameObject> _rightRayCasterObjects;
    // Store current ray, so only one object is needed
    private Ray _ray;
    // Link to Configuration 
    private Configuration _configuration;
    

    // Get the configuration from children
    private void Awake() => _configuration = GetComponentInChildren<Configuration>();
    
    private void OnEnable()
    {
        CreateRayCastObjects();
    }
    
    private void OnDisable()
    {
        DestroyRayCastObjects();
    }
    
    private void Update()
    {
        // Send the calculated pressure event
        NotifyObservers(new PressureChangeEvent(CalculateCurrentPressure()));
    }

    private void CreateRayCastObjects()
    {
        // Create GameObject Lists
        _leftRayCasterObjects = new List<GameObject>();
        _rightRayCasterObjects = new List<GameObject>();
        
        // Create GameObject
        GameObject tempRayCaster = new GameObject("RayCasterObject");
        // Set position of object to current object with this component
        tempRayCaster.transform.position = transform.position;
        // Set parent of GameObject to current object with this component
        tempRayCaster.transform.SetParent(transform);
        
        // Calculate all Angles with the parameters from the config
        CalculateAngle(-1); // Left Side
        for (int i = 0; i < _configuration.RayCount; i++) // Create Left Side RayGameObjects
        {
            GameObject caster = Instantiate(tempRayCaster, transform.position, transform.localRotation, transform);
            caster.transform.localRotation = Quaternion.Euler(_angles[i]);
            _leftRayCasterObjects.Add(caster);
        }
        CalculateAngle(1); // Right Side
        for (int i = 0; i < _configuration.RayCount; i++) // Create Right Side RayGameObjects
        {
            GameObject caster = Instantiate(tempRayCaster, transform.position, transform.localRotation, transform);
            caster.transform.localRotation = Quaternion.Euler(_angles[i]);
            _rightRayCasterObjects.Add(caster);
        }
    }

    private void DestroyRayCastObjects()
    {
        // Destroy temporary GameObjects for each side
        foreach (GameObject o in _leftRayCasterObjects)
        {
            Destroy(o);
        }
        foreach (GameObject o in _leftRayCasterObjects)
        {
            Destroy(o);
        }
    }
    
    private float CalculateCurrentPressure()
    {
        // Get the Pressure from each side and calculate an average
        float currentPressure = CalculateSidedPressure(_leftRayCasterObjects) - CalculateSidedPressure(_rightRayCasterObjects);
        if(tommysKaeferPressureLogs) Debug.Log("Tommys CurrentPressure: " + currentPressure);
        return currentPressure;
    }
    
    private float CalculateSidedPressure(List<GameObject> tempRayCasterObjects)
    {
        // Check Ray Count for 0
        if (_configuration.RayCount <= 0) return 0f;
        
        // Send rays in the specified tempRayCasterObjects and get the results
        bool[] rayHits = SendRays(tempRayCasterObjects);
        
        // Get Ray Hits Length
        int rayCount = _configuration.RayCount;

        // Initialize variables for calculating pressure
        float pressure = 0f;
        // Iterate through the ray hits
        for (int i = 0; i < rayCount; i++)
        {
            if(!rayHits[i]) continue;
            // Get ratio of ray position
            float ratio = i / (float)rayCount;
            // Weight calculation
            pressure = Mathf.Exp(-ratio * _configuration.CalculationCurve);
            break;
        }
        
        // Log the pressure value if tommy's Kaefer logs are enabled
        if (tommysKaeferPressureLogs) Debug.Log("Tommys Pressure: " + pressure);

        // Return the calculated pressure
        return pressure;
    }
    
    // Send a Ray in down direction of each object in List. Return bool Array with hit info
    private bool[] SendRays(List<GameObject> tempRayCasterObjects)
    {
        // Get Config
        float rayLength = _configuration.MaxRayLength;
        int rayCount = _configuration.RayCount;
        // Temp Variable for Hits
        bool[] rayHits = new bool[rayCount];

        Vector3 currentPosition = transform.position;
        int index = 0;
        bool rayHit = false;

        // Send Rays
        foreach (GameObject obj in tempRayCasterObjects)
        {
            // Create Ray
            _ray = new Ray(currentPosition, -obj.transform.up);

            // Send Ray
            if (!rayHit && Physics.Raycast(_ray, out RaycastHit hit, rayLength, _configuration.LayerMask))
            {
                // Draw Ray
                if(tommysKaeferLines) Debug.DrawLine(_ray.origin, hit.point, Color.green);
                // Set hit to true
                rayHits[index] = true;
                rayHit = true;
            }
            else
            {
                // Draw Ray
                if(tommysKaeferLines) Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * rayLength, Color.black);
                // Set hit to false
                rayHits[index] = false;
            }

            index++;
        }
        return rayHits;
    }

    private void CalculateAngle(float direction)
    {
        // Initialiesierung des Winkel Arrays
        int rayCount = _configuration.RayCount;
        _angles = new Vector3[rayCount];
        // Rotationswinkel um die X-Achse
        float rotationX = -_configuration.DetectionAngle;
        // Ray Winkelteilung berechnen
        float rayAngleTotal = _configuration.MaxAngle - _configuration.MinAngle;
        float rayAnglePartial = rayCount > 1 ? (rayAngleTotal / (rayCount - 1)) : rayAngleTotal;
        // Rotationswinkel um die Z-Achse
        for (int i = 0; i < rayCount; i++)
        {
            // Raywinkel berechnen
            float rotationZ = i * rayAnglePartial + _configuration.MinAngle;
            // Winkel als Quaternion
            _angles[i] = new Vector3(rotationX, 0f, rotationZ * direction);
        }
    }
}