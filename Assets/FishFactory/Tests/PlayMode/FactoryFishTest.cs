using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class FactoryFishTest
{

    private GameObject Fish;
    private FactoryFishState _fishState;
    private Scene factoryFishScene;
    private GameManager gameManager;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        factoryFishScene = SceneManager.CreateScene("FactoryFishStateTest");
        SceneManager.SetActiveScene(factoryFishScene);
        var fishPrefab = Resources.Load<GameObject>("Prefabs/Fish/FishFactoryBadFish");
        Fish = UnityEngine.Object.Instantiate(fishPrefab);
        _fishState = Fish.GetComponent<FactoryFishState>();

         var gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Load the sound effects
        var audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
        var audioManager = UnityEngine.Object.Instantiate(audioManagerPrefab);
        gameManager.SoundEffects = Resources.LoadAll<AudioClip>("Sounds");
        gameManager.AudioManager = audioManager;

        //create an audio listener
        var listener = new GameObject("AudioListener");
        listener.AddComponent<AudioListener>();

    }

    [Test]
    public void CutFishGillTest()
    {
        Material fishMaterial = Fish.transform.GetChild(0).GetComponent<Renderer>().material;
        Renderer fishRenderer = Fish.transform.GetChild(0).GetComponent<Renderer>();

        foreach (FactoryFishState.Tier tier in Enum.GetValues(typeof(FactoryFishState.Tier)))
        {
            fishRenderer.material = fishMaterial;
            _fishState.correctlyBled = false;
            _fishState.fishTier = tier;
            _fishState.CutFishGills();
            if(tier == FactoryFishState.Tier.BadQuality)
            {
                Assert.IsFalse(_fishState.correctlyBled, "fish shouldn not be registered as correctly bled");
            }
            else 
            {
                Assert.IsTrue(_fishState.correctlyBled, "fish should have been registered as correctly bled");
            }
        }
    }
    [Test]
    public void CutFishBodyTest()
    {
        for(int i=0;i>2;i++)
        {
            if(i==0)
                _fishState.correctlyBled=false;
            else
                _fishState.correctlyBled=true;
            _fishState.CutFishBody();
            if(i==0)
                Assert.IsFalse(_fishState.correctlyBled);
            else
                Assert.IsTrue(_fishState.correctlyBled);
            
        }
    }
    [Test]
    public void MetalInFishTest()
    {
        _fishState.PlaceMetalInFish();
        GameObject MetalPiece = GameObject.Find("MetalPiece").gameObject;
        Assert.AreNotEqual(MetalPiece,null,"cannot find metal piece");
    }


}
