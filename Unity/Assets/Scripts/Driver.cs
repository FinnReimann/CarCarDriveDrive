using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour, Observer
{
    private Brain brainToWatch;
    private TestObservee _observee;
    private GameObject car;
    
    //Acceleration
    private float acceleration;
    private float accSpeed;
    private float braking;
    private float brakeSpeed;
    private float velocity;
    [SerializeField]private float _MaxAcc = 1.0f;
    [SerializeField]private float _MinAcc = -1.0f;
    [SerializeField]private float _MaxBrake = 1.0f;
    [SerializeField]private float _MinBrake = -1.0f;

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
        brainToWatch = car.GetComponent<Brain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Nachrichten vom Brain abonnieren
        //brainToWatch.Attach(this);
        _observee.Attach(this);
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
        if(steeringSpeed != 0)
            car.transform.Rotate(Vector3.up, steeringSpeed * Time.deltaTime);

        //acceleration
        if (accSpeed != 0)
            acceleration += accSpeed;
        
        if (acceleration > _MaxAcc)
            acceleration = _MaxAcc;
        else if (acceleration < _MinAcc)
            acceleration = _MinAcc;

        velocity += acceleration;
        
       
        //braking
        if (brakeSpeed != 0)
            braking += brakeSpeed;
        
        if (braking > _MaxBrake)
            braking = _MaxBrake;
        else if (braking < _MinBrake)
            braking = _MinBrake;
        
        velocity -= braking;

        
        //Testing velocity control
        if (velocity > _MaxVelocity)
            velocity = _MaxVelocity;
        else if (velocity < -_MaxVelocity)
            velocity = -_MaxVelocity;

        car.transform.Translate(Vector3.forward * velocity * Time.deltaTime);
        //Debug.Log("Car Velocity:" + velocity);
        
    }


    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is DriveControllEvent driveChange)
        {
            accSpeed = driveChange.Accelerate;
            brakeSpeed = driveChange.Break;
            steeringSpeed = driveChange.Steer;
            //wird 2 mal aufgerufen??
            Debug.Log("Driver: Got new Acceleration and Steering! Yay!" );
        }
    }
}
