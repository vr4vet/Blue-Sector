using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GuttingFishSortingTest
{
    private const string TestSceneName = "GuttingFishSortingTests";
    private GameObject Fish;
    private GameObject Sorter;
    private GuttingFishSorting SortingScript;

    // A Test behaves as an ordinary method
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
       // create a new test scene
       SceneManager.CreateScene(TestSceneName);

       // Load and instantiate the prefabs
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       Fish = Object.Instantiate(fishPrefab);
       Fish.transform.position = new Vector3(10, 10, 10);
       Sorter = GameObject.CreatePrimitive(PrimitiveType.Cube);
       Sorter.transform.position = new Vector3(0, 0, 0);
       BoxCollider SorterTrigger = Sorter.AddComponent<BoxCollider>();
       SorterTrigger.isTrigger = true;
       SortingScript = Sorter.AddComponent<GuttingFishSorting>();
        
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator SortGuttingSuccess()
    {
        Fish.GetComponent<FactoryFishState>().CurrentState = FactoryFishState.State.GuttingSuccess;
        SortingScript._successOnGuttingSuccess = FactoryFishState.State.GuttingSuccess;
       Assert.AreEqual(
        true,
        SortingScript.checkFishState(FactoryFishState.State.GuttingSuccess, Fish)
       );

        yield return null;
    }
    public void OneTimeTearDown()
    {
        Object.Destroy(Fish);
        Object.Destroy(Sorter);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
