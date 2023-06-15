using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayRayRayYourBoat : MonoBehaviour
{
    private Ray _ray;
    private Vector3 _currentPosition;
    [SerializeField] private float xAxis;
    [SerializeField] private float YAxis;
    [SerializeField] private float ZAxis;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getCurrentPositon();
        sendRay();
    }

    public void sendRay()
    {
        RaycastHit hit;
        
        Vector3 tempVector = Quaternion.Euler(xAxis, YAxis, ZAxis) * transform.forward;
        
        _ray = new Ray(_currentPosition, tempVector);
        Debug.DrawLine(_ray.origin, _ray.origin+tempVector, Color.green);

        
    }

    void getCurrentPositon()
    {
        _currentPosition = transform.position;
    }
    
}
