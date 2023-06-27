using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Tacho : ObserveeMonoBehaviour
{
    private const int QueueSize = 2;
    private Queue<float> _lastValues;
    private Vector3 _lastPosition;
    [Header("Debug Variables")] 
    [SerializeField] private bool debug = false;
    private SpeedChangeEvent _lastSpeedChangeEvent;
    
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
        _lastSpeedChangeEvent = new SpeedChangeEvent(-1f,-1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Send Speed Data
        SpeedChangeEvent speedChangeEvent = new SpeedChangeEvent(CalculateSpeed(), CalculateRecentAverageSpeed());
        if(speedChangeEvent.RecentAverageSpeed != _lastSpeedChangeEvent.RecentAverageSpeed)
            NotifyObservers(speedChangeEvent);
        _lastSpeedChangeEvent = speedChangeEvent;
        if (debug)
            Debug.Log("Tacho: CurrentPosition: " + transform.position + " CurrentSpeed: " + speedChangeEvent.CurrentSpeed + " RecentAverageSpeed: " + speedChangeEvent.RecentAverageSpeed);
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
