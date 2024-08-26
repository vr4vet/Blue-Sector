using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeScreenSpaceOverlay : MonoBehaviour
{
    private float StartTime;
    private bool CameraSet = false;
    private BNG.BNGPlayerController PlayerController;
    private Camera PlayerCamera;
    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = true;
        StartTime = Time.time;
        PlayerController = FindObjectOfType<BNG.BNGPlayerController>();
        PlayerCamera = PlayerController.transform.Find("CameraRig/TrackingSpace/CenterEyeAnchor").transform.GetComponent<Camera>();

        // disable the microscope overlay so the player can see their environment
        PlayerCamera.cullingMask = LayerMask.GetMask(
            "Default", "TransparentFX", 
            "Ignore Raycast", "Water", 
            "UI", "Fish", "Merd", 
            "UnderwaterShade", "Grabbable", "Player",
            "Menu", "Hand", "Bone");
    }

    private void FixedUpdate()
    {
        // need to wait a bit before setting fetching the CenterEyeAnchor camera
        if (Time.time - StartTime > 1 && !CameraSet)
        {
            GetComponent<Canvas>().worldCamera = PlayerCamera;//PlayerController.transform.Find("CameraRig/TrackingSpace/CenterEyeAnchor").transform.GetComponent<Camera>();
            Debug.Log("It's been 1 second");
            CameraSet = true;
            Debug.Log(GetComponent<Canvas>().worldCamera);
        }   
    }

    public void EnableOverlay()
    {
        GetComponent<Canvas>().transform.GetComponentInChildren<RawImage>().texture = MicroscopeMonitor.GetComponentInChildren<RawImage>().texture;
        GetComponent<Canvas>().transform.GetComponentInChildren<RawImage>().uvRect = MicroscopeMonitor.GetComponentInChildren<RawImage>().uvRect;
        PlayerCamera.cullingMask = LayerMask.GetMask(
    /*"Default", "TransparentFX",
    "Ignore Raycast", "Water",
    "UI", "Fish", "Merd",
    "UnderwaterShade", "Grabbable", "Player",
    "Menu", "Hand", "Bone", */"MicroscopeOverlay");
    }

    public void DisableOverlay()
    {
        PlayerCamera.cullingMask = LayerMask.GetMask(
            "Default", "TransparentFX",
            "Ignore Raycast", "Water",
            "UI", "Fish", "Merd",
            "UnderwaterShade", "Grabbable", "Player",
            "Menu", "Hand", "Bone");
    }
}
