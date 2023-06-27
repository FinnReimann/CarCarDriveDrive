using System;
using UnityEngine;

public class Brain : ObserveeMonoBehaviour, Observer
{
    //(Acceleration Response): Leisurely, Dynamic
    [SerializeField][Range(0f, 1f)][Tooltip("0 is Leisurely(Exp), 0.5 is average (Lin) ,1 is Dynamic(Log).")]
    private float accelerationResponse = 0.5f;
    //(Starting Behavior): Gentle, Powerful
    [SerializeField][Range(0f, 1f)][Tooltip("Close to 0 is Gentle, 1 is Powerful. 0 is no Acceleration at all")]
    private float startingBehabior = 0.75f;
    //(Breaking Response): Early, Late
    [SerializeField][Range(0f, 1f)][Tooltip("Close to 0 is Early Response, 1 is late Response")]
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
    private float _currentPressur;
    private float _targetSpeed;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponentInChildren<Tacho>().Attach(this);
        GetComponentInChildren<Tommy>().Attach(this);
    }
    
    void OnDisable()
    {
        GetComponentInChildren<Tacho>().Detach(this);
        GetComponentInChildren<Tommy>().Detach(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (useDebugTarget)
        {
            _targetSpeed = debug_targetSpeed;
        }
        if (useDebugTacho)
        {
            _currentSpeed = debug_currentSpeed;
        }
        if (useDebugPressur)
        {
            _currentPressur = debug_currentPressur;
        }
        
        NotifyObservers(calculateDriveControll());
    }

    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is SpeedChangeEvent speedChangeEvent)
        {
            if (showDebugLog)
                Debug.Log("Get new Speed from Tacho");
            if (!useDebugTacho)
                _currentSpeed = speedChangeEvent.CurrentSpeed;
        }
        
        if (e is PressureChangeEvent pressureChangeEvent)
        {
            if (!useDebugPressur)
                _currentPressur = pressureChangeEvent.CurrentPressure;
            Debug.Log("Get new Pressure from Tommy: " + _currentPressur);
        }

        if (e is NavigationEvent navigationEvent)
        {
            if(!useDebugTarget)
                _targetSpeed = navigationEvent.TargetSpeed;
        }
    }

    private DriveControllEvent calculateDriveControll()
    {
        // Acceleration and braking
        float acceleration = 0f;
        float breaking = 0f;
        float steering = 0f;
        float speedRatio = _currentSpeed / _targetSpeed;
        if (speedRatio < 1)
        {   // Accelerate
            acceleration = Mathf.Clamp01(CalcCurve(speedRatio, startingBehabior, accelerationResponse));
            if (showDebugLog)
                Debug.Log("Speed Ratio: " + _currentSpeed / _targetSpeed + " acceleration: " + acceleration);
            
        }
        else if (speedRatio > 1)
        {   // Break
            breaking = 1f-Mathf.Clamp01(CalcCurve(speedRatio - 1f, 1f, breakingResponse));
            if (showDebugLog)
                Debug.Log("Speed Ratio: " + _currentSpeed / _targetSpeed + " breaking: " + breaking);
        }

        steering = _currentPressur;

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
        return (-(x / (1/startingPoint)) + 1*startingPoint) * Mathf.Exp(x*y);
    }
    
    
}
