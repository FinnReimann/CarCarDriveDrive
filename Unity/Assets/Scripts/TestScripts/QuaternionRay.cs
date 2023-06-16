using UnityEngine;

public class QuaternionRay : MonoBehaviour
{
    public float angle1;
    public float angle2;
    public float angle3;

    private Ray _ray;
    private Vector3 _rayDirection;
    private Vector3 _currentPosition;

    void Update()
    {
        _currentPosition = transform.position;

        //_rayDirection = Quaternion.Euler(0f, angle1, 0f) * Quaternion.Euler(angle2, 0f, 0f) * transform.forward;
        //_rayDirection = transform.forward * angle1 + transform.right * angle2 + transform.up * angle3;
        _rayDirection = Quaternion.Euler(0f , angle2, 0f) * transform.forward;

        _ray = new Ray(_currentPosition,  _rayDirection);

        Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * 10f, Color.magenta);
    }
}


