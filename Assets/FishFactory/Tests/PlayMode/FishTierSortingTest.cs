using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

public class FishTierSortingTest
{
    private const string TestSceneName = "FishTierSortingTests";
    private GameObject Tier1Fish;
    private GameObject Tier1Fish2;
    private GameObject SortingTrigger;
    private GameObject SortingButton;
    private FishSortingTrigger FishSortingTriggerScript;
    private FishSortingButton FishSortingButtonScript;
    private GameManager gameManager;
    private GameObject audioManager;

    // A Test behaves as an ordinary method
    [SetUp]
    public void OneTimeSetUp()
    {
       // Create a new test scene
       SceneManager.CreateScene(TestSceneName);

       // Load and instantiate the prefab
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       
       // Create a tier 1 fish
       Tier1Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Tier1Fish.transform.position = new Vector3(10, 10, 10);
       Tier1Fish.GetComponent<FactoryFishState>().fishTier = FactoryFishState.Tier.Tier1;
       
       // Create a tier 2 fish
       Tier1Fish2 = UnityEngine.Object.Instantiate(fishPrefab);
       Tier1Fish2.transform.position = new Vector3(10, 20, 10);
       Tier1Fish2.GetComponent<FactoryFishState>().fishTier = FactoryFishState.Tier.Tier1;

       // Create the sorting trigger box
       SortingTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
       SortingTrigger.transform.position = new Vector3(0, 0, 0);
       BoxCollider SorterCollider = SortingTrigger.AddComponent<BoxCollider>();
       SorterCollider.isTrigger = true;
       FishSortingTriggerScript = SortingTrigger.AddComponent<FishSortingTrigger>();

       // Add the sorting button object to the scene 
       SortingButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
       SortingButton.transform.position = new Vector3(100, 100, 100);
       FishSortingButtonScript = SortingButton.AddComponent<FishSortingButton>();

       FishSortingTriggerScript._tierManager = FishSortingButtonScript;

       // Create a new GameObject and add the GameManager component to it
        var gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();
        // Load the sound effects
        var audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
        audioManager = UnityEngine.Object.Instantiate(audioManagerPrefab);
        gameManager.SoundEffects = Resources.LoadAll<AudioClip>("Sounds");
        gameManager.AudioManager = audioManager;
        //create an audio listener
        var listener = new GameObject("AudioListener");
        listener.AddComponent<AudioListener>();
        //configure hand objects
        gameManager.LeftHandGameObj = new GameObject();
        gameManager.RightHandGameObj = new GameObject();
    }

    [UnityTest]
    public IEnumerator GiveIncorrectOnIncorrectTier()
    {
        FishSortingButtonScript.SetTier(2);
        yield return new WaitForSeconds(0.1f);
        Assert.AreNotEqual(
            FishSortingButton.FishTier.Tier1,
            FishSortingButtonScript.CurrentTier,
            "The fish tier should not be 1"
        );

        Tier1Fish.transform.position = SortingTrigger.transform.position + Vector3.up; // Set the fish position to be on top of the sorting box
        yield return new WaitForSeconds(2);

        Assert.AreEqual(gameManager.SoundEffects[1], gameManager.AudioManager.GetComponent<AudioSource>().clip);
    }

    [UnityTest]
    public IEnumerator GiveCorrectOnCorrectTier()
    {
        // Test if the right tier is set
        FishSortingButtonScript.SetTier(1);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(
            FishSortingButton.FishTier.Tier1,
            FishSortingButtonScript.CurrentTier,
            "The fish tier should be set to 1"
        );

        Tier1Fish.transform.position = SortingTrigger.transform.position + Vector3.up; // Set the fish position to be on top of the sorting box
        yield return new WaitForSeconds(2);

        Assert.AreEqual(gameManager.SoundEffects[0], gameManager.AudioManager.GetComponent<AudioSource>().clip);

    }

    

    [TearDown]
    public void OneTimeTearDown()
    {
        UnityEngine.Object.Destroy(Tier1Fish);
        UnityEngine.Object.Destroy(Tier1Fish2);
        UnityEngine.Object.Destroy(SortingTrigger);
        UnityEngine.Object.Destroy(audioManager);
        UnityEngine.Object.Destroy(gameManager);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }

}
