using System.Collections.Generic;
using UnityEngine;

public class ObserveeMonoBehaviour : MonoBehaviour, IObservee
{
    private List<Observer> observers = new List<Observer>();
    
    // Attach an observer to the subject.
    public void Attach(Observer observer)
    {
        this.observers.Add(observer);
        Debug.Log("Subject: Attached Observer");
    }

    // Detach an observer from the subject.
    public void Detach(Observer observer)
    {
        this.observers.Remove(observer);
        Debug.Log("Subject: Detached Observer");
    }

    // Notify all observers about an event.
    public void NotifyObservers(CCDDEvents e)
    {
        foreach (Observer observer in observers)
        {
            observer.CCDDUpdate(e);
        }
    }
}