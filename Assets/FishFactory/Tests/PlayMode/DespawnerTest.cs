using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class DespawnerTest
{
    private const string TestSceneName = "DespawnerTests";
    private GameObject Fish;
    private GameObject Knife;
    private GameObject Despawner;
    private Despawner DespawnerScript;

    // A Test behaves as an ordinary method
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
       // create a new test scene
       SceneManager.CreateScene(TestSceneName);

       // Load and instantiate the prefabs
       var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactorySalmon");
       Fish = UnityEngine.Object.Instantiate(fishPrefab);
       Fish.transform.position = new Vector3(10, 10, 10);

       var knifePrefab = Resources.Load<GameObject>("Prefabs/Knife/FishKnife");
       Knife = UnityEngine.Object.Instantiate(knifePrefab);
       Knife.transform.position = new Vector3(10, 20, 10);

       Despawner = GameObject.CreatePrimitive(PrimitiveType.Cube);
       Despawner.transform.position = new Vector3(0, 0, 0);
       BoxCollider DespawnerTrigger = Despawner.AddComponent<BoxCollider>();
       DespawnerTrigger.isTrigger = true;
       DespawnerScript = Despawner.AddComponent<Despawner>();
        
    }

    [UnityTest]
    public IEnumerator DespawnFish()
    {
        Fish.transform.position = Despawner.transform.position + Vector3.up; // Set the fish position to be on top of the despawner box
        yield return new WaitForSeconds(1); // Wait for the collision and potential destruction to be processed
        Debug.Log(Fish);
       
        if (Fish == null){
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }

    /// <summary>
    /// Tests to ensure that the DiscardBadFish script does not count non-fish objects towards the discarded fish count.
    /// </summary>
    [UnityTest]
    public IEnumerator DoesNotDespawnNonFishObjects()
    {
        Knife.transform.position = Despawner.transform.position + Vector3.up; // Set the knife position to be on top of the despawner box
        yield return new WaitForSeconds(1); // wait for 1 second to let the the coolision happen
        Debug.Log(Knife.tag);

        if (Knife == null){
            Assert.Fail();
        }
        else
        {
            Assert.Pass();
        }
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        UnityEngine.Object.Destroy(Fish);
        UnityEngine.Object.Destroy(Knife);
        UnityEngine.Object.Destroy(Despawner);
        SceneManager.UnloadSceneAsync(TestSceneName);
    }
}
