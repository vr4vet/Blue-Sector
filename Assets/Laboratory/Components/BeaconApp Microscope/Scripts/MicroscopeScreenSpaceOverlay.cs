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

        // checking if system is Quest 3
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

        Image.sprite = MicroscopeMonitor.GetImage();

        // need this to correct image on Quest 3 (positioned too high)
        float OffsetY = 0f;
        if (Quest3)
            OffsetY = -50f;

        float ratio = MicroscopeMonitor.GetImageRectTransform().sizeDelta.x / Image.GetComponent<RectTransform>().sizeDelta.x;
        Image.GetComponent<RectTransform>().localPosition = (MicroscopeMonitor.GetImagePosition() + new Vector3(0f, OffsetY, 0f)) / ratio;
        
        int Scale = MicroscopeMonitor.GetMagnificationLevel();
        Image.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);

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
