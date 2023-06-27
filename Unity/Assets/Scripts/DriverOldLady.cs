using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverOldLady : MonoBehaviour, Observer
{
    private Brain brainToWatch;
    private TestObservee _observee;
    private GameObject car;
    
    [SerializeField] private bool showDebugLog = false;
    
    //Acceleration
    private float acceleration;
    private float accSpeed;
    private float braking;
    private float brakeSpeed;
    private float velocity;
    [SerializeField]private float _MaxAcc = 1.0f;
    [SerializeField]private float _MinAcc = 0; //needed when implementing driving backwards
    [SerializeField]private float _MaxBrake = 1.0f;
    [SerializeField]private float maxSteeringAngle = 60.0f;

    //Steering
    private float steeringSpeed;

    //for testing
    [SerializeField]private float _MaxVelocity = 50.0f;
    //[SerializeField] private float TestSteeringSpeed;
    //[SerializeField] private float TestAccSpeed;

    private void Awake()
    {
        car = GameObject.FindGameObjectWithTag("Vehicle");
        _observee = car.GetComponent<TestObservee>();
        brainToWatch = car.GetComponentInChildren<Brain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Nachrichten vom Brain abonnieren
        brainToWatch.Attach(this);
        //_observee.Attach(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //apply Scene Acceleration and Stirring to Car
        
        //**************
        //With rigidbody
        //**************
        //car.GetComponent<Rigidbody>().AddForce(car.transform.forward * (acceleration * TestAccSpeed * Time.deltaTime));
        //Debug.Log("Car Velocity:" + (car.GetComponent<Rigidbody>().velocity));


        //*****************
        //Without rigidbody & with stirring
        //*****************
        //steering
        if(steeringSpeed != 0 && velocity != 0)
            car.transform.Rotate(Vector3.up, maxSteeringAngle * steeringSpeed * Time.fixedDeltaTime *velocity/_MaxVelocity);
        //*(1-(velocity/_MaxVelocity)/2) -> dont steer max possible if speed is max.

        //acceleration
        if (accSpeed != 0)
        {
            acceleration += accSpeed;

            if (acceleration > _MaxAcc)
                acceleration = _MaxAcc;
            else if (acceleration < _MinAcc)
                acceleration = _MinAcc;

            velocity += acceleration;
        }


        //braking
        if (brakeSpeed != 0)
        {
            braking += brakeSpeed;

            if (braking > _MaxBrake)
                braking = _MaxBrake;

            if (velocity > 0)
            {
                velocity -= braking;
            }
            else
            {
                velocity = 0;
            }
            
        }


        //Testing velocity control
        if (velocity > _MaxVelocity)
            velocity = _MaxVelocity;
        else if (velocity < -_MaxVelocity)
            velocity = -_MaxVelocity;

        car.transform.Translate(Vector3.forward * velocity * Time.fixedDeltaTime);
        if(showDebugLog)
            Debug.Log("Car Velocity:" + velocity);
    }

    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is DriveControllEvent driveChange)
        {
            accSpeed = driveChange.Accelerate;
            brakeSpeed = driveChange.Break;
            steeringSpeed = driveChange.Steer;
            if(showDebugLog)
                Debug.Log("Driver: Got new Acceleration and Steering! Yay!" );
        }
    }
}
