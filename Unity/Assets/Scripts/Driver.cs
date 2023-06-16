using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour, Observer
{
    private float sceneAcc;

    private float sceneStirring;

    [SerializeField] private GameObject car;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //apply Scene Acceleration and Stirring to Car
    }
    
    public void CCDDUpdate(CCDDEvents e)
    {
        if (e is AccChangeEvent AccChange)
        {
            sceneAcc = AccChange.SceneAcceleration;
        }
        
        if (e is StirringChangeEvent StirringChange)
        {
            sceneStirring = StirringChange.SceneStirring;
        }

    }
}
