using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionDetection : ObserveeMonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool useDebug = false;
    [SerializeField] private float distance;
    
    private float _rayLength;
    private float _rayAngle;
    [SerializeField] private Vector3 rayOffset = new Vector3(0f,-1f,2f);
    [SerializeField] private LayerMask layerMask;
    
    private List<GameObject> _rayCastObjects;
    private Ray _ray;
    private bool _hasHit;

    private Configuration _configuration;
    
    private void Awake() => _configuration = GetComponentInChildren<Configuration>();
    
    private void Start()
    {
        GetConfig();
        _rayCastObjects = new List<GameObject>();
        
    
        
        
        Transform tempTransform = transform;
        GameObject rayCastObj = new GameObject("FrontDetection");
        Transform tempRayTransform = rayCastObj.transform;
        tempRayTransform.position = tempTransform.position;
        tempRayTransform.rotation = tempTransform.rotation;
        tempRayTransform.SetParent(tempTransform);
        tempRayTransform.Translate(rayOffset,Space.Self);
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

    private void Update()
    {
        if (GetRayDistance())
        {
            NotifyObservers(new ObstacleAheadEvent(distance));

        } else if (_hasHit)
        {
            NotifyObservers(new ObstacleAheadEvent(-1f));
            _hasHit = false;
        }
    }

    private void GetConfig()
    {
        _rayLength = _configuration.MaxDetectionLength;
        _rayAngle = -_configuration.MinAngle;
    }
    
    private bool GetRayDistance()
    {
        foreach (var tempTransform in _rayCastObjects.Select(rayCastObject => rayCastObject.transform))
        {
            _ray = new Ray(tempTransform.position,tempTransform.forward);
            if (Physics.Raycast(_ray, out RaycastHit hit, _rayLength, layerMask))
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
