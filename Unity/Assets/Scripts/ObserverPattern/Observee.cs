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