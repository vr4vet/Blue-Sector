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
        // get currently selected cage camera and player camera
        Camera? selectedCamera = GetComponent<MerdCameraController>().SelectedCamera;
        Camera? playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        if (selectedCamera == null || playerCam == null)
        {
            return;
        }

        // save player position for return trip
        playerStartPosition = PlayerController.transform.position;
        playerStartRotation = PlayerController.transform.rotation;

        // save player camera settings for restore trip
        originalBackgroundColor = playerCam.backgroundColor;
        originalClearFlags = playerCam.clearFlags;
        originalCullingMask = playerCam.cullingMask;


        // teleport player to the camera's fish cage
        Teleport.TeleportPlayerToTransform(selectedCamera.transform.parent.transform);
        
        // set up underwater effects
        playerCam.GetComponent<UnderwaterFog>().EnableEffects();
        playerCam.backgroundColor = new Color(0, 255, 255, 255);
        playerCam.clearFlags = CameraClearFlags.SolidColor;
        playerCam.cullingMask = selectedCamera.cullingMask;
        
       
        Gravity.GravityEnabled = false;
    }

    // Moves player to original position
    public void TeleportBack()
    {
        // get currently selected cage camera and player camera
        Camera? selectedCamera = GetComponent<MerdCameraController>().SelectedCamera;
        Camera? playerCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        
        if (selectedCamera == null || playerCam == null)
        {
            return;
        }

        // teleport player back to original position
        Teleport.TeleportPlayer(playerStartPosition, playerStartRotation);
        
        // Disable underwater effects and camera restore settings
        playerCam.GetComponentInChildren<UnderwaterFog>().DisableEffects();
        playerCam.backgroundColor = originalBackgroundColor;
        playerCam.clearFlags = originalClearFlags;
        playerCam.cullingMask = originalCullingMask;

        Gravity.GravityEnabled = true;
    }
}
