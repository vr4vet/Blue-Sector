using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class DiscardKnifeTest
{
    //TODO: Expand test to test knife respawning correctly
    
    private const string TestSceneName = "DiscardKnifeTest";
    private GameObject Knife;
    private GameObject Fish;
    private GameObject DiscardBox;
    private DiscardKnife DiscardKnifeScript;

    // Can be removed once issue #521 is complete
    private GameManager gameManager;
    private GameObject listener;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject audioManager;
    //----------------------------------------------

    // A Test behaves as an ordinary method
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Create a new test scene
        SceneManager.CreateScene(TestSceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

       // Create the knife
       var knifePrefab = Resources.Load<GameObject>("Prefabs/Knife/FishKnife");
       Knife = UnityEngine.Object.Instantiate(knifePrefab);
       Knife.transform.position = new Vector3(10, 20, 10);
       
       // Create a fish
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Fish.transform.position = new Vector3(10, 10, 10);
       Fish.GetComponent<FactoryFishState>().Stunned = false;

       //Create a stun box and add the StunFish script and a collider
       DiscardBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
       DiscardBox.transform.position = new Vector3(0, 0, 0);
       BoxCollider discardTrigger = DiscardBox.AddComponent<BoxCollider>();
       discardTrigger.isTrigger = true;
       DiscardKnifeScript = DiscardBox.AddComponent<DiscardKnife>();
        
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
        //--------------------------------------------------------
    }

    /// <summary>
    /// Tests to ensure that the knife gets discarded.
    /// </summary>
    [UnityTest]
    public IEnumerator DiscardsKnife()
    {
        Knife.transform.position = DiscardBox.transform.position + Vector3.up; // Set the knife position to be on top of the discard box
        yield return new WaitForSeconds(1); // wait for 1 second to let the the coolision happen

        if (Knife == null){
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }

    }
    /// <summary>
    /// Tests to ensure that the discard box does not discard a non knife object.
    /// </summary>
    [UnityTest]
    public IEnumerator DoesNotDiscadNonKnifeObjects()
    {
        Fish.transform.position = DiscardBox.transform.position + Vector3.up; // Set the fish position to be on top of the discard box
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed
       
        if (Fish == null){
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }

    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        UnityEngine.Object.DestroyImmediate(Knife);
        UnityEngine.Object.DestroyImmediate(Fish);
        UnityEngine.Object.DestroyImmediate(DiscardBox);
        // Can be removed once issue #521 is complete
        UnityEngine.Object.DestroyImmediate(gameManager);
        UnityEngine.Object.DestroyImmediate(audioManager);
        UnityEngine.Object.DestroyImmediate(listener);
        UnityEngine.Object.DestroyImmediate(leftHand);
        UnityEngine.Object.DestroyImmediate(rightHand);
        //---------------------------------------
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
