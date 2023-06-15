using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Tacho : ObserveeMonoBehaviour
{
    private const int QueueSize = 10;
    private Queue<float> _lastValues;
    private Vector3 _lastPosition;
    [Header("Debug Variables")] 
    [SerializeField] private bool debug = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("Tacho meldet sich zum dienst");
        _lastValues = new Queue<float>(QueueSize);
        for (int i = 0; i < QueueSize; i++)
        {
            _lastValues.Enqueue(0);
        }
        _lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Send Speed Data
        SpeedChangeEvent speedChangeEvent = new SpeedChangeEvent(CalculateSpeed(), CalculateRecentAverageSpeed());
        if (debug)
            Debug.Log("Tacho: CurrentPosition: " + transform.position + " CurrentSpeed: " + speedChangeEvent.CurrentSpeed + " RecentAverageSpeed: " + speedChangeEvent.RecentAverageSpeed);
        NotifyObservers(speedChangeEvent);
    }

    float CalculateSpeed()
    {
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(_lastPosition, currentPosition);
        _lastPosition = currentPosition;
        float speed = distance *  (1 / Time.fixedDeltaTime);
        UpdatLastValuesQueue(speed);
        return speed;
    }

    void UpdatLastValuesQueue(float speed)
    {
        _lastValues.Enqueue(speed);
        _lastValues.Dequeue();
    }

    private float CalculateRecentAverageSpeed()
    {
        float result = 0f;
        foreach (float value in _lastValues)
        {
            result += value;
        }
        return result/QueueSize;
    }

    public bool IsTestFinished { get; }
}
