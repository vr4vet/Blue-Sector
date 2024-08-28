using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeScreenSpaceOverlay : MonoBehaviour
{
    private float StartTime;
    private bool CameraSet = false; // used to end the wait in FixedUpdate after setting camera
    private bool OverlayEnabled = false;
    private BNG.BNGPlayerController PlayerController;
    private Camera PlayerCamera;
    private RawImage RawImage;
    private Collider HeadCollider;
    MicroscopeOverlayTrigger trigger;

    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Canvas>().enabled = true;
        StartTime = Time.time;
        PlayerController = FindObjectOfType<BNG.BNGPlayerController>();
        PlayerCamera = PlayerController.transform.Find("CameraRig/TrackingSpace/CenterEyeAnchor").transform.GetComponent<Camera>();
        RawImage = GetComponent<Canvas>().transform.GetComponentInChildren<RawImage>();
        trigger = transform.parent.GetComponent<MicroscopeOverlayTrigger>();

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
            GetComponent<Canvas>().worldCamera = PlayerCamera;
            CameraSet = true;
        }   
    }

    private void Update()
    {
        if (OverlayEnabled)
        {
            // roll math found at https://sunday-lab.blogspot.com/2008/04/get-pitch-yaw-roll-from-quaternion.html
            Quaternion rotation = HeadCollider.transform.localRotation;
            float roll = Mathf.Atan2(2 * (rotation.x * rotation.y + rotation.w * rotation.z), rotation.w * rotation.w + rotation.x * rotation.x - rotation.y * rotation.y - rotation.z * rotation.z);
            
            float rotationOffset = Vector3.Angle((trigger.transform.position - HeadCollider.transform.position), HeadCollider.transform.forward);

            // dim overlay when head is rotated or rolled
            if (Mathf.Abs(roll) > 0.1f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), Mathf.Abs(roll) * 3f, 2f * Time.deltaTime));
            else if (rotationOffset > 20f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), rotationOffset * 0.02f, 2f * Time.deltaTime));
            else if (Mathf.Abs(roll) <= 0.1f && rotationOffset <= 20f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), 0f, 2f * Time.deltaTime));

        }
    }

    public void EnableOverlay()
    {
        GetComponent<Canvas>().enabled = true;
        OverlayEnabled = true;

        RawImage.texture = MicroscopeMonitor.GetComponentInChildren<RawImage>().texture;
        RawImage.uvRect = MicroscopeMonitor.GetComponentInChildren<RawImage>().uvRect;
        PlayerCamera.cullingMask = LayerMask.GetMask("MicroscopeOverlay");  // cull everything except for microscope overlay, making only that visible
    }

    public void DisableOverlay()
    {
        GetComponent<Canvas>().enabled = false;
        OverlayEnabled = false;
        PlayerCamera.cullingMask = LayerMask.GetMask(
            "Default", "TransparentFX",
            "Ignore Raycast", "Water",
            "UI", "Fish", "Merd",
            "UnderwaterShade", "Grabbable", "Player",
            "Menu", "Hand", "Bone");    // cull nothing except for microscope overlay, making all other objects visible
    }

    public void RotateImage(float x, float y, float z)
    {
        RawImage.transform.Rotate(x, y, z);
    }

    public void SetHeadCollider(Collider HeadCollider)
    {
        this.HeadCollider = HeadCollider;
    }
}
