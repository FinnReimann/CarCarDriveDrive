using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObserveeMonoBehaviour : MonoBehaviour {
    
    private static List<Observer> observers = new List<Observer>();

    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

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
}