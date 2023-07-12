using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TestTools;

public class Speedometer : ObserveeMonoBehaviour
{
    private const int QueueSize = 2;
    private Queue<float> _lastValues;
    private Vector3 _lastPosition;
    [Header("Debug Variables")] 
    [SerializeField] private bool debug = false;
    [SerializeField] private float currentSpeedInKmh;

    // Init every last values with 0, and safe current position.
    public void Start()
    {
        Debug.Log("Speedometer online");
        _lastValues = new Queue<float>(QueueSize);
        for (int i = 0; i < QueueSize; i++)
        {
            _lastValues.Enqueue(0);
        }
        _lastPosition = transform.position;
    }
    
    // Trigger calculation of speed. If not moving > return early.
    void FixedUpdate()
    {
        float currentSpeed = CalculateSpeed();
        if (Math.Abs(currentSpeed) < 0.0001f)
        {
            currentSpeedInKmh = 0f;
            return;
        }
        // Send speed event
        NotifyObservers(new SpeedChangeEvent(currentSpeed, CalculateRecentAverageSpeed()));
        // Show current speed in the inspector
        currentSpeedInKmh = currentSpeed * 3.6f;
        if (debug)
            Debug.Log("Speedometer: CurrentPosition: " + transform.position + " CurrentSpeed: " + currentSpeed + " RecentAverageSpeed: " + CalculateRecentAverageSpeed());
    }

    // calculate the speed using the last and current position
    float CalculateSpeed()
    {
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(_lastPosition, currentPosition);
        _lastPosition = currentPosition;
        float speed = distance *  (1 / Time.fixedDeltaTime);
        UpdatLastValuesQueue(speed);
        return speed;
    }

    // Safe current speed in queue and delete oldest
    void UpdatLastValuesQueue(float speed)
    {
        _lastValues.Enqueue(speed);
        _lastValues.Dequeue();
    }

    // calculate the average speed from all values in the queue
    private float CalculateRecentAverageSpeed()
    {
        float result = 0f;
        foreach (float value in _lastValues)
        {
            result += value;
        }
        return result/QueueSize;
    }
}
