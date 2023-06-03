using UnityEngine;

public class CurveDetection : MonoBehaviour
{
    private MoveScript _moveScript;
    private LineDetector _lineDetector;
    private Configuration _configuration;

    private void Awake()
    {
        _moveScript = GetComponentInParent<MoveScript>();
        _lineDetector = GetComponent<LineDetector>();
        _configuration = GetComponent<Configuration>();
    }

    private void Update()
    {
        // Führe die Kurvenerkennung durch
        //DetectCurve();
    }

    private void DetectCurve()
    {
        // Sende Raycasts aus den Seiten-Detektoren
        Vector3 rightRay = _lineDetector.GetRayAsVector3(_configuration.rightDetector);
        Vector3 leftRay = _lineDetector.GetRayAsVector3(_configuration.leftDetector);

        // Überprüfe, ob eine Kurve erkannt wurde
        if (rightRay != Vector3.zero && leftRay != Vector3.zero)
        {
            // Kurve Erkannt
            Debug.Log("Kurve Erkannt!");
            
            // Ermittle die Richtung der erkannten Kurve
            float rotationAmount = Vector3.SignedAngle(rightRay, leftRay, Vector3.up) / 180f;

            // Rotiere das Fahrzeug entsprechend der Kurvenrichtung
            _moveScript.TurnVehicle(rotationAmount);
        }
    }
}

