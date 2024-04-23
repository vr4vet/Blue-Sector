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
    private GameObject knife;
    private DiscardBadFish discardScript;
    private const string TestSceneName = "DiscardFishTestScene";

    [SetUp]
    public void SetUp()
    {
        // Load and instantiate the prefabs
        var discardBoxPrefab = Resources.Load<GameObject>("Prefabs/FishDiscardBox");
        var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/TempFish");
        fish = Object.Instantiate(fishPrefab);
        discardBox = Object.Instantiate(discardBoxPrefab);

        // Load, instantiate and setup the knife prefab
        var knife = Resources.Load<GameObject>("Prefabs/Knife/FishKnife");
        knife = Object.Instantiate(knife);
        var knifeState = knife.GetComponent<KnifeState>();
        if (knifeState == null)
        {
            Debug.LogError("KnifeState component not found on the knife prefab.");
            return;
        }
        //configure the knife state script
        var bladePrefab = Resources.Load<GameObject>("Prefabs/Knife/FishKnifeBlade");
        GameObject regularKnife = Object.Instantiate(bladePrefab);
        knifeState.RegularKnife = regularKnife;
        knifeState.ChippedKnife = regularKnife;


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

        yield return new WaitForSeconds(1);

        Assert.AreEqual(
            1,
            discardScript.NrFishDiscarded,
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
            discardScript.NrFishDiscarded,
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
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed
        fish.transform.position = discardBox.transform.position + Vector3.up; // Set the fish position to be on top of the discard box
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed

        Assert.AreEqual(
            1,
            discardScript.NrFishDiscarded,
            "The fish should not be discarded again after it has been discarded once"
        );
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(fish);
        Object.Destroy(discardBox);
    }
}
