using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    Scoring script;
    GameObject startGameToolTip;

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Scoring>();
        startGameToolTip = GameObject.Find("StartGameToolTip");
        /*startGameToolTip.GetComponent<Renderer>().enabled = false;*/
        startGameToolTip.SetActive(false);
    }

    /*private void Update()
    {
        if (script.startGame)
        {
            startGameToolTip.SetActive(false);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CameraRig" && !script.inActivatedArea)
        {
            script.inActivatedArea = true;
            /*startGameToolTip.GetComponent<Renderer>().enabled = true;*/
            startGameToolTip.SetActive(true);
            Debug.Log("activated true");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "CameraRig" && script.inActivatedArea)
        {
            script.inActivatedArea = false;
            /*startGameToolTip.GetComponent<Renderer>().enabled = false;*/
            startGameToolTip.SetActive(false);
            Debug.Log("activated false");
        }
    }


}
