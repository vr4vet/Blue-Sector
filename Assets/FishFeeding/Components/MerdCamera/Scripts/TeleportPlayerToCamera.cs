#nullable enable
using BNG;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportPlayerToCamera : MonoBehaviour
{
    [SerializeField] private GameObject PlayerController; 

    private PlayerTeleport Teleport;
    private PlayerGravity Gravity;
    private GameObject CameraRig;

    private readonly List<(Camera, LayerMask)> cameraLayers = new();

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    private void Start()
    {
        // get the necessary parts of the player rig
        Teleport = PlayerController.GetComponent<PlayerTeleport>();
        Gravity = PlayerController.GetComponent<PlayerGravity>();
        CameraRig = PlayerController.transform.Find("CameraRig").gameObject;
    }

    // Moves player to the fish cage
    public void TeleportToCamera()
    {
        // get the currently selected camera
        Camera? selectedCamera = GetComponent<MerdCameraController>().SelectedCamera;
        if (selectedCamera == null)
        {
            return;
        }

        // save player position for return trip
        playerStartPosition = PlayerController.transform.position;
        playerStartRotation = PlayerController.transform.rotation;

        // teleport player to the camera's fish cage
        Teleport.TeleportPlayerToTransform(selectedCamera.transform.parent.transform);
        Gravity.GravityEnabled = false;
        
        // set up underwater shader
        Camera[] cameras = CameraRig.GetComponentsInChildren<Camera>();
        LayerMask postProcessLayer = selectedCamera.GetComponent<PostProcessLayer>().volumeLayer;
        foreach (var camera in cameras)
        {
            var layer = camera.GetComponent<PostProcessLayer>();
            if (layer == null)
            {
                continue;
            }

            cameraLayers.Add((camera, layer.volumeLayer));
            layer.volumeLayer = postProcessLayer; // water
        }
    }

    // Moves player to original position
    public void TeleportBack()
    {
        Teleport.TeleportPlayer(playerStartPosition, playerStartRotation);
        Gravity.GravityEnabled = true;

        foreach (var (camera, layer) in cameraLayers)
        {
            camera.GetComponent<PostProcessLayer>().volumeLayer = layer;
        }
    }
}
