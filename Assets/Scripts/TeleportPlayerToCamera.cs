using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportPlayerToCamera : MonoBehaviour {
    public Valve.VR.InteractionSystem.Player player;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    //moves player to the camera position
    public void TeleportToCamera() {
        if (gameObject.GetComponent<Camera>().enabled == true) {
            //save player position for the return trip
            playerStartPosition = player.transform.position;
            playerStartRotation = player.transform.rotation;
            Debug.Log(playerStartPosition);
            player.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    // Moves player to original position
    public void TeleportBack() {
        player.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
        Debug.Log(playerStartPosition);
    }
}
