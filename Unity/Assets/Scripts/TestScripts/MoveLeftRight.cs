using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    private Tommy _tommy;
    
    public float movementSpeed = 1f; // Geschwindigkeit der Bewegung

    private void Awake()
    {
        _tommy = GetComponentInChildren<Tommy>();
    }

    private void Update()
    {
        // Berechnung der Bewegung in x-Richtung
        float movementX = _tommy.CurrentPressure * movementSpeed * Time.deltaTime;

        // Objekt entlang der x-Achse bewegen
        transform.parent.Translate(movementX, 0f, 0f);
    }
}
