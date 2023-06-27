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
        if (showConstantDebug)
            Debug.Log("The Car is in Round: " + drivenRounds + " and passed to " + enteredCheckpoints + " Checkpoints");
        
        if (lastSpeed != targetSpeed)
        {
            lastSpeed = targetSpeed;
            NotifyObservers(new NavigationEvent(targetSpeed));
        }
    }
    
    private void OnEnable()
    {
        gameObject.AddComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerIndex) return;
        if(showDebug)
            Debug.Log("Enter: " + other.name);
        enteredCheckpoints++;
        if (enteredCheckpoints == CheckpointCount)
        {
            enteredCheckpoints = 0;
            drivenRounds++;
            targetSpeed += speedIncreasingPerRound;
        }




    }
}