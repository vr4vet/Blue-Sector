using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleSlakteri : MonoBehaviour
{

    private Vector3 telePosition;
    private Vector3 boatPosition;
    private Quaternion boatRotation;
    public GameObject boat;
    public Valve.VR.InteractionSystem.Player player;
    public newAccel boataccel;
    public BoatKey turnoffBoat;

    void Start()
    {
 
        telePosition = new Vector3(1117.7f, 1.4f, 1410.8f);
        boatPosition = new Vector3(1117.4f, 0.09f, 1367.33f);
        boatRotation = boat.transform.rotation;

    }

    // Moves player to position
    public void TeleportToLocation()
    {
        GameObject.FindGameObjectWithTag("Player").transform.parent = null;
        if (boataccel.enabled)
        {
            turnoffBoat.TurnOnOff();
            StartCoroutine(boatFreeze());
        }
        else
        {
            boat.transform.position = boatPosition;
            boat.transform.rotation = boatRotation;
        }
        player.transform.SetPositionAndRotation(telePosition, player.transform.rotation);

    }

    IEnumerator boatFreeze()
    {
        yield return new WaitForSeconds(1.5f);
        boat.transform.position = boatPosition;
        boat.transform.rotation = boatRotation;
    }
}