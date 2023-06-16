using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*public class ObserveeMonoBehaviour : MonoBehaviour {
    
    private static List<Observer> observers = new List<Observer>();

    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

public class ObserveeMonoBehaviour : MonoBehaviour{
    public void NotifyObservers(CCDDEvents e)
    {
        foreach (Observer observer in observers)
        {
            observer.CCDDUpdate(e);
        }
        if (e is SpeedChangeEvent)
        {
            Debug.Log("Observee: Ein Speed Change Event wurde nicht weitergeleitet, weil die Methode nicht implementiert ist, aber bis hier funktioniert alles noch. Nice!");
        }
        if (e is DriveControllEvent)
        {
            Debug.Log("Observee :Ein Drive Controll Event wurde nicht weitergeleitet, weil die Methode nicht implementiert ist, aber bis hier funktioniert alles noch. Nice!");
        }
    }
}*/

public class ObserveeMonoBehaviour : MonoBehaviour
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
        Debug.Log("Subject: Notifying Observers...");
        foreach (Observer observer in observers)
        {
            observer.CCDDUpdate(e);
        }
        if (e is SpeedChangeEvent)
        {
            Debug.Log("Observee: Ein Speed Change Event wurde nicht weitergeleitet, weil die Methode nicht implementiert ist, aber bis hier funktioniert alles noch. Nice!");
        }
        if (e is DriveControllEvent)
        {
            Debug.Log("Observee :Ein Drive Controll Event wurde nicht weitergeleitet, weil die Methode nicht implementiert ist, aber bis hier funktioniert alles noch. Nice!");
        }
    }
}