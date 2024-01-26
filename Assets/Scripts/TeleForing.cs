using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleForing : MonoBehaviour {

    private Vector3 telePosition;
    private Vector3 boatPosition;
    private Quaternion boatRotation;

    public Valve.VR.InteractionSystem.Player player;
    public GameObject boat;
    public newAccel boataccel;
    public BoatKey turnoffBoat;
    void Start()
    {
        telePosition = new Vector3(1294.2f, 1.1f, 1078.7f);
        boatPosition = new Vector3(1291.9f, 0.09f, 1089.2f);
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
