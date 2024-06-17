using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class KnifeStateTest
{
    private GameObject Knife;
    private KnifeState _knifeState;
    private GameObject Fish;
    private FactoryFishState _fishState;
    private GameObject GameManager;
    private GameObject AudioManager;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        SceneManager.CreateScene("KnifeStateTest");
        var KnifePrefab = Resources.Load<GameObject>("Prefabs/Knife/FishKnife");
        Knife = Object.Instantiate(KnifePrefab);
        _knifeState = Knife.GetComponent<KnifeState>();
        _knifeState._durabilityCount = 1;
        var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactoryBadFish");
        Fish = Object.Instantiate(fishPrefab);
        _fishState = Fish.GetComponent<FactoryFishState>();

    }
    [Test]
    public void GetKnifeStateTest() {
        var knife = GameObject.FindGameObjectsWithTag("Knife");
        if (knife.Length >=1 && knife[0])
            _knifeState = knife[0].GetComponent<KnifeState>();
        Assert.AreEqual(_knifeState.gameObject, Knife,"Knife not found");
    }

    [Test]
    public void DecrementDurabilityTest()
    {
        int[] Tests = new int[3];
        Tests[0] = Random.Range(2,int.MaxValue);
        Tests[1] = Random.Range(0,1);
        Tests[2] = Random.Range(-1,-int.MaxValue);
        foreach (int i in Tests){
            _knifeState._durabilityCount = i;
            var knifeDurability = Knife.GetComponent<KnifeState>()._durabilityCount;
            _knifeState.DecrementDurabilityCount();
            if (knifeDurability == 1 || knifeDurability == 0)
                Assert.AreEqual(Knife.GetComponent<KnifeState>()._durabilityCount,-1);
            else if (knifeDurability > 1)
                Assert.AreEqual(Knife.GetComponent<KnifeState>()._durabilityCount, knifeDurability -1);
            else 
                Assert.AreEqual(Knife.GetComponent<KnifeState>()._durabilityCount, knifeDurability);
        }
    }

    [Test]
    public void RandomizeDurabilityTest()
    {
        _knifeState._durabilityCount = _knifeState.RandomizeDurability();
        Assert.IsTrue(5 < _knifeState._durabilityCount, "Randomized integer is too low");
        Assert.IsTrue(17 > _knifeState._durabilityCount, "Randomized integer is too high");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() 
    {
        Object.Destroy(Knife);
        Object.Destroy(Fish);
        SceneManager.UnloadSceneAsync("KnifeStateTest");
    }
}
