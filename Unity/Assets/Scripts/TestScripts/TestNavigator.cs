using System;
using UnityEngine;

public class TestNavigator : Navigator
{
    [SerializeField] private int layerIndex;
    [SerializeField] private int CheckpointCount;
    [SerializeField] private bool showDebug = true;
    [SerializeField] private bool showConstantDebug = false;
    [SerializeField] private float speedIncreasingPerRound = 1;
    private int enteredCheckpoints;
    private int drivenRounds;

    protected override void Update()
    {
        GetConfig();
        if (showConstantDebug)
            Debug.Log("The Car is in Round: " + drivenRounds + " and passed through " + enteredCheckpoints + " Checkpoints. Current Speed is " + lastSpeed);
        
        if (lastSpeed != targetSpeed)
        {
            lastSpeed = targetSpeed;
            NotifyObservers(new NavigationEvent(targetSpeed));
        }
    }
    
    private void OnEnable()
    {
        gameObject.AddComponent<BoxCollider>();
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerIndex) return;
        CheckPointTest checkpoint = other.GetComponent<CheckPointTest>();
        if(showDebug)
            Debug.Log("Enter Checkpoint: " + checkpoint.CheckPointNumber);
        if(enteredCheckpoints+1 == checkpoint.CheckPointNumber)
            enteredCheckpoints++;
        if (enteredCheckpoints == CheckpointCount)
        {
            if(showDebug)
                Debug.Log("Complete Round " + drivenRounds + " with speed of" + targetSpeed);
            enteredCheckpoints = 0;
            drivenRounds++;
            targetSpeed += speedIncreasingPerRound;
        }
    }
}