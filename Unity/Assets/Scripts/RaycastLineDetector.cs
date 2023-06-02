using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class RaycastLineDetector : MonoBehaviour
{
    private DirectionCalculator _directionCalculator;
    private MoveScript _moveScript;
    private Configuration _configuration;

    private void Awake()
    {
        _directionCalculator = GetComponent<DirectionCalculator>();
        _moveScript = GetComponentInParent<MoveScript>();
        _configuration = GetComponent<Configuration>();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray;
        
        Debug.Log("Now checking for hits!");
        if (!SendRay(_configuration.frontRay, out hit, out ray, Color.red))
        {
            LineColorDetector(ray, hit);
        }
        if (!SendRay(_configuration.rightRay, out hit, out ray, Color.green))
        {
            LineColorDetector(ray, hit);
        }
        if (!SendRay(_configuration.leftRay, out hit, out ray, Color.green))
        {
            LineColorDetector(ray, hit);
        }
    }

    private bool SendRay(GameObject raycaster, out RaycastHit hit, out Ray ray, Color color)
    {
        // Sende einen Raycast aus
        var transformVar = raycaster.transform;
        ray = new Ray(transformVar.position, -transformVar.up);

        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _configuration.raycastMask)) return true;
        
        // Draw Raycast
        DrawRay(ray, hit, color);

        return false;
    }

    private static void DrawRay(Ray ray, RaycastHit hit, Color color)
    {
        // Zeichne Raycast
        Debug.DrawLine(ray.origin, hit.point, color);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void LineColorDetector(Ray ray, RaycastHit hit)
    {
        // Überprüfe, ob der Raycast ein Objekt getroffen hat
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null ||
            meshCollider == null) return;

        Debug.Log("Object hit!");

        // Hole die Textur des getroffenen Objekts
        Texture2D texture = renderer.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= texture.width;
        pixelUV.y *= texture.height;

        Color pixelColor = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        // Debug.Log("Farbe des pixels: " + pixelColor);
        // Debug.Log("Die Detection Color: " + _configuration.detectionColor);

        // Überprüfe, ob der Pixel eine bestimmte Farbe hat
        if (pixelColor == _configuration.detectionColor)
        {
            Debug.Log("Pixel mit richtiger Farbe gefunden bei: (" + (int)pixelUV.x + ", " + (int)pixelUV.y + ")");
            _moveScript.SetDirection(_directionCalculator.CalculateDirection(ray, hit));
        }
        else
        {
            Debug.Log("Nicht die richtige Farbe!");
        }
    }

    public Vector3 getRayAsVector3(GameObject raycaster)
    {
        if(SendRay(raycaster, out var hit, out var ray, Color.black)) return Vector3.zero;
        Vector3 hitVector = hit.point - ray.origin;
        Debug.Log("Ray as Vector: " + hitVector);
        return hitVector;
    }
}
