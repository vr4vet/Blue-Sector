using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class GameManagerTests
{
    private GameManager gameManager;
    private GameObject listener;
    private GameObject leftHand;
    private GameObject rightHand;
    private const string TestSceneName = "GameManagerTestScene";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Create a new test scene
        SceneManager.CreateScene(TestSceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

        // Create a new GameObject and add the GameManager component to it
        var gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Load the sound effects
        var audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
        var audioManager = Object.Instantiate(audioManagerPrefab);
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

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Object.Destroy(gameManager);
        Object.Destroy(listener);
        Object.Destroy(leftHand);
        Object.Destroy(rightHand);
        SceneManager.UnloadSceneAsync(TestSceneName);
        
    }

    /// <summary>
    /// Tests to ensure that the toggle of task work.
    /// </summary>
    [UnityTest]
    public IEnumerator ToggleTaskOnTest()
    {
        Assert.IsFalse(gameManager.IsTaskOn);
        gameManager.ToggleTaskOn();
        Assert.IsTrue(gameManager.IsTaskOn);
        gameManager.ToggleTaskOn();
        Assert.IsFalse(gameManager.IsTaskOn);
        yield return null;
    }

    /// <summary>
    /// Tests to ensure that the correct sound is played.
    /// </summary>
    [UnityTest]
    public IEnumerator PlaySoundTest()
    {
        gameManager.PlaySound("correct");
        Assert.AreEqual(gameManager.AudioManager.GetComponent<AudioSource>().clip, gameManager.SoundEffects[0]);
        gameManager.PlaySound("incorrect");
        Assert.AreEqual(gameManager.AudioManager.GetComponent<AudioSource>().clip, gameManager.SoundEffects[1]);
        yield return null;
    }
};
