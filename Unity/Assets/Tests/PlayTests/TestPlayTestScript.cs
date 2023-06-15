using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestObserver : Observer
{
    private List<CCDDEvents> receivedEvents = new List<CCDDEvents>();

    public void CCDDUpdate(CCDDEvents e)
    {
        receivedEvents.Add(e);
    }

    public List<CCDDEvents> GetReceivedEvents()
    {
        return receivedEvents;
    }
}

public class TestPlayTestScript
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSpeedEvent()
    {
        GameObject gameObject = new GameObject();
        Tacho tacho = gameObject.AddComponent<Tacho>();
        TestObserver testObserver = new TestObserver();
        ObserveeMonoBehaviour observeeMonoBehaviour = new ObserveeMonoBehaviour();
        observeeMonoBehaviour.Attach(testObserver);

        SpeedChangeEvent expectedEvent = new SpeedChangeEvent(1f,10f);
        CCDDEvents speedEvent = new SpeedChangeEvent(1f,10f);
        tacho.NotifyObservers(speedEvent);

        yield return new WaitForSeconds(1f);
        
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        
        if (receivedEvents.Count > 0)
        {
            CCDDEvents receivedEvent = receivedEvents[0];
            
            if (receivedEvent is SpeedChangeEvent receivedSpeedChangeEvent)
            {
                Debug.Log(receivedSpeedChangeEvent.CurrentSpeed);
                Assert.AreEqual(expectedEvent.CurrentSpeed, receivedSpeedChangeEvent.CurrentSpeed);
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
}
