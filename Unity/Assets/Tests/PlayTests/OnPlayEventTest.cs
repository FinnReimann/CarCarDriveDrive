using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EventTestObserver : Observer
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

[TestFixture]
public class TestPlayTestScript
{
    private GameObject car;
    private GameObject road;
    private GameObject checkpoints;
    
    [SetUp]
    public void SetUp()
    {
        road = Resources.Load<GameObject>("RoadExample");
        car = Resources.Load<GameObject>("Car");
        checkpoints = Resources.Load<GameObject>("Checkpoints");
        Object.Instantiate(road);
        Object.Instantiate(car);
        Object.Instantiate(checkpoints);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestSpeedEvent()
    {
        //Arrange
        Tacho tacho = car.GetComponentInChildren<Tacho>();
        EventTestObserver testObserver = new EventTestObserver();
        tacho.Attach(testObserver);
        
        //Act
        CCDDEvents speedEvent = new SpeedChangeEvent(1f,10f);
        tacho.NotifyObservers(speedEvent);
        yield return new WaitForSeconds(60f);
        
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.Contains(speedEvent,receivedEvents);
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
}
