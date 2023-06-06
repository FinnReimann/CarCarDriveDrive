using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    private Tommy _tommy;
    private Configuration _configuration;
    
    public float movementSpeed = 1f; // Geschwindigkeit der Bewegung
    public float rotationSpeed = 45f; // Geschwindigkeit der Drehung

    private void Awake()
    {
        _tommy = GetComponentInChildren<Tommy>();
        _configuration = GetComponentInChildren<Configuration>();
    }

    private void Update()
    {
        if (_configuration.DemoMode) return;
            // Objekt entlang der x-Achse bewegen
        transform.parent.Translate(0f, 0f, movementSpeed);
        
        // Berechnung der Drehung um die y-Achse
        float rotationY = _tommy.CurrentPressure * rotationSpeed * Time.deltaTime;

        // Objekt um die y-Achse drehen
        transform.parent.Rotate(0f, rotationY, 0f);
    }
}
