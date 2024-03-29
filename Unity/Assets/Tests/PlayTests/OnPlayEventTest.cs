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
   
    [SetUp]
    public void SetUp()
    {
        car = new GameObject();
        car.AddComponent<Configuration>();
        car.AddComponent<Navigator>();
    }
    // A UnityTest behaves like a coroutine in Play Mode
    [UnityTest]
    public IEnumerator TestSpeedEvent()
    {
        //Arrange
        Speedometer speedometer = car.AddComponent<Speedometer>();
        EventTestObserver testObserver = new EventTestObserver();
        speedometer.Attach(testObserver);
        
        //Act
        CCDDEvents speedEvent = new SpeedChangeEvent(1f,10f);
        speedometer.NotifyObservers(speedEvent);
        yield return null;
        
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
    
    [UnityTest]
    public IEnumerator TestDriveEvent()
    {
        //Arrange
        car.AddComponent<Speedometer>();
        car.AddComponent<SidePressureCalculator>();
        Brain brain = car.AddComponent<Brain>();
        EventTestObserver testObserver = new EventTestObserver();
        brain.Attach(testObserver);
        
        //Act
        CCDDEvents driveControllEvent = new DriveControllEvent(0.5f,0.8f,-0.2f);
        brain.NotifyObservers(driveControllEvent);
        yield return null;
        
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.Contains(driveControllEvent,receivedEvents);
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator TestPressureEvent()
    {
        //Arrange
        SidePressureCalculator sidePressureCalculator = car.AddComponent<SidePressureCalculator>();
        EventTestObserver testObserver = new EventTestObserver();
        sidePressureCalculator.Attach(testObserver);
        
        //Act
        CCDDEvents pressureEvent = new PressureChangeEvent(1f);
        sidePressureCalculator.NotifyObservers(pressureEvent);
        yield return null;
        
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.Contains(pressureEvent,receivedEvents);
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
}
