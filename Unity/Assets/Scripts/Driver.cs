using UnityEngine;

public class Driver : MonoBehaviour, Observer
{
    private Brain brainToWatch;
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
    
    
    private float acceleration;
    private float braking;
    private float brakeStrength;
    private float _velocity;

    [Header("Steering")]
    [SerializeField]private float maxSteeringAngle = 80.0f;
    [SerializeField]private float steeringAcceleration = 2.0f;
    [SerializeField] private float selfCenteringFactor = 0.99f;
    
    private float targetSteering;
    private float currentSteering;

    [Header("for testing")]
    [SerializeField]private float _MaxVelocity = 50.0f;
    //[SerializeField] private float TestSteeringSpeed;
    //[SerializeField] private float TestAccSpeed;

    private void Awake()
    {
        brainToWatch = GetComponentInChildren<Brain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Nachrichten vom Brain abonnieren
        brainToWatch.Attach(this);
    }

    private void FixedUpdate()
    {
        // Acceleration
        frontLeft.motorTorque = accelerationFactor * acceleration;
        frontRight.motorTorque = accelerationFactor * acceleration;
        
        // Brake
        frontLeft.brakeTorque = brakeFactor * brakeStrength;
        frontRight.brakeTorque = brakeFactor * brakeStrength;
        rearLeft.brakeTorque = brakeFactor * brakeStrength;
        rearRight.brakeTorque = brakeFactor * brakeStrength;
        
        // Steering
        
        float delta = Mathf.Abs(currentSteering - targetSteering);
        if (targetSteering > 0f && (currentSteering < targetSteering))
        {
            // Fast to Target
            currentSteering += delta * steeringAcceleration;
        }
        else if(targetSteering < 0f && (currentSteering > targetSteering))
        {
            // Fast to Target
            currentSteering -= delta * steeringAcceleration;
        }
        else
        {
            // Slow back to Target
            currentSteering *= selfCenteringFactor;
        }
        Mathf.Clamp(currentSteering, -1, 1);
        frontLeft.steerAngle = maxSteeringAngle * currentSteering;
        frontRight.steerAngle = maxSteeringAngle * currentSteering;
    }
    
    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is DriveControllEvent driveChange)
        {
            acceleration = driveChange.Accelerate;
            brakeStrength = driveChange.Brake;
            targetSteering = driveChange.Steer;
            if(showDebugLog)
                Debug.Log("Driver: Got new Acceleration and Steering! Yay!" );
        }
    }
}
