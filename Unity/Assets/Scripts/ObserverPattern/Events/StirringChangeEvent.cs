using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StirringChangeEvent : ChangeEvents 
{
    public StirringChangeEvent(float brainStirring)
    {
        SceneStirring = brainStirring;
    }
    
    public float SceneStirring {set; get; }
}
