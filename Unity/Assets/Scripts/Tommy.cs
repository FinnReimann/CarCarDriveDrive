using UnityEngine;

public class Tommy : MonoBehaviour
{
    private Quaternion[] _angles;
    private Ray _ray;
    private float _currentPressure;
    
    private Configuration _configuration;

    private void Awake()
    {
        _configuration = GetComponent<Configuration>();
    }

    private void Start()
    {
        //CalculateAngle();
    }

    private void Update()
    {
        //SendRays();
        CalculateCurrentPressure();
    }

    private void CalculateAngle(float direction)
    {
        int rayCount = _configuration.RayCount;
        _angles = new Quaternion[rayCount];
        
        float rayAngleTotal = _configuration.MaxAngle - _configuration.MinAngle;
        float rayAnglePartial = rayAngleTotal / (rayCount - 1);

        float detectionAngle = -_configuration.DetectionAngle;

        if (direction > 0) detectionAngle = -detectionAngle;

        for (int i = 0; i < rayCount; i++)
        {
            _angles[i] = Quaternion.Euler(detectionAngle, direction, (i) * rayAnglePartial + _configuration.MinAngle);
        }
    }

    private bool[] SendRays(float direction)
    {
        CalculateAngle(direction);

        float rayLength = _configuration.MaxRayLength;
        int rayCount = _configuration.RayCount;
        bool[] rayHits = new bool[rayCount];
        RaycastHit hit;

        Transform currentTransform = transform;
        Vector3 currentPosition = currentTransform.position;
        Vector3 negativeUpDirection = -currentTransform.up;

        // Create and Send Rays
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate Ray Direction with direction vector
            Vector3 rayDirection = _angles[i] * negativeUpDirection;

            // Create Rays
            _ray = new Ray(currentPosition, rayDirection);

            // Send Rays
            if (Physics.Raycast(_ray, out hit, rayLength, _configuration.LayerMask))
            {
                // Zeichne Raycast
                Debug.DrawLine(_ray.origin, hit.point, Color.green);
                
                rayHits[i] = true;
            }
            else
            {
                // Zeichne Raycast
                Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * 10f, Color.black);
                
                rayHits[i] = false;
            }
        }
        return rayHits;
    }

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
    }

    private void CalculateCurrentPressure()
    {
        _currentPressure = CalculateSidedPressure(180) - CalculateSidedPressure(0);
        Debug.Log("CurrentPressure: " + _currentPressure);
    }

    public float CurrentPressure => _currentPressure;
}
