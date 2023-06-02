using System;
using UnityEngine;

public class DirectionCalculator : MonoBehaviour
{
    public Vector3 CalculateDirection(Ray ray, RaycastHit hit)
    {
        // Berechne den Richtungsvektor vom Kamerastandpunkt und Raycast

        Vector3 direction = hit.point - ray.origin;
        direction.Normalize();

        Debug.Log("Richtungsvektor: " + direction);
        return direction;
    }
}
