using UnityEngine;

public class QuaternionRay : MonoBehaviour
{
    public float angle1;
    public float angle2;

    private Ray _ray;
    private Vector3 _rayDirection;
    private Vector3 _currentPosition;

    void Update()
    {
        _currentPosition = transform.position;

        Quaternion rotation = Quaternion.Euler(0f, angle1, 0f) * Quaternion.Euler(angle2, 0f, 0f);
        _rayDirection = rotation * transform.forward;

        _ray = new Ray(_currentPosition, _rayDirection);

        Debug.DrawLine(_ray.origin, _ray.origin + _ray.direction * 10f, Color.magenta);
    }
}


