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
        GetComponent<Canvas>().enabled = true;
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
            Debug.Log(GetComponent<Canvas>().worldCamera);
        }   
    }

    private float OldPlayerRotationZ;
    private float InitialRotationZ;
    private Quaternion InitialRotation;
    private void Update()
    {
        if (OverlayEnabled)
        {
            // the commented out code is multple attempts at correcting the image's rotation when the user rolls their head

            //Debug.Log("Enabled");
            //Debug.Log(HeadCollider.transform.eulerAngles);

            //float OffsetZ = HeadCollider.transform.eulerAngles.z - OldPlayerRotationZ;
            //RotateImage(0, 0, -OffsetZ);
            //RotateImage(0, 0, HeadCollider.transform.eulerAngles.z -  InitialRotationZ);
            //RawImage.transform.rotation *= Quaternion.Euler(1, 1, 3);
            //Quaternion inverse = Quaternion.Inverse(HeadCollider.transform.rotation);
            //RawImage.transform.rotation *= Quaternion.Euler(1, 1, inverse.eulerAngles.z);    
            //RotateImage(0, 0, );

            //Debug.Log("All: " + InitialRotation * Quaternion.Inverse(HeadCollider.transform.rotation));
/*            float rotationOffset = Mathf.Max(
                Mathf.Abs((InitialRotation * Quaternion.Inverse(HeadCollider.transform.localRotation)).x),
                Mathf.Abs((InitialRotation * Quaternion.Inverse(HeadCollider.transform.localRotation)).y),
                Mathf.Abs((InitialRotation * Quaternion.Inverse(HeadCollider.transform.localRotation)).z)
                );*/

            //Debug.Log("Max: " + rotationOffset);
            /*            if (rotationOffset > 0.05f)
                            trigger.AdjustDarkening(rotationOffset * 3f);*/

            // dim overlay when head is rotated
            float rotationOffset = Vector3.Angle((trigger.transform.position - HeadCollider.transform.position), HeadCollider.transform.forward);
            if (rotationOffset > 20f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), rotationOffset * 0.02f, 2f * Time.deltaTime));
            else
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), 0f, 2f * Time.deltaTime));

        }
    }

    public void EnableOverlay()
    {
        //InitialRotationZ = HeadCollider.transform.eulerAngles.z;
        //RawImage.transform.rotation *= Quaternion.Euler(1, 1, 0);
        InitialRotation = HeadCollider.transform.localRotation;
        OverlayEnabled = true;
        RawImage.texture = MicroscopeMonitor.GetComponentInChildren<RawImage>().texture;
        RawImage.uvRect = MicroscopeMonitor.GetComponentInChildren<RawImage>().uvRect;
        PlayerCamera.cullingMask = LayerMask.GetMask("MicroscopeOverlay");
    }

    public void DisableOverlay()
    {
        OverlayEnabled = false;
        PlayerCamera.cullingMask = LayerMask.GetMask(
            "Default", "TransparentFX",
            "Ignore Raycast", "Water",
            "UI", "Fish", "Merd",
            "UnderwaterShade", "Grabbable", "Player",
            "Menu", "Hand", "Bone");
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
