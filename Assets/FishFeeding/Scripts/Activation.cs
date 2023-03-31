using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    Scoring script;

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Scoring>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CameraRig" && !script.inActivatedArea)
        {
            script.inActivatedArea = true;
            Debug.Log("activated true");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "CameraRig" && script.inActivatedArea)
        {
            script.inActivatedArea = false;
            Debug.Log("activated false");
        }
    }
}
