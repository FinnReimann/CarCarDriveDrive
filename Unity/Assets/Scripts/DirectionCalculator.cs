using UnityEngine;

public class DirectionCalculator : MonoBehaviour
{
    public Vector3 CalculateDirection(Ray ray, RaycastHit targetObject, int xCoordinate, int yCoordinate)
    {
        Transform hitTransform = targetObject.transform;

        // Berechne die Position des Pixels in Weltkoordinaten
        Vector3 pixelPosition = hitTransform.TransformPoint(xCoordinate, yCoordinate, 0);

        // Berechne den Richtungsvektor vom Kamerastandpunkt zur Pixelposition
        Vector3 direction = pixelPosition - ray.origin;
        direction.Normalize();

        Debug.Log("Richtungsvektor: " + direction);
        return direction;
    }
}
