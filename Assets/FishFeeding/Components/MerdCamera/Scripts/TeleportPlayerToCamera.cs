#nullable enable
using BNG;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportPlayerToCamera : MonoBehaviour
{
    [SerializeField] private GameObject PlayerController; 
    [SerializeField] private PlayerTeleport Teleport;
    [SerializeField] private PlayerGravity Gravity;
    [SerializeField] private GameObject CameraRig;
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

        
        playerStartPosition = PlayerController.transform.position;
        playerStartRotation = PlayerController.transform.rotation;

        Teleport.TeleportPlayer(selectedCamera.transform.position, selectedCamera.transform.rotation);
        Gravity.GravityEnabled = false;
        Camera[] cameras = CameraRig.GetComponentsInChildren<Camera>();
/*        Transform playerController = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerController");
        Vector3 destination = selectedCamera.transform.parent.position;
        playerController.GetComponent<PlayerGravity>().GravityEnabled = false;
        playerStartPosition = playerController.transform.position;
        playerStartRotation = playerController.transform.rotation;
        playerController.transform.SetPositionAndRotation(destination, playerStartRotation);
*/
        LayerMask postProcessLayer = selectedCamera.GetComponent<PostProcessLayer>().volumeLayer;
        //Camera[] cameras = playerController.Find("CameraRig").GetComponentsInChildren<Camera>();
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
        Teleport.TeleportPlayer(playerStartPosition + new Vector3(0, 2, 0), playerStartRotation);
        Gravity.GravityEnabled = true;
        //var playerController = BNGPlayerLocator.Instance.PlayerController;
        //playerController.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
        //playerController.GetComponent<PlayerGravity>().GravityEnabled = true;

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
