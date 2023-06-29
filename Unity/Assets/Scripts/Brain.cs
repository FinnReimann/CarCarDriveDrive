using System;
using UnityEngine;

public class Brain : ObserveeMonoBehaviour, Observer
{
    //(Acceleration Response): Leisurely, Dynamic
    private float accelerationResponse = 0.5f;
    //(Starting Behavior): Gentle, Powerful
    private float startingBehabior = 0.75f;
    //(Breaking Response): Early, Late
    private float breakingResponse = 0.5f;

    [Header("Debug Variables")] 
    [SerializeField] private bool useDebugTarget = false;
    [SerializeField] private bool useDebugTacho = false;
    [SerializeField] private bool useDebugPressur = false;
    [SerializeField] private bool showDebugLog = false;
    [SerializeField] private float debug_currentSpeed = 0f;
    [SerializeField] private float debug_targetSpeed = 100f;
    [SerializeField] private float debug_currentPressur = 0f;
    
    private float _currentSpeed;
    private float _currentPressure;
    private float _targetSpeed;

    private Configuration _configuration;

    private void Awake() => _configuration = GetComponentInChildren<Configuration>();
    void OnEnable()
    {
        try
        {
            GetComponentInChildren<Speedometer>().Attach(this);
            GetComponentInChildren<SidePressureCalculator>().Attach(this);
            GetComponentInChildren<Navigator>().Attach(this);
        }
        catch (Exception e)
        {
            Debug.LogWarning("The Brain needs a Speedometer, SidePressureCalculator and a Navigator to work proper");
        }
        GetConfig();
    }
    
    void OnDisable()
    {
        try
        {
            GetComponentInChildren<Speedometer>().Detach(this);
            GetComponentInChildren<SidePressureCalculator>().Detach(this);
            GetComponentInChildren<Navigator>().Detach(this);
        }
        catch (Exception e)
        {
            Debug.LogWarning("The Brain needs a Speedometer, SidePressureCalculator and a Navigator to work proper");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetConfig();
        if (useDebugTarget)
            _targetSpeed = debug_targetSpeed;
        if (useDebugTacho)
            _currentSpeed = debug_currentSpeed;
        if (useDebugPressur)
            _currentPressure = debug_currentPressur;
        
        NotifyObservers(CalculateDriveControll());
    }

    private void GetConfig()
    {
        accelerationResponse = _configuration.AccelerationResponse;
        startingBehabior = _configuration.StartingBehabior;
        breakingResponse = _configuration.AccelerationResponse;
    }

    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is SpeedChangeEvent speedChangeEvent)
        {
            if (showDebugLog)
                Debug.Log("Get new Speed from Speedometer");
            if (!useDebugTacho)
                _currentSpeed = speedChangeEvent.RecentAverageSpeed;
        }
        
        if (e is PressureChangeEvent pressureChangeEvent)
        {
            if (!useDebugPressur)
                _currentPressure = pressureChangeEvent.CurrentPressure;
            if (showDebugLog)
                Debug.Log("Get new Pressure from SidePressureCalculator: " + _currentPressure);
        }

        if (e is NavigationEvent navigationEvent)
        {
            if(!useDebugTarget)
                _targetSpeed = navigationEvent.TargetSpeed;
        }
    }

    private DriveControllEvent CalculateDriveControll()
    {
        // Acceleration and braking
        float acceleration = 0f;
        float breaking = 0f;
        float steering = 0f;
        
        float speedRatio = _currentSpeed / _targetSpeed;
        if (speedRatio < 1)
        {   // Accelerate
            acceleration = CalcCurve(speedRatio, startingBehabior, accelerationResponse);
            if (showDebugLog)
                Debug.Log("Speed Ratio: " + _currentSpeed / _targetSpeed + " acceleration: " + acceleration);
            
        }
        else if (speedRatio > 1)
        {   // Break
            breaking = 1f-CalcCurve(speedRatio - 1f, 1f, breakingResponse);
            if (showDebugLog)
                Debug.Log("Speed Ratio: " + _currentSpeed / _targetSpeed + " breaking: " + breaking);
        }
        if (_targetSpeed == 0f)
            breaking = 1;

        // Steering
        steering = _currentPressure;

        DriveControllEvent e = new DriveControllEvent(acceleration, breaking, steering);

        return e;
    }

    // curveBehavior should be beetween 0 and 1. So 0.5 is linear from Startingpoint to 0.
    // 0-0.5 is logarithmic (fast start, slow end) and 0.5-1 is exponentially (slow start, fast End)
    private float CalcCurve(float input,float startingPoint, float curveBehavior)
    {
        // Make shure everything is between 0 and 1
        float x = Mathf.Clamp01(input);
        startingPoint = Mathf.Clamp01(startingPoint);
        curveBehavior = Mathf.Clamp01(curveBehavior);
        if (startingPoint == 0f)
            return 0f; // Exit early if Startingpoint is 0. 
        float y;
        
        if (curveBehavior > 0.5f)
        {   // If over 0.5 the curveBehavior Faktor is between 0 and 2
            y = 4f*curveBehavior-2f;
        }
        else
        {   // If under 0.5 the curveBehavior Faktor is between -10 and 0
            y = 20f*curveBehavior-10f;
        }
        return Mathf.Clamp01(startingPoint * (Mathf.Exp(x * y) - x));
    }
}
