using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoodBad : MonoBehaviour {

    public bool fishGood;
    public bool goodDespawn;
    public FishManager manager;

    private void Start()
    {
        goodDespawn = true;
        manager = GameObject.Find("FishManager").GetComponent<FishManager>();
    }
    // Checks if the fish is good or bad, and changes the counter of wrongs in the manager
    private void OnDestroy()
    {
        if (!fishGood && goodDespawn)
        {
            manager.wrongCounter += 1;
        }
        if (fishGood && !goodDespawn)
        {
            manager.wrongCounter += 1;
        }
    }
}
