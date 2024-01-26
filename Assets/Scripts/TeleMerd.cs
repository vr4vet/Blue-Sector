using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleMerd : MonoBehaviour
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

        telePosition = new Vector3(1175.2f, 0.7f, 1100.5f);
        boatPosition = new Vector3(1168.04f, 0.09f, 1097.73f);
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
