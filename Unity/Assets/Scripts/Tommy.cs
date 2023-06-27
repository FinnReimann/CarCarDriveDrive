using UnityEngine;

public class Tommy : ObserveeMonoBehaviour
{
    [Header("Debug")]
    public bool tommysKaeferLines;
    public bool tommysKaeferAngleLogs;
    public bool tommysKaeferPressureLogs;
    
    private Vector3[] _angles;
    private Ray _ray;

    private Configuration _configuration;

    private void Awake()
    {
        _configuration = GetComponentInChildren<Configuration>();
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
            if(tommysKaeferAngleLogs) Debug.Log("Tommys Richtungswinkel: " + rayDirection);
            
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
            float ratio = i / (float)rayCount;

            // Weight calculation
            float weight = Mathf.Exp(-ratio * _configuration.CalculationCurve);

            // Accumulate the total weight
            if (rayHits[i] && weight > pressure)
            {
                pressure = weight;
            }
        }

        // Clamp the pressure between 0 and 1
        // pressure = Mathf.Clamp01(pressure);

        // Log the pressure value if tommy's Kaefer logs are enabled
        if (tommysKaeferPressureLogs) Debug.Log("Tommys Pressure: " + pressure);

        // Return the calculated pressure
        return pressure;
    }
    
    private float CalculateCurrentPressure()
    {
        float currentPressure = CalculateSidedPressure(-1) - CalculateSidedPressure(1);
        if(tommysKaeferPressureLogs) Debug.Log("Tommys CurrentPressure: " + currentPressure);
        return currentPressure;
    }

    private void FixedUpdate()
    {
        PressureChangeEvent pressureChangeEvent = new PressureChangeEvent(CalculateCurrentPressure());
        NotifyObservers(pressureChangeEvent);
    }
}