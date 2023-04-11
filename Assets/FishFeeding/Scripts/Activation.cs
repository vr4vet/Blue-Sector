using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    Game script;
    GameObject startGameToolTip;
    MeshCollider activationCollider; 

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Game>();
        startGameToolTip = GameObject.Find("StartGameToolTip");
        activationCollider = GetComponent<MeshCollider>();
        startGameToolTip.SetActive(false);
    }

    private void Update()
    {
        if (script.inActivatedArea && !script.startGame)
        {
            startGameToolTip.SetActive(true);
        }
        else
        {
            startGameToolTip.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CameraRig" && !script.inActivatedArea)
        {
            script.inActivatedArea = IsInside(activationCollider, other.transform.position);
            Debug.Log("activated true" + script.inActivatedArea);
            Debug.Log("position: " + other.transform.position);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "CameraRig" && script.inActivatedArea)
        {
            script.inActivatedArea = IsInside(activationCollider, other.transform.position);
            Debug.Log("activated false" + script.inActivatedArea);
        }
    }

    public static bool IsInside(Collider c, Vector3 point)
    {
        Vector3 closest = c.ClosestPoint(point);
        point.y = closest.y;
        Debug.Log("closest: " + closest);
        Debug.Log("point: " + point);
        return closest == point;  // If closest == point then point is inside collider
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.name == "CameraRig" && !script.inActivatedArea)
        {
            script.inActivatedArea = true;
            *//*startGameToolTip.GetComponent<Renderer>().enabled = true;*//*
            Debug.Log("activated true");
        }
    }*/


}
