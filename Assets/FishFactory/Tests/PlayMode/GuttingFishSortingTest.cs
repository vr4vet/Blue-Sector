using System;
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
        // Create a new test scene
        SceneManager.CreateScene(TestSceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

       // Load and instantiate the prefabs
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Fish.transform.position = new Vector3(10, 10, 10);
       Sorter = GameObject.CreatePrimitive(PrimitiveType.Cube);
       Sorter.transform.position = new Vector3(0, 0, 0);
       BoxCollider SorterTrigger = Sorter.AddComponent<BoxCollider>();
       SorterTrigger.isTrigger = true;
       SortingScript = Sorter.AddComponent<GuttingFishSorting>();
        
    }
    [UnityTest]
    public IEnumerator DoesNotSortAgain()
    {
        Fish.transform.position = Sorter.transform.position + Vector3.up; // Set the fish position to be on top of the discard box
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed
        Fish.transform.position = Sorter.transform.position + Vector3.up; // Set the fish position to be on top of the discard box
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed

        Assert.AreEqual(
            1,
            SortingScript.SortedFishCount,
            "The fish should not be discarded again after it has been discarded once"
        );
    }
    
    [UnityTest]
    public IEnumerator SortGuttingSuccess()
    {
        foreach (FactoryFishState.GuttingState state in Enum.GetValues(typeof(FactoryFishState.GuttingState)))
        {
            Fish.GetComponent<FactoryFishState>().guttingState = state;
            SortingScript._successOnGuttingSuccess = FactoryFishState.GuttingState.GuttingSuccess;
            if (Fish.GetComponent<FactoryFishState>().guttingState == FactoryFishState.GuttingState.GuttingSuccess)
                Assert.AreEqual(true, SortingScript.checkFishState(Fish));
            else
                Assert.AreEqual(false, SortingScript.checkFishState(Fish));
        }
        yield return null;
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        UnityEngine.Object.Destroy(Fish);
        UnityEngine.Object.Destroy(Sorter);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
