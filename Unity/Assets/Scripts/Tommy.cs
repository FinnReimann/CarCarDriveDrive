using UnityEngine;

public class Tommy : MonoBehaviour
{
    private bool[] _raysHits;
    private Configuration _configuration;
    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _configuration = GetComponent<Configuration>();
    }
    
    private void Start()
    {
        _raysHits = new bool[_configuration.rayCount];
    }

    private void SendRays()
    {
        Transform currentTransform = transform;
        
        // Create and Send Rays
        for (int i = 0; i < _configuration.rayCount; i++)
        {
            // Create Rays
             _ray = new Ray(currentTransform.position, currentTransform.forward);

             // Send Rays
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _configuration.raycastMask))
            {
                // Zeichne Raycast
                Debug.DrawLine(_ray.origin, _hit.point, Color.green);
                
                _raysHits[i] = true;
            }
            else
            {
                // Zeichne Raycast
                Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * 10f, Color.black);
                
                _raysHits[i] = false;
            }
        }
    }

    public void GetPressure()
    {
        
    }
}
