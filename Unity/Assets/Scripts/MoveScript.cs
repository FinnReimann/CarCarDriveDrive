using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Vector3 _driveDirection; // Richtungsvektor des Objekts
    private float _rotationSpeed;
    
    private Configuration _configuration;
    
    private void Awake()
    {
        _configuration = GetComponentInChildren<Configuration>();
    }
    private void Start()
    {
        _driveDirection = Vector3.forward;
        _rotationSpeed = _configuration.steeringSpeed;
    }

    void Update()
    {
        DriveForward();
        RotateVehicle();
    }

    private void DriveForward()
    {
        // Bewegung des Objekts
        transform.parent.Translate(_driveDirection.normalized * (_configuration.movementSpeed * Time.deltaTime));
    }

    public void ChangeRotation(Ray ray, RaycastHit hit)
    {
        Vector3 localDirection = transform.InverseTransformDirection(ray.direction);

        // Überprüfe das Vorzeichen der X-Komponente, um die Richtung zu bestimmen
        if (localDirection.x > 0f)
        {
            // Push Vehicle to the left
            Debug.Log("Raycast geht in positive X-Richtung der Kamera.");
            _rotationSpeed = -_configuration.steeringSpeed;
        }
        else if (localDirection.x < 0f)
        {
            // Push Vehicle to the right
            Debug.Log("Raycast geht in negative X-Richtung der Kamera.");
            _rotationSpeed = _configuration.steeringSpeed;
        }
        else
        {
            Debug.Log("Raycast geht nicht in die X-Richtung der Kamera.");
        }
    }

    private void RotateVehicle()
    {
        // Rotiere das Objekt um die Y-Achse basierend auf der Eingabe
        transform.parent.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
