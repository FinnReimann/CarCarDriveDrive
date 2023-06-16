using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AccChangeEvent : ChangeEvents
{
      public AccChangeEvent(float brainAcc)
      {
            SceneAcceleration = brainAcc;
      }
      public float SceneAcceleration {set; get; }
}