using System;
using UnityEngine;

public class TestNavigator : Navigator
{
    // This class is the basic navigator with added functionality to log the driven rounds and change speed
    
    // Number of the Checkpoint layer, in the example case its 7
    [SerializeField] private int layerIndex = 7;
    [SerializeField] private int CheckpointCount;
    [SerializeField] private bool showDebug = true;
    [SerializeField][Tooltip("Show Debug every frame")] private bool showConstantDebug = false;
    [SerializeField] private float speedIncreasingPerRound = 1;
    // Counter Varialbes
    private int enteredCheckpoints;
    private int drivenRounds;
    
    // Override to show the Debug log
    protected override void Update()
    {
        if (showConstantDebug)
            Debug.Log("The Car is in Round: " + drivenRounds + " and passed through " + enteredCheckpoints + " Checkpoints. Current Speed is " + lastSpeed);
        
        if (lastSpeed != targetSpeed)
        {
            lastSpeed = targetSpeed;
            NotifyObservers(new NavigationEvent(targetSpeed));
        }
    }
    
    // Create a rigidbody, to check if there is a collision with the checkpoints
    private void OnEnable()
    {
        gameObject.AddComponent<BoxCollider>();
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    // Check for Collision with Checkpoints
    private void OnTriggerEnter(Collider other)
    {
        // If the collided Object is not in the layer index (Checkpoints) -> return
        if (other.gameObject.layer != layerIndex) return;
        CheckPointTest checkpoint = other.GetComponent<CheckPointTest>();
        if(showDebug)
            Debug.Log("Enter Checkpoint: " + checkpoint.CheckPointNumber);
        // Check if it was the right checkpoint
        if(enteredCheckpoints+1 == checkpoint.CheckPointNumber)
            enteredCheckpoints++;
        // Increase round number
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