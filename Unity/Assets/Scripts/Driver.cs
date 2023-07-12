using UnityEngine;
using UnityEngine.Serialization;

public class Driver : MonoBehaviour, Observer
{
    private GameObject car;
    
    [SerializeField] private bool showDebugLog = false;

    // Set the wheelcollider for every wheel
    [Header("WheelColliders")] 
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;
    
    // Config variables
    [Header("Engine and Break Power")]
    [SerializeField]private float enginePower = 800.0f;
    [SerializeField]private float brakeForce = 500.0f;
    
    // Variables in which the current state is saved
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

    // Register on Observers
    void OnEnable()
    {
        GetComponentInChildren<Brain>().Attach(this);
    }
    // Deregister on Observers
    void OnDisable()
    {
        GetComponentInChildren<Brain>().Detach(this);
    }
    
    // In every FixedUpdate (Physics timing)
    private void FixedUpdate()
    {
        // Acceleration (only to the front wheels)
        frontLeft.motorTorque = enginePower * _acceleration;
        frontRight.motorTorque = enginePower * _acceleration;
        
        // Brake (to all 4 wheels)
        frontLeft.brakeTorque = brakeForce * _brakeStrength;
        frontRight.brakeTorque = brakeForce * _brakeStrength;
        rearLeft.brakeTorque = brakeForce * _brakeStrength;
        rearRight.brakeTorque = brakeForce * _brakeStrength;
        
        // Steering
        // If the abs target steering in the same direction and lower, than go slow back.
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
    
    // Check for event type - and if, update cache values
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
