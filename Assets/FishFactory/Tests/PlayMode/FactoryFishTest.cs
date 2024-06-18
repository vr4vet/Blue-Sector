using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class FactoryFishTest
{

    private GameObject Knife;
    private KnifeState _knifeState;
    private GameObject Fish;
    private FactoryFishState _fishState;
    private Scene factoryFishScene;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        factoryFishScene = SceneManager.CreateScene("FactoryFishStateTest");
        SceneManager.SetActiveScene(factoryFishScene);
        var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactoryBadFish");
        Fish = Object.Instantiate(fishPrefab);
        _fishState = Fish.GetComponent<FactoryFishState>();

    }

    [Test]
    public void CutFishGillTest()
    {
        
    }


}
