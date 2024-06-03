using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class DiscardFishTests
{
    private GameObject discardBox;
    private GameObject fish;
    private DiscardBadFish discardScript;
    private const string TestSceneName = "DiscardFishTestScene";

    [SetUp]
    public void SetUp()
    {
        // create a new test scene
        SceneManager.CreateScene(TestSceneName);
        // Load and instantiate the prefabs
        var discardBoxPrefab = Resources.Load<GameObject>("Prefabs/FishDiscardBox");
        var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactoryBadFish");
        fish = Object.Instantiate(fishPrefab);
        fish.GetComponent<FactoryFishState>().CurrentState = FactoryFishState.State.BadQuality;
        discardBox = Object.Instantiate(discardBoxPrefab);

        // get the DiscardBadFish script
        discardScript = discardBox.GetComponentInChildren<DiscardBadFish>();
    }

    /// <summary>
    /// Tests to ensure that the DiscardBadFish script correctly discards fish objects when they collide with the discard box.
    /// </summary>
    [UnityTest]
    public IEnumerator DiscardFish()
    {
        fish.transform.position = discardBox.transform.position + Vector3.up; // set the fish position to be on top of the discard box

        yield return new WaitForSeconds(3);

        Assert.AreEqual(
            1,
            discardScript.DiscardedFishCount,
            "The fish should be discarded and the counter should be increased"
        );
    }

    /// <summary>
    /// Tests to ensure that the DiscardBadFish script does not count non-fish objects towards the discarded fish count.
    /// </summary>
    [UnityTest]
    public IEnumerator DoesNotDiscardNonFishObjects()
    {
        //create a game object with a box collider and rigidbody to simulate a non-fish object
        var gameObject = new GameObject();
        gameObject.AddComponent<BoxCollider>();
        gameObject.AddComponent<Rigidbody>();
        gameObject.transform.position = discardBox.transform.position + Vector3.up;

        yield return new WaitForSeconds(1); // wait for 3 seconds temporarily to let the the coolision happen

        Assert.AreEqual(
            0,
            discardScript.DiscardedFishCount,
            "The non-fish object should not count towards discarded fish"
        );
    }

    /// <summary>
    /// Tests to ensure that the DiscardBadFish script does not discard a fish object that has already been discarded.
    /// </summary>
    [UnityTest]
    public IEnumerator DoesNotDiscardAgain()
    {
        fish.transform.position = discardBox.transform.position + Vector3.up; // Set the fish position to be on top of the discard box
        yield return new WaitForSeconds(3); // Wait for the collision and potential destruction to be processed
        fish.transform.position = discardBox.transform.position + Vector3.up; // Set the fish position to be on top of the discard box
        yield return new WaitForSeconds(3); // Wait for the collision and potential destruction to be processed

        Assert.AreEqual(
            1,
            discardScript.DiscardedFishCount,
            "The fish should not be discarded again after it has been discarded once"
        );
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(fish);
        Object.Destroy(discardBox);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
