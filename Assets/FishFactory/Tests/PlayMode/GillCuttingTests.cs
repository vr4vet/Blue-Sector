using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class GillCuttingTests
{
    private GameObject fish;
    private GameObject knife;
    private const string TestSceneName = "GillCuttingTestScene";

    [SetUp]
    public void SetUp()
    {
        SceneManager.CreateScene(TestSceneName);
        var fishPrefab = Resources.Load<GameObject>("Prefabs/TempFish");
        var knifePrefab = Resources.Load<GameObject>("Prefabs/Knife/FishKnife");
        fish = Object.Instantiate(fishPrefab);
        knife = Object.Instantiate(knifePrefab);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(fish);
        Object.Destroy(knife);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GillCuttingTestsWithEnumeratorPasses()
    {
        yield return null;
    }

    
}
