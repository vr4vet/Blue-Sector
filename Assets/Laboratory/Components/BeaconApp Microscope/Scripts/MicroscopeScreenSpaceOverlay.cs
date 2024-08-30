using UnityEngine;
using UnityEngine.UI;

public class MicroscopeScreenSpaceOverlay : MonoBehaviour
{
    private float StartTime;
    private bool CameraSet = false; // used to end the wait in FixedUpdate after setting camera
    private bool OverlayEnabled = false;
    private BNG.BNGPlayerController PlayerController;
    private Camera PlayerCamera;
    private Image Image;
    private Collider HeadCollider;
    MicroscopeOverlayTrigger trigger;

    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    private bool Quest3 = false;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Canvas>().enabled = true;
        StartTime = Time.time;
        PlayerController = FindObjectOfType<BNG.BNGPlayerController>();
        PlayerCamera = PlayerController.transform.Find("CameraRig/TrackingSpace/CenterEyeAnchor").transform.GetComponent<Camera>();
        Image = GetComponent<Canvas>().transform.GetComponentInChildren<Image>();
        trigger = transform.parent.GetComponent<MicroscopeOverlayTrigger>();

        // disable the microscope overlay so the player can see their environment
        PlayerCamera.cullingMask = LayerMask.GetMask(
            "Default", "TransparentFX", 
            "Ignore Raycast", "Water", 
            "UI", "Fish", "Merd", 
            "UnderwaterShade", "Grabbable", "Player",
            "Menu", "Hand", "Bone");

        if (SystemInfo.deviceModel == "Oculus Quest")
        {
            AndroidJavaClass build = new AndroidJavaClass("android.os.Build");
            string device = build.GetStatic<string>("DEVICE");
            //Debug.Log(device);
            //Debug.Log(device.Contains("eureka"));
            if (device.Contains("eureka"))
            {
                Quest3 = true;
            }
                
        }
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

