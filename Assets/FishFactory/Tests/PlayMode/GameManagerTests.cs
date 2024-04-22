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
        Object.Destroy(gameManagerObject);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }

    [Test]
    public void ToggleTaskOnTest()
    {
        Assert.IsFalse(gameManager.IsTaskOn);
        gameManager.ToggleTaskOn();
        Assert.IsTrue(gameManager.IsTaskOn);
        gameManager.ToggleTaskOn();
        Assert.IsFalse(gameManager.IsTaskOn);
    }

    [Test]
    public void SoundsArePlaying()
    {
        gameManager.PlaySound("correct");
        Assert.IsTrue(gameManager.GetComponent<AudioSource>().isPlaying);
    }

    [Test]
    public void PlaysTheCorrectSound()
    {
        gameManager.PlaySound("correct");
        Assert.AreEqual(gameManager.GetComponent<AudioSource>().clip, soundEffects[0]);
        gameManager.PlaySound("incorrect");
        Assert.AreEqual(gameManager.GetComponent<AudioSource>().clip, soundEffects[1]);
    }

    [Test]
    public void CanCompleteHSE()
    {
        gameManager.SetPlayerGloves();
        Assert.IsFalse(gameManager.HSERoomCompleted);
        gameManager.EarProtectionOn = true;
        Assert.IsFalse(gameManager.HSERoomCompleted);
        gameManager.BootsOn = true;
        Asser.IsTrue(gameManager.HSERoomCompleted);
    }
};
