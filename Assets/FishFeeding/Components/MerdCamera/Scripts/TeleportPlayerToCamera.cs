#nullable enable
using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportPlayerToCamera : MonoBehaviour
{
    private readonly List<(Camera, LayerMask)> cameraLayers = new();
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

        var playerController = BNGPlayerLocator.Instance.PlayerController;
        var destination = selectedCamera.transform.position - playerController.CameraRig.localPosition;
        playerController.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePosition;
        playerStartPosition = playerController.transform.position;
        playerStartRotation = playerController.transform.rotation;
        playerController.transform.SetPositionAndRotation(destination, playerStartRotation);

        var postProcessLayer = selectedCamera.GetComponent<PostProcessLayer>().volumeLayer;
        var cameras = playerController.CameraRig.GetComponentsInChildren<Camera>();
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
        var playerController = BNGPlayerLocator.Instance.PlayerController;
        playerController.transform!.SetPositionAndRotation(playerStartPosition, playerStartRotation);
        playerController.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
        foreach (var (camera, layer) in cameraLayers)
        {
            camera.GetComponent<PostProcessLayer>().volumeLayer = layer;
        }
        //foreach (var component in cameraComponents)
        //{
        //    Destroy(component);
        //}
        //cameraComponents.Clear();
    }
}
