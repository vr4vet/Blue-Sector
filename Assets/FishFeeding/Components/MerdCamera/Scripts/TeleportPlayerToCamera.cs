#nullable enable
using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportPlayerToCamera : MonoBehaviour
{
    private Transform? cameraRig;

    [SerializeField]
    public GameObject Player;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;


    public void TeleportToCamera()
    {
        //save player position for the return trip
        var selectedCamera = GetComponent<MerdCameraController>().SelectedCamera;
        if (selectedCamera == null)
        {
            return;
        }

        var destination = selectedCamera.transform;
        cameraRig = Player.GetComponentInChildren<BNGPlayerController>().CameraRig;
        playerStartPosition = cameraRig.position;
        playerStartRotation = cameraRig.rotation;
        cameraRig.SetPositionAndRotation(destination.position, playerStartRotation);
    }

    // Moves player to original position
    public void TeleportBack()
    {
        cameraRig!.SetPositionAndRotation(playerStartPosition, playerStartRotation);
    }
}
