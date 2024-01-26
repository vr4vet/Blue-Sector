using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadFilletCount : MonoBehaviour {

    public FiletManager manager;

    public bool destroyArea;

    // Use this for initialization
    void Start () {
        manager = GameObject.FindWithTag("FilletManager").GetComponent<FiletManager>();
        destroyArea = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        // Detects if the fillet is in the last area of the belt
        if (other.gameObject.name == "DestroyArea")
        {
            destroyArea = true;
        }
    }

    /*void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "DestroyArea")
        destroyArea = true;
    }*/
    void OnTriggerExit(Collider other)
    {
        // Detects if the fillet is NOT in the last area of the belt
        if (other.gameObject.name == "DestroyArea")
            destroyArea = false;
    }
    void OnDestroy()
    {
        //If the fillet is destroyed in the area, it is counted as wrong, this is to differentiate the objects destroyed by cutting
        if (destroyArea)
        {
            manager.filletWrong += 1;
        }
    }
}
