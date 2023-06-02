using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float speed = 2f; // Geschwindigkeit des Objekts

    private Vector3 _direction; // Richtungsvektor des Objekts

    private void Start()
    {
        _direction = Vector3.right;
    }

    void Update()
    {
        // Bewegung des Objekts
        transform.parent.Translate(-_direction.normalized * (speed * Time.deltaTime));
    }
    
    public void SetDirection(Vector3 newDirection)
    {
        // Richtungsvektor setzen und auf x-Achse projizieren
        Debug.Log(_direction);
        _direction = Vector3.Project(newDirection, Vector3.right);
    }
}
