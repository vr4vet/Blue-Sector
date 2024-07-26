using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework.Constraints;
using UnityEditor.PackageManager;

public class FishTierSortingTest
{
    private const string TestSceneName = "FishTierSortingTests";
    private GameObject Tier1Fish;
    private GameObject SortingTrigger;
    private GameObject SortingButton;
    private FishSortingTrigger FishSortingTriggerScript;
    private FishSortingButton FishSortingButtonScript;
    private GameManager gameManager;
    private GameObject audioManager;
    private GameObject listener;
    private GameObject leftHand;
    private GameObject rightHand;

    // A Test behaves as an ordinary method
    [SetUp]
    public void SetUp()
    {
       // Create a new test scene
       SceneManager.CreateScene(TestSceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

       // Load and instantiate the fish prefab
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       
       // Create a tier 1 fish
       Tier1Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Tier1Fish.transform.position = new Vector3(10, 10, 10);
       Tier1Fish.GetComponent<FactoryFishState>().fishTier = FactoryFishState.Tier.Tier1;
       
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
        listener = new GameObject("AudioListener");
        listener.AddComponent<AudioListener>();

        //configure hand objects
        leftHand = new GameObject("Green Gloves Right");
        rightHand = new GameObject("Green Gloves Left");
        gameManager.LeftHandGameObj = leftHand;
        gameManager.RightHandGameObj = rightHand;
    }

    /// <summary>
    /// Tests to ensure that the button sets the correct tier and the "correct" sound is played when the correct fish is sorted.
    /// </summary>
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
        yield return new WaitForSeconds(1);

        Assert.AreEqual(gameManager.SoundEffects[0], gameManager.AudioManager.GetComponent<AudioSource>().clip);
    }

    /// <summary>
    /// Tests to ensure that the button sets the correct tier and the "incorrect" sound is played when the wrong fish is sorted.
    /// </summary>
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
        yield return new WaitForSeconds(1);
        Assert.AreEqual(gameManager.SoundEffects[1], gameManager.AudioManager.GetComponent<AudioSource>().clip);
    }

    /// <summary>
    /// Tests to ensure that the button sets the correct tier and the "incorrect" sound is played when the wrong fish is sorted.
    /// </summary>
    [UnityTest]
    public IEnumerator GiveErrorOnTooHighTier()
    {
        var tiers = Enum.GetValues(typeof(FishSortingButton.FishTier));
        for (int i = 1; i < 5; i++)
        {
            if (i == 4)
            {   
                LogAssert.ignoreFailingMessages = true;
                FishSortingButtonScript.SetTier(i);
                LogAssert.Expect(LogType.Error, "Invalid tier number");
            }
            else
            {
                FishSortingButtonScript.SetTier(i);
                Assert.AreEqual(
                    tiers.GetValue(i-1),
                    FishSortingButtonScript.CurrentTier,
                    "The fish tier should be set to " + i
                );
            }
        }
        yield return new WaitForSeconds(0.1f);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(Tier1Fish);
        UnityEngine.Object.Destroy(SortingTrigger);
        UnityEngine.Object.Destroy(SortingButton);
        UnityEngine.Object.Destroy(gameManager);
        UnityEngine.Object.Destroy(audioManager);
        UnityEngine.Object.Destroy(listener);
        UnityEngine.Object.Destroy(leftHand);
        UnityEngine.Object.Destroy(rightHand);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
