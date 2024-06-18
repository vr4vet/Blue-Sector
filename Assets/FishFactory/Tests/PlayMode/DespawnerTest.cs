using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

public class DespawnerTest
{
    private const string TestSceneName = "DespawnerTests";
    private GameObject Fish;
    private GameObject FishWithMetal;
    private GameObject Knife;
    private GameObject Despawner;
    private Despawner DespawnerScript;
    private GameManager gameManager;
    private GameObject listener;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject audioManager;

    [SetUp]
    public void SetUp()
    {
       // create a new test scene
       SceneManager.CreateScene(TestSceneName);
       SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

       // Load and instantiate the fish prefab
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       // Create a regular fish
       Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Fish.transform.position = new Vector3(10, 10, 10);

       // Create a fish with metal in it
       FishWithMetal = UnityEngine.Object.Instantiate(fishPrefab);
       FishWithMetal.transform.position = new Vector3(10, 10, 10);
       FishWithMetal.GetComponent<FactoryFishState>().ContainsMetal = true;

       // Create the knife
       var knifePrefab = Resources.Load<GameObject>("Prefabs/Knife/FishKnife");
       Knife = UnityEngine.Object.Instantiate(knifePrefab);
       Knife.transform.position = new Vector3(10, 20, 10);

       // Create the despawner cube and add the script and a collider
       Despawner = GameObject.CreatePrimitive(PrimitiveType.Cube);
       Despawner.transform.position = new Vector3(0, 0, 0);
       BoxCollider DespawnerTrigger = Despawner.AddComponent<BoxCollider>();
       DespawnerTrigger.isTrigger = true;
       DespawnerScript = Despawner.AddComponent<Despawner>();

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
    /// Tests to ensure that the Despawner script play an incorrect sound when a fish containing metal gets despawned.
    /// </summary>
    [UnityTest]
    public IEnumerator ASoundIsPlayedWhenFishWithMetalDespawn()
    {
        FishWithMetal.transform.position = Despawner.transform.position + Vector3.up; // Set the knife position to be on top of the despawner box
        yield return new WaitForSeconds(1); // wait for 1 second to let the the coolision happen
        Assert.AreEqual(gameManager.SoundEffects[1], gameManager.AudioManager.GetComponent<AudioSource>().clip);
        
        if (FishWithMetal == null){
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// Tests to ensure that the Despawner despawn regular fish.
    /// </summary>
    [UnityTest]
    public IEnumerator DespawnFish()
    {
        Fish.transform.position = Despawner.transform.position + Vector3.up; // Set the fish position to be on top of the despawner box
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed
       
        if (Fish == null){
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// Tests to ensure that the Despawner script does not despawn non-fish objects.
    /// </summary>
    [UnityTest]
    public IEnumerator DoesNotDespawnNonFishObjects()
    {
        Knife.transform.position = Despawner.transform.position + Vector3.up; // Set the knife position to be on top of the despawner box
        yield return new WaitForSeconds(1); // wait for 1 second to let the the coolision happen

        if (Knife == null){
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(Fish);
        UnityEngine.Object.Destroy(FishWithMetal);
        UnityEngine.Object.Destroy(Knife);
        UnityEngine.Object.Destroy(Despawner);
        UnityEngine.Object.Destroy(gameManager);
        UnityEngine.Object.Destroy(audioManager);
        UnityEngine.Object.Destroy(listener);
        UnityEngine.Object.Destroy(leftHand);
        UnityEngine.Object.Destroy(rightHand);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
