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

    private bool[] SendRays(float direction)
    {
        CalculateAngle(direction);

        float rayLength = _configuration.MaxRayLength;
        int rayCount = _configuration.RayCount;
        bool[] rayHits = new bool[rayCount];

        Vector3 currentPosition = transform.position;

        // Create and Send Rays
        for (int i = 0; i < rayCount; i++)
        {
            // Get ray direction
            Vector3 rayDirection = _angles[i];
            // Create GameObject
            GameObject tempRayCaster = new GameObject("RayCasterObject");
            // Set parent of GameObject
            tempRayCaster.transform.SetParent(transform);
            // Set position of object
            tempRayCaster.transform.position = currentPosition;
            // Rotate GameObject
            tempRayCaster.transform.localRotation = Quaternion.Euler(rayDirection);
            // Create Ray
            _ray = new Ray(currentPosition, -tempRayCaster.transform.up);
            // Destroy temporary GameObject
            Destroy(tempRayCaster);

            if(tommysKaeferAngleLogs) Debug.Log("Tommys Richtungswinkel: " + rayDirection);

            // Send Ray
            if (Physics.Raycast(_ray, out RaycastHit hit, rayLength, _configuration.LayerMask))
            {
                // Draw Ray
                if(tommysKaeferLines) Debug.DrawLine(_ray.origin, hit.point, Color.green);
                // Set hit to true
                rayHits[i] = true;
            }
            else
            {
                // Draw Ray
                if(tommysKaeferLines) Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * rayLength, Color.black);
                // Set hit to false
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