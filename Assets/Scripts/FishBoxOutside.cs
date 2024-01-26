using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBoxOutside : MonoBehaviour {

    public FishBoxManager fbManager;

	// Use this for initialization
	void Start () {
        fbManager = GameObject.Find("FishBoxManager").GetComponent<FishBoxManager>();
	}

    private void OnTriggerEnter(Collider other)
    {
        //For Box sorting, checks is the fish is outside of the box when despawning
        if (other.gameObject.name == "OutsideBoxDetection")
        {
            fbManager.wrongCounter += 1;
        }



    }
}
