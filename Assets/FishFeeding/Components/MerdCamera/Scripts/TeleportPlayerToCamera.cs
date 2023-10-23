#nullable enable
using BNG;
using UnityEngine;

public class TeleportPlayerToCamera : MonoBehaviour
{
    [SerializeField] private GameObject PlayerController = null!; 

    private PlayerTeleport Teleport = null!;
    private PlayerGravity Gravity = null!;
    private GameObject CameraRig = null!;

    //private readonly List<(Camera, LayerMask)> cameraLayers = new();

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    private Color originalBackgroundColor;
    private CameraClearFlags originalClearFlags;
    private int originalCullingMask;

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

        Camera? playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();//CameraRig.GetComponentInChildren<Camera>();

        // save player position for return trip
        playerStartPosition = PlayerController.transform.position;
        playerStartRotation = PlayerController.transform.rotation;

        originalBackgroundColor = playerCam.backgroundColor;
        originalClearFlags = playerCam.clearFlags;
        originalCullingMask = playerCam.cullingMask;


        // teleport player to the camera's fish cage
        Teleport.TeleportPlayerToTransform(selectedCamera.transform.parent.transform);
        
        playerCam.GetComponent<UnderwaterFog>().EnableEffects();
        playerCam.backgroundColor = new Color(0, 255, 255, 255);
        playerCam.clearFlags = CameraClearFlags.SolidColor;
        playerCam.cullingMask = selectedCamera.cullingMask;
        
       
        Gravity.GravityEnabled = false;
    }

    // Moves player to original position
    public void TeleportBack()
    {
        // get the currently selected camera
        Camera? selectedCamera = GetComponent<MerdCameraController>().SelectedCamera;

        if (selectedCamera == null)
        {
            return;
        }

        Camera? playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();//CameraRig.GetComponentInChildren<Camera>();

        // teleport player back to original position
        Teleport.TeleportPlayer(playerStartPosition, playerStartRotation);
        
        playerCam.GetComponentInChildren<UnderwaterFog>().DisableEffects();
        playerCam.backgroundColor = originalBackgroundColor;
        playerCam.clearFlags = originalClearFlags;
        playerCam.cullingMask = originalCullingMask;

        Gravity.GravityEnabled = true;
    }
}
