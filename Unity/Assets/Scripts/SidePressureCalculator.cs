using System;
using System.Collections.Generic;
using UnityEngine;

public class SidePressureCalculator : ObserveeMonoBehaviour
{
    [Header("Debug")]
    public bool tommysKaeferLines;
    public bool tommysKaeferPressureLogs;
    
    private Vector3[] _angles;
    private Ray _ray;
    private Configuration _configuration;
    private List<GameObject> _leftRayCasterObjects;
    private List<GameObject> _rightRayCasterObjects;

    private void Awake() => _configuration = GetComponentInChildren<Configuration>();

    private void OnEnable()
    {
        // Create GameObject Lists
        _leftRayCasterObjects = new List<GameObject>();
        _rightRayCasterObjects = new List<GameObject>();
        
        // Create GameObject
        GameObject tempRayCaster = new GameObject("RayCasterObject");
        // Set position of object
        tempRayCaster.transform.position = transform.position;
        // Set parent of GameObject
        tempRayCaster.transform.SetParent(transform);
        
        CalculateAngle(-1);
        for (int i = 0; i < _configuration.RayCount; i++)
        {
            GameObject caster = Instantiate(tempRayCaster, transform.position, transform.localRotation, transform);
            caster.transform.localRotation = Quaternion.Euler(_angles[i]);
            _leftRayCasterObjects.Add(caster);
        }
        CalculateAngle(1);
        for (int i = 0; i < _configuration.RayCount; i++)
        {
            GameObject caster = Instantiate(tempRayCaster, transform.position, transform.localRotation, transform);
            caster.transform.localRotation = Quaternion.Euler(_angles[i]);
            _rightRayCasterObjects.Add(caster);
        }
    }

    private void OnDisable()
    {
        // Destroy temporary GameObjects
        foreach (GameObject o in _leftRayCasterObjects)
        {
            Destroy(o);
        }
        foreach (GameObject o in _leftRayCasterObjects)
        {
            Destroy(o);
        }
    }
    
    private void Update()
    {
        // Nicht jedes mal neuen Pressure wenn sich nix ändert todo
        PressureChangeEvent pressureChangeEvent = new PressureChangeEvent(CalculateCurrentPressure());
        NotifyObservers(pressureChangeEvent);
    }
    
    private float CalculateCurrentPressure()
    {
        float currentPressure = CalculateSidedPressure(-1) - CalculateSidedPressure(1);
        if(tommysKaeferPressureLogs) Debug.Log("Tommys CurrentPressure: " + currentPressure);
        return currentPressure;
    }
    
    private float CalculateSidedPressure(float direction)
    {
        // Check Ray Count for 0
        if (_configuration.RayCount <= 0) return 0f;
        
        // Send rays in the specified direction and get the results
        bool[] rayHits = SendRays(direction);
        
        // Get Ray Hits Length
        int rayCount = _configuration.RayCount;

        // Initialize variables for calculating pressure
        float pressure = 0f;
        // Iterate through the ray hits
        for (int i = 0; i < rayCount; i++)
        {
            // Get ratio of ray position
            float ratio = i / (float)rayCount;
            // Weight calculation
            float weight = Mathf.Exp(-ratio * _configuration.CalculationCurve);
            // Accumulate the total weight
            if (rayHits[i] && weight > pressure) pressure = weight;
        }

        // Clamp the pressure between 0 and 1
        // pressure = Mathf.Clamp01(pressure);

        // Log the pressure value if tommy's Kaefer logs are enabled
        if (tommysKaeferPressureLogs) Debug.Log("Tommys Pressure: " + pressure);

        // Return the calculated pressure
        return pressure;
    }

    private bool[] SendRays(float direction)
    {
        List<GameObject> tempRayCasterObjects = direction == -1 ? _leftRayCasterObjects : _rightRayCasterObjects;

        float rayLength = _configuration.MaxRayLength;
        int rayCount = _configuration.RayCount;
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