using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class StunFishTest
{
    private const string TestSceneName = "StunFishTest";
    private GameObject Fish;
    private GameObject StunBox;
    private StunFish StunFishScript;

    // Can be removed once issue #521 is complete
    private GameManager gameManager;
    private GameObject listener;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject audioManager;
    //-------------------------------------------

    // A Test behaves as an ordinary method
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Create a new test scene
        SceneManager.CreateScene(TestSceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

       // Load and instantiate the fish prefab
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");

       // Create a fish
       Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Fish.transform.position = new Vector3(10, 10, 10);
       Fish.GetComponent<FactoryFishState>().Stunned = false;

       //Create a stun box and add the StunFish script and a collider
       StunBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
       StunBox.transform.position = new Vector3(0, 0, 0);
       BoxCollider stunTrigger = StunBox.AddComponent<BoxCollider>();
       stunTrigger.isTrigger = true;
       StunFishScript = StunBox.AddComponent<StunFish>();
       StunBox.AddComponent<AudioSource>();

        // Can be removed once issue #521 is complete

       // Create a new GameObject and add the GameManager component to it
        var gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Load the sound effects
        var audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
        audioManager = UnityEngine.Object.Instantiate(audioManagerPrefab);
        gameManager.SoundEffects = Resources.LoadAll<AudioClip>("Sounds");
        gameManager.AudioManager = audioManager;

        //create an audio listener
        listener = new GameObject("AudioListener");
        listener.AddComponent<AudioListener>();
        
        //configure hand objects
        leftHand = new GameObject("Green Gloves Right");
        rightHand = new GameObject("Green Gloves Left");
        gameManager.LeftHandGameObj = leftHand;
        gameManager.RightHandGameObj = rightHand;
        //--------------------------------------------------
    }

    /// <summary>
    /// Tests to ensure that an alive fish gets stunned once in contact with the stun box.
    /// </summary>
    [UnityTest]
    public IEnumerator StunAliveFish()
    {
        Fish.transform.position = StunBox.transform.position + Vector3.up; // Set the fish position to be on top of the stun box
        yield return new WaitForSeconds(1); // wait for 1 second to let the the coolision happen
        Assert.AreEqual(
            true,
            Fish.GetComponent<FactoryFishState>().Stunned,
            "Fish was not stunned"
        );
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        UnityEngine.Object.DestroyImmediate(Fish);
        UnityEngine.Object.DestroyImmediate(StunBox);
        // Can be removed once issue #521 is complete
        UnityEngine.Object.DestroyImmediate(gameManager);
        UnityEngine.Object.DestroyImmediate(audioManager);
        UnityEngine.Object.DestroyImmediate(listener);
        UnityEngine.Object.DestroyImmediate(leftHand);
        UnityEngine.Object.DestroyImmediate(rightHand);
        //---------------------------------------------
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
