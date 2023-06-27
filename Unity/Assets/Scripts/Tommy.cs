using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Tommy : ObserveeMonoBehaviour
{
    [Header("Debug")]
    public bool tommysKaeferLines;
    public bool tommysKaeferLogs;
    
    private Vector3[] _angles;
    private Ray _ray;

    private Configuration _configuration;

    private void Awake()
    {
        _configuration = GetComponent<Configuration>();
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
        float rayAnglePartial;
        if (rayCount > 1)
        {
            rayAnglePartial = rayAngleTotal / (rayCount - 1);
        }
        else
        {
            rayAnglePartial = rayAngleTotal;
        }

        // Rotationswinkel um die Z-Achse
        for (int i = 0; i < rayCount; i++)
        {
            // Raywinkel berechnen
            float rotationZ = i * rayAnglePartial + _configuration.MinAngle;
            
            // Winkel als Quaternion
            _angles[i] = new Vector3(rotationX, 0f, rotationZ * direction);
        }
    }

    private bool[] SendRays(float direction)
    {
        CalculateAngle(direction);

        float rayLength = _configuration.MaxRayLength;
        int rayCount = _configuration.RayCount;
        bool[] rayHits = new bool[rayCount];
        RaycastHit hit;
        
        Vector3 currentPosition = transform.position;

        // Create and Send Rays
        for (int i = 0; i < rayCount; i++)
        {
            // Get Ray Direction
            Vector3 rayDirection = _angles[i];
            if(tommysKaeferLogs) Debug.Log("Tommys Richtungswinkel: " + rayDirection);
            
            // Create GameObjects
            GameObject tempRayCaster = new GameObject("RayCasterObject");
            // Set Parent
            tempRayCaster.transform.SetParent(transform);
            // Set Position of Object
            tempRayCaster.transform.position = currentPosition;
            // Rotate GameObject
            tempRayCaster.transform.localRotation = Quaternion.Euler(_angles[i]);

            // Create Ray
            _ray = new Ray(currentPosition, -tempRayCaster.transform.up);
            
            Destroy(tempRayCaster);

            // Send Ray
            if (Physics.Raycast(_ray, out hit, rayLength, _configuration.LayerMask))
            {
                // Draw Ray
                if(tommysKaeferLines) Debug.DrawLine(_ray.origin, hit.point, Color.green);
                
                rayHits[i] = true;
            }
            else
            {
                // Draw Ray
                if(tommysKaeferLines) Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * rayLength, Color.black);
                rayHits[i] = false;
            }
        }
        return rayHits;
    }
    
    private float CalculateSidedPressure(float direction)
    {
        bool[] rayHits = SendRays(direction);
        int increasingValues = _configuration.IncreasingRays;
        if (increasingValues > rayHits.Length)
        {
            increasingValues = rayHits.Length;
        }

        float sum = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < rayHits.Length; i++)
        {
            float weight;

            if (i < increasingValues)
            {
                weight = Mathf.Pow(2f, increasingValues - i);
            }
            else
            {
                weight = Mathf.Pow(2f, i - increasingValues + 1);
            }

            sum += rayHits[i] ? 0f : weight;
            totalWeight += weight;
        }

        float pressure = 1f - (sum / totalWeight);
        pressure = Mathf.Clamp01(pressure);

        if(tommysKaeferLogs) Debug.Log("Tommys Pressure: " + pressure);
        return pressure;
    }
    
    // Update Event raussenden statt dem kram todo
    private float CalculateCurrentPressure()
    {
        float currentPressure = CalculateSidedPressure(-1) - CalculateSidedPressure(1);
        if(tommysKaeferLogs) Debug.Log("Tommys CurrentPressure: " + currentPressure);
        return currentPressure;
    }

    private void FixedUpdate()
    {
        PressureChangeEvent pressureChangeEvent = new PressureChangeEvent(CalculateCurrentPressure());
        NotifyObservers(pressureChangeEvent);
    }
}
