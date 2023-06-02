using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Vector3 _swingDirection; // Richtungsvektor des Objekts
    private Vector3 _driveDirection; // Richtungsvektor des Objekts
    
    private Configuration _configuration;
    
    private void Awake()
    {
        _configuration = GetComponentInChildren<Configuration>();
    }
    private void Start()
    {
        _swingDirection = Vector3.right;
        _driveDirection = Vector3.forward;
    }

    void Update()
    {
        // Bewegung des Objekts
        transform.parent.Translate(-_swingDirection.normalized * (_configuration.redirectSpeed * Time.deltaTime));
        transform.parent.Translate(_driveDirection.normalized * (_configuration.movementSpeed * Time.deltaTime));
    }
    
    public void SetDirection(Vector3 newDirection)
    {
        // Richtungsvektor setzen und auf x-Achse projizieren
        Debug.Log("Direction: " + _swingDirection);
        _swingDirection = Vector3.Project(newDirection, Vector3.right);
    }
}
