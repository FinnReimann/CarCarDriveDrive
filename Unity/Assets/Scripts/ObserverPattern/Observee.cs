using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observee
{
    public void NotifyObservers(CCDDEvents e);

}

public class ObserveeMonoBehaviour : MonoBehaviour, Observee {
    public void NotifyObservers(CCDDEvents e)
    {
        Debug.Log("das scheint zu gehen");
    }
}