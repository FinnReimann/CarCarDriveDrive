using UnityEngine;

public class Tommy : ObserveeMonoBehaviour
{
    private Vector3[] _angles;
    private Ray _ray;
    private float _currentPressure;
    
    private Configuration _configuration;

    private void Awake()
    {
        _configuration = GetComponent<Configuration>();
    }

    private void Update()
    {
        //SendRays();
        CalculateCurrentPressure();
    }

    private void CalculateAngle(float direction)
    {
        // Initialiesierung des Winkel Arrays
        int rayCount = _configuration.RayCount;
        _angles = new Vector3[rayCount];
        
        // Rotationswinkel um die X-Achse
        float rotationX = _configuration.DetectionAngle;
        transform.rotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Vorzeichen für die Drehrichtung um die Z-Achse => direction
        Vector3 forwardDirection = transform.forward;

        // Ray Winkelteilung berechnen
        float rayAngleTotal = _configuration.MaxAngle - _configuration.MinAngle;
        float rayAnglePartial = rayAngleTotal / (rayCount - 1);
        
        // Rotationswinkel um die Z-Achse
        for (int i = 0; i < rayCount; i++)
        {
            // Raywinkel berechnen
            float rayAngle = i * rayAnglePartial + _configuration.MinAngle;
            
            // Winkel als Quaternion
            //_angles[i] = Quaternion.Euler(-(rotationX + (i * 1)), rotationY, rotationZ * direction) * forwardDirection;
            //new Vector3(rotationX + (i*1), 0f, rotationZ * direction)
            // Quaternion.Euler(rotationX + (i*1),0f,0f)
            _angles[i] = Quaternion.Euler(0f, rayAngle * direction, 0f) * forwardDirection;
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
            Debug.Log("Tommy: Richtungswinkel: " + rayDirection);

            // Create Ray
            _ray = new Ray(currentPosition, rayDirection);

            // Send Ray
            if (Physics.Raycast(_ray, out hit, rayLength, _configuration.LayerMask))
            {
                // Draw Ray
                Debug.DrawLine(_ray.origin, hit.point, Color.green);
                
                rayHits[i] = true;
            }
            else
            {
                // Draw Ray
                Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * 10f, Color.black);
                
                rayHits[i] = false;
            }
        }
        return rayHits;
    }
    
    private float CalculateSidedPressure(float direction)
    {
        bool[] rayHits = SendRays(direction);
        int increasingValues = _configuration.IncreasingRays;

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

        Debug.Log("Tommy: " + pressure);
        return pressure;
    }

    /*
    private float CalculateSidedPressure(float direction)
    {
        bool[] rayHits = SendRays(direction);

        float sum = 0f;
        float totalWeight = 0f;

        for (int i = 0; i < rayHits.Length; i++)
        {
            float weight = Mathf.Pow(2f, rayHits.Length - i); // Exponentialwert basierend auf dem Index
            sum += rayHits[i] ? 0f : weight;
            totalWeight += weight;
        }

        float pressure = 1f - (sum / totalWeight);
        pressure = Mathf.Clamp01(pressure);

        Debug.Log(pressure);
        return pressure;
    } */

    
    // Update Event raussenden statt dem kram todo
    private void CalculateCurrentPressure()
    {
        _currentPressure = CalculateSidedPressure(-1) - CalculateSidedPressure(1);
        //Debug.Log("CurrentPressure: " + _currentPressure);
    }

    public float CurrentPressure => _currentPressure;
}
