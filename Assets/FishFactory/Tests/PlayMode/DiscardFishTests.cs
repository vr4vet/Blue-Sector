using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DiscardFishTests
{
    // Test if the fish is discarded when it enters the trigger
    
    [UnityTest]
    public IEnumerator DiscardFish()
    {
        var gameObject = new GameObject();
        var discardFish = gameObject.AddComponent<DiscardBadFish>();
        yield return new WaitForSeconds(3); // wait for 3 seconds temporarily to let the test
        Assert.AreEqual(0, discardFish.NrFishDiscarded);
    }

    // Test if the fish is deleted when the treshold is reached
    [UnityTest]
    public IEnumerator DeleteFishAtTreshold()
    {
        var gameObject = new GameObject();
        var discardFish = gameObject.AddComponent<DiscardBadFish>();
        yield return new WaitForSeconds(3); // wait for 3 seconds temporarily to let the test
    }
}
