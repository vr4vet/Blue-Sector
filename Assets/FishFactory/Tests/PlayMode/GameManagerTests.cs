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
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(gameManagerObject);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }

    [Test]
    public void GameManager_IsSingleton()
    {
        // Create another GameManager instance
        var anotherGameObject = new GameObject();
        var anotherGameManager = anotherGameObject.AddComponent<GameManager>();

        // Assert that the second instance is destroyed and the first instance remains
        Assert.IsTrue(gameManager == GameManager.Instance);
        Assert.IsFalse(anotherGameManager == GameManager.Instance);

        // Clean up
        Object.Destroy(anotherGameObject);
    }

    [Test]
    public void ToggleTaskOn_TogglesCorrectly()
    {
        // Initial state is false
        Assert.IsFalse(gameManager.IsTaskOn);

        // Toggle task on
        gameManager.ToggleTaskOn();
        Assert.IsTrue(gameManager.IsTaskOn);

        // Toggle task off
        gameManager.ToggleTaskOn();
        Assert.IsFalse(gameManager.IsTaskOn);
    }
};
