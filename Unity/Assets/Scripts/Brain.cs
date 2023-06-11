using UnityEngine;

public class Brain : ObserveeMonoBehaviour, Observer
{
    //(Acceleration Response): Leisurely, Dynamic
    [SerializeField][Range(0f, 1f)] private float accelerationResponse = 0.5f;
    //(Starting Behavior): Gentle, Powerful
    [SerializeField][Range(0f, 1f)] private float startingBehabior = 0.75f;

    [Header("Debug Variables")] 
    [SerializeField] private bool useDebugVariables = false;
    [SerializeField] private float debug_currentSpeed = 0f;
    [SerializeField] private float debug_targetSpeed = 100f;
    [SerializeField] private float debug_currentPressur = 0f;
    
    
    private float _currentSpeed;
    private float _currentPressur;
    private float _targetSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get Conifg from Config Script
        // TODO
    }

    // Update is called once per frame
    void Update()
    {
        if (useDebugVariables)
        {
            _currentSpeed = debug_currentSpeed;
            _targetSpeed = debug_targetSpeed;
            _currentPressur = debug_currentPressur;
        }
        
        
        calculateDriveControll();

        //CCDDEvents e = new DriveControllEvent();
        //NotifyObservers(e);
    }
    
    

    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is SpeedChangeEvent speedChangeEvent)
        {
            _currentSpeed = speedChangeEvent.CurrentSpeed;
        }
        
        if (e is PressureChangeEvent pressureChangeEvent)
        {
            _currentPressur = pressureChangeEvent.CurrentPressure;
        }

        if (e is NavigationEvent navigationEvent)
        {
            
        }
        
    }

    private void calculateDriveControll()
    {
        float acceleration = 0f;
        if (_currentSpeed < _targetSpeed)
        {   // Accelerate
            acceleration = Mathf.Clamp01(CalcCurve(_currentSpeed / _targetSpeed, startingBehabior, accelerationResponse));
            if (useDebugVariables)
                Debug.Log("Speed Ratio: " + _currentSpeed / _targetSpeed + " acceleration: " + acceleration);
            
        }
        else
        {   // Break
            
        }
        
        DriveControllEvent e = new DriveControllEvent
        {
            Accelerate = acceleration,
            Break = 0f,
            Steer = 0f
        };
        NotifyObservers(e);
    }

    // curveBehavior should be beetween 0 and 1. 0.5 is a Linear from Startingpoint to 0.
    // 0-0.5 is logarithmic (fast start, slow end) and 0.5-1 is exponentially (slow start, fast End)
    private float CalcCurve(float input,float startingPoint, float curveBehavior)
    {
        // Make shure everything is between 0 and 1
        float x = Mathf.Clamp01(input);
        startingPoint = Mathf.Clamp01(startingPoint);
        curveBehavior = Mathf.Clamp01(curveBehavior);
        
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
