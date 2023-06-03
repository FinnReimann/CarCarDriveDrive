using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class LineDetector : MonoBehaviour
{
    private MoveScript _moveScript;
    private Configuration _configuration;

    private void Awake()
    {
        _moveScript = GetComponentInParent<MoveScript>();
        _configuration = GetComponent<Configuration>();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray;
        
        Debug.Log("Now checking for hits!");
        if (!SendRay(_configuration.frontDetector, out hit, out ray, Color.red))
        {
            LineColorDetector(ray, hit);
        }
        if (!SendRay(_configuration.rightDetector, out hit, out ray, Color.green))
        {
            LineColorDetector(ray, hit);
        }
        if (!SendRay(_configuration.leftDetector, out hit, out ray, Color.green))
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
        // Detecting Line Now
        // Debug.Log("Detecting Line Now!");
        
        // Überprüfe, ob der Raycast ein Objekt getroffen hat
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        
        // Nullpointer Check
        if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null ||
            meshCollider == null) return;

        // Object Hit!
        //Debug.Log("Object hit!");

        // Hole die Textur des getroffenen Objekts
        Texture2D texture = renderer.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= texture.width;
        pixelUV.y *= texture.height;

        // Get Pixel Color
        Color pixelColor = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        // Debug.Log("Farbe des pixels: " + pixelColor);
        // Debug.Log("Die Detection Color: " + _configuration.detectionColor);

        // Überprüfe, ob der Pixel eine bestimmte Farbe hat
        if (pixelColor == _configuration.detectionColor)
        {
            // Pixel mit richtiger Farbe gefunden
            //Debug.Log("Pixel mit richtiger Farbe gefunden bei: (" + (int)pixelUV.x + ", " + (int)pixelUV.y + ")");
            
            // Fahrzeug rotieren
            _moveScript.ChangeRotation(ray, hit);
        }
        else
        {
            Debug.Log("Nicht die richtige Farbe!");
        }
    }
    
    public Vector3 GetRayAsVector3(GameObject raycaster)
    {
        RaycastHit hit;
        Ray ray;
    
        if (SendRay(raycaster, out hit, out ray, Color.black))
        {
            // Kein Treffer, gib einen Nullvektor zurück
            return Vector3.zero;
        }
    
        Vector3 hitVector = hit.point - ray.origin;
        //Debug.Log("Ray as Vector: " + hitVector);
    
        return hitVector;
    }
    
}
