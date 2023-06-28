using UnityEngine;

public class Driver : MonoBehaviour, Observer
{
    private GameObject car;
    
    [SerializeField] private bool showDebugLog = false;

    [Header("WheelColliders")] 
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;
    
    [Header("Acceleration")]
    [SerializeField]private float accelerationFactor = 500.0f;
    [SerializeField]private float brakeFactor = 500.0f;
    
    
    private float _acceleration;
    private float _braking;
    private float _brakeStrength;
    private float _velocity;

    [Header("Steering")]
    [SerializeField]private float maxSteeringAngle = 80.0f;
    [SerializeField]private float steeringAcceleration = 2.0f;
    [SerializeField] private float selfCenteringFactor = 0.99f;
    
    private float _targetSteering;
    private float _currentSteering;

    void OnEnable()
    {
        GetComponentInChildren<Brain>().Attach(this);
    }
    
    void OnDisable()
    {
        GetComponentInChildren<Brain>().Detach(this);
    }
    
    private void FixedUpdate()
    {
        // Acceleration
        frontLeft.motorTorque = accelerationFactor * _acceleration;
        frontRight.motorTorque = accelerationFactor * _acceleration;
        
        // Brake
        frontLeft.brakeTorque = brakeFactor * _brakeStrength;
        frontRight.brakeTorque = brakeFactor * _brakeStrength;
        rearLeft.brakeTorque = brakeFactor * _brakeStrength;
        rearRight.brakeTorque = brakeFactor * _brakeStrength;
        
        // Steering
        float delta = Mathf.Abs(_currentSteering - _targetSteering);
        if (_targetSteering > 0f && (_currentSteering < _targetSteering))
        {
            // Fast to Target
            _currentSteering += delta * steeringAcceleration;
        }
        else if(_targetSteering < 0f && (_currentSteering > _targetSteering))
        {
            // Fast to Target
            _currentSteering -= delta * steeringAcceleration;
        }
        else
        {
            // Slow back to Target
            _currentSteering *= selfCenteringFactor;
        }
        Mathf.Clamp(_currentSteering, -1, 1);
        frontLeft.steerAngle = maxSteeringAngle * _currentSteering;
        frontRight.steerAngle = maxSteeringAngle * _currentSteering;
    }
    
    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is DriveControllEvent driveChange)
        {
            _acceleration = driveChange.Accelerate;
            _brakeStrength = driveChange.Brake;
            _targetSteering = driveChange.Steer;
            if(showDebugLog)
                Debug.Log("Driver: Got new Acceleration and Steering! Yay!" );
        }
    }
}