        //Debug.Log(Utils.GetSystemHeadsetType());
        //Debug.Log(SystemInfo.deviceModel);
        if (OverlayEnabled)
        {
            // roll math found at https://sunday-lab.blogspot.com/2008/04/get-pitch-yaw-roll-from-quaternion.html
            Quaternion rotation = HeadCollider.transform.localRotation;
            float roll = Mathf.Abs(Mathf.Atan2(2 * (rotation.x * rotation.y + rotation.w * rotation.z), rotation.w * rotation.w + rotation.x * rotation.x - rotation.y * rotation.y - rotation.z * rotation.z));
            
            float rotationOffset = Vector3.Angle((trigger.transform.position - HeadCollider.transform.position), HeadCollider.transform.forward);

            // dim overlay when head is rotated or rolled
            if (roll > 0.1f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), roll * 3f, 2f * Time.deltaTime));
            else if (rotationOffset > 20f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), rotationOffset * 0.02f, 2f * Time.deltaTime));
            else// if (roll <= 0.1f && rotationOffset <= 20f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), 0f, 2f * Time.deltaTime));

        }
    }

    public void EnableOverlay()
    {
        GetComponent<Canvas>().enabled = true;
        OverlayEnabled = true;

        //RawImage.texture = MicroscopeMonitor.GetComponentInChildren<RawImage>().texture;

        Texture texture = MicroscopeMonitor.GetComponentInChildren<RawImage>().texture;
        Image.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));

        // need this to correct image on Quest 2 (positioned and scaled too low on y-axis otherwise). not sure why these exact values work as they were discovered through trial and error.
        /*        float OffsetY = .25f;
                float ScaleModifierY = .85f;
                float ScaleModifierX = .85f;
                if (!Quest3)
                {
                    OffsetY = .265f;
                    ScaleModifierY = .9f;
                    ScaleModifierX = .8f;
                }*/

        //Debug.Log(MicroscopeMonitor.GetCurrentXY().x);
        //Debug.Log(MicroscopeMonitor.GetCurrentXY().y);

        //float NormalizedX = Normalize(MicroscopeMonitor.GetCurrentXY().x, 0f, 1f, 100f / MicroscopeMonitor.GetMagnificationLevel(), -100f / MicroscopeMonitor.GetMagnificationLevel());
        //float NormalizedY = Normalize(MicroscopeMonitor.GetCurrentXY().y, 0f, 1f, 100f / MicroscopeMonitor.GetMagnificationLevel(), -100f / MicroscopeMonitor.GetMagnificationLevel());
        //float NormalizedX = Normalize(MicroscopeMonitor.GetCurrentXY().x, 0f, 1f, 100f / MicroscopeMonitor.GetMagnificationLevel(), -100f / MicroscopeMonitor.GetMagnificationLevel());
        //float NormalizedY = Normalize(MicroscopeMonitor.GetCurrentXY().y, 0f, 1f, 100f / MicroscopeMonitor.GetMagnificationLevel(), -100f / MicroscopeMonitor.GetMagnificationLevel());

        //float NormalizedX = Normalize(MicroscopeMonitor.GetUVRect().x, 0f, 0.5f, 100f, -100f);
        //float NormalizedY = Normalize(MicroscopeMonitor.GetUVRect().y, 0f, 0.5f, 100f, -100f);

        //float NormalizedX = Normalize(MicroscopeMonitor.GetCurrentXY().x, 0f, 0.5f, 100f, -100f);
        float NormalizedX = Normalize(MicroscopeMonitor.GetCurrentXY().x, 0f, 1f, 100f, -100f) * 2; // normalize function seems wrong, but times 2 gives correct answer...
        Debug.Log(NormalizedX);

        NormalizedX = Normalize(NormalizedX, 100f, -100f, 25f * MicroscopeMonitor.GetMagnificationLevel(), -25f * MicroscopeMonitor.GetMagnificationLevel());   // normalize again to account for magnification
        
        float NormalizedY = (Normalize(MicroscopeMonitor.GetCurrentXY().y, 0f, 1f, 100f, -100f) * 2)/* * (1 / MicroscopeMonitor.GetAspectRatio() * 2)*/;
        NormalizedY = Normalize(NormalizedY, 100f, -100f, 25f * MicroscopeMonitor.GetMagnificationLevel(), -25f * MicroscopeMonitor.GetMagnificationLevel());

        Image.GetComponent<RectTransform>().localPosition = new Vector3(NormalizedX, NormalizedY, Image.GetComponent<RectTransform>().localPosition.z);

        //Image.GetComponent<RectTransform>().localPosition = new Vector3(-50f, NormalizedY, Image.GetComponent<RectTransform>().localPosition.z);

        int Scale = MicroscopeMonitor.GetMagnificationLevel();
        Image.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);

        // copy the microscope monitor's UVRect and revert aspect ratio corrections.
        /*        RawImage.uvRect = new Rect(
                    //MicroscopeMonitor.GetUVRect().x * ScaleModifierX,
                    MicroscopeMonitor.GetCurrentXY().x*//* - (MicroscopeMonitor.GetMagnification() / MicroscopeMonitor.GetAspectRatio() / OffsetY)*//*,
                    MicroscopeMonitor.GetCurrentXY().y - (MicroscopeMonitor.GetMagnification() * MicroscopeMonitor.GetAspectRatio() * OffsetY),
                    MicroscopeMonitor.GetUVRect().width*//* * ScaleModifierX*//*,
                    MicroscopeMonitor.GetUVRect().height * MicroscopeMonitor.GetAspectRatio() * ScaleModifierY);*/

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
        //RawImage.transform.Rotate(x, y, z);
    }

    public void SetHeadCollider(Collider HeadCollider)
    {
        this.HeadCollider = HeadCollider;
    }

    // found this function at https://stackoverflow.com/questions/51161098/normalize-range-100-to-100-to-0-to-3
    private float Normalize(float val, float valmin, float valmax, float min, float max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }
}
