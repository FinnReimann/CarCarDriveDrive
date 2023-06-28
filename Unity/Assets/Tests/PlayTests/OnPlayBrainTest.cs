using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class OnPlayBrainTest
{
    private GameObject car;
    [SetUp]
    public void SetUp()
    {
        car = new GameObject();
        car.AddComponent<Configuration>();
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestAccelerate()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new NavigationEvent(10f));
        brain.CCDDUpdate(new SpeedChangeEvent(5f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.Greater(driveControllEvent.Accelerate, 0);
                    Assert.LessOrEqual(driveControllEvent.Accelerate, 1);
                    Assert.AreEqual(driveControllEvent.Brake, 0);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestHeavyAccelerate()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new NavigationEvent(100000f));
        brain.CCDDUpdate(new SpeedChangeEvent(5f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.Greater(driveControllEvent.Accelerate, 0);
                    Assert.LessOrEqual(driveControllEvent.Accelerate, 1);
                    Assert.AreEqual(driveControllEvent.Brake, 0);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestBreaking()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new NavigationEvent(10f));
        brain.CCDDUpdate(new SpeedChangeEvent(15f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.AreEqual(driveControllEvent.Accelerate, 0);
                    Assert.Greater(driveControllEvent.Brake, 0);
                    Assert.LessOrEqual(driveControllEvent.Brake, 1);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestHeavyBreaking()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new NavigationEvent(0f));
        brain.CCDDUpdate(new SpeedChangeEvent(100000f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.AreEqual(driveControllEvent.Accelerate, 0);
                    Assert.Greater(driveControllEvent.Brake, 0);
                    Assert.LessOrEqual(driveControllEvent.Brake, 1);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestStandStill()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new NavigationEvent(0f));
        brain.CCDDUpdate(new SpeedChangeEvent(0f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.AreEqual(0f,driveControllEvent.Accelerate);
                    Assert.AreEqual(1f,driveControllEvent.Brake);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestLeftSteering()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new PressureChangeEvent(-1f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.Less(driveControllEvent.Steer,0);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
    
    [UnityTest]
    public IEnumerator OnPlayBrainTestRightSteering()
    {
        //Arrange
        EventTestObserver testObserver = new EventTestObserver();
        Brain brain = car.AddComponent<Brain>();
        //Act
        brain.CCDDUpdate(new PressureChangeEvent(1f));
        brain.Attach(testObserver);
        yield return null;
        //Assert
        List<CCDDEvents> receivedEvents = testObserver.GetReceivedEvents();
        if (receivedEvents.Count > 0)
        {
            Assert.IsTrue(receivedEvents.Any(item => item is DriveControllEvent));
            foreach (var events in receivedEvents)
            {
                if(events is DriveControllEvent driveControllEvent)
                {
                    Assert.Greater(driveControllEvent.Steer,0f);
                }
            }
        }
        else
        {
            Assert.Fail("No Event Received");
        }
    }
}
