using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    Game script;
    GameObject startGameToolTip;
    GameObject monitorScore;
    GameObject player;
    MeshCollider activationCollider; 

    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<Game>();
        startGameToolTip = GameObject.Find("StartGameToolTip");
        monitorScore = GameObject.Find("MonitorScore");
        player = GameObject.Find("XR Rig Advanced");
        activationCollider = GetComponent<MeshCollider>();
        /*startGameToolTip.SetActive(false);*/
    }

    private void Update()
    {
        /*Vector3 playerPosition = player.transform.position;
        Vector3 monitorPosition = monitorScore.transform.position;
        Debug.Log("playerposition: " + playerPosition);
        Debug.Log("monitorposition: " + monitorPosition);
        if ((playerPosition.z < monitorPosition.z ) && (playerPosition.z > monitorPosition.z - 1.5))
        {
            script.inActivatedArea = true;
        }
        else
        {
            script.inActivatedArea = false;
        }*/

        if (!script.startGame)
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
        if (other.name == "CameraRig" && !script.InActivatedArea)
        {
            script.InActivatedArea = true;
            /*startGameToolTip.GetComponent<Renderer>().enabled = true;*/
            Debug.Log("activated true");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "CameraRig" && script.InActivatedArea)
        {
            script.InActivatedArea = false;
            /*startGameToolTip.GetComponent<Renderer>().enabled = false;*/
            Debug.Log("activated false");
        }
    }

    /*private void OnTriggerEnter(Collider other)
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
    }*/


}
