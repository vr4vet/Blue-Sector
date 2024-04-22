using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class GameManagerTests
{
    private GameManager gameManager;
    private GameObject gameManagerObject;
    private AudioClip[] soundEffects;
    private const string TestSceneName = "GameManagerTestScene";

    [SetUp]
    public void SetUp()
    {
        // create scene for testing
        SceneManager.CreateScene(TestSceneName);

        // Create a new GameObject and add the GameManager component to it
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Load the sound effects
        soundEffects = Resources.LoadAll<AudioClip>("Sounds");
        gameManager.SoundEffects = soundEffects;

    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the GameManager object
        Object.Destroy(gameManagerObject);
        // Unload the test scene
        SceneManager.UnloadSceneAsync(TestSceneName);
    }

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

    [UnityTest]
    public IEnumerator PlaysTheCorrectSound()
    {
        gameManager.PlaySound("correct");
        Assert.AreEqual(gameManager.GetComponent<AudioSource>().clip, soundEffects[0]);
        gameManager.PlaySound("incorrect");
        Assert.AreEqual(gameManager.GetComponent<AudioSource>().clip, soundEffects[1]);
        yield return null;
    }
};
