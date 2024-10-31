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
    private GameObject Grid;
    private Collider HeadCollider;
    MicroscopeOverlayTrigger trigger;

    [SerializeField] private MicroscopeMonitor MicroscopeMonitor;

    private QuestDevice Device;
    private enum QuestDevice
    {
        Quest2, Quest3, QuestPro
    }

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

        // checking if system is Quest 2, Quest 3, or Quest Pro
        if (SystemInfo.deviceModel == "Oculus Quest")
        {
            AndroidJavaClass build = new AndroidJavaClass("android.os.Build");
            string device = build.GetStatic<string>("DEVICE");
            if (device.Contains("hollywood"))
                Device = QuestDevice.Quest2;
            else if (device.Contains("eureka"))
                Device = QuestDevice.Quest3;
            else if (device.Contains("seacliff"))
                Device = QuestDevice.QuestPro;
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
        //Debug.Log(PlayerController.name);
    }

    private void Update()
    {
        if (OverlayEnabled)
        {
            // Calculating roll. Code heavily based on https://github.com/fredsa/unity-1st-person-racing/blob/master/Assets/Standard%20Assets/Vehicles/Aircraft/Scripts/AeroplaneController.cs
            float roll = 0;
            Vector3 flatForward = HeadCollider.transform.forward;
            flatForward.y = 0;
            // If the flat forward vector is non-zero (which would only happen if the player's head was pointing exactly straight upwards)
            if (flatForward.sqrMagnitude > 0)
            {
                flatForward.Normalize();
                Vector3 flatRight = Vector3.Cross(Vector3.up, flatForward);
                Vector3 localFlatRight = HeadCollider.transform.InverseTransformDirection(flatRight);
                roll = Mathf.Abs(Mathf.Atan2(localFlatRight.y, localFlatRight.x));
            }

            // calculating the rotation offset from the eye pieces (other words: checking if the player looks directly into them)
            float rotationOffset = Vector3.Angle((trigger.transform.position - HeadCollider.transform.position), HeadCollider.transform.forward);

            // dim overlay when head is rotated or rolled
            if (roll > 0.1f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), roll * 3f, 2f * Time.deltaTime));
            else if (rotationOffset > 20f)
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), rotationOffset * 0.02f, 2f * Time.deltaTime));
            else
                trigger.AdjustDarkening(Mathf.Lerp(trigger.GetCurrentDarkening(), 0f, 2f * Time.deltaTime));
        }
    }

    public void EnableOverlay()
    {
        GetComponent<Canvas>().enabled = true;
        OverlayEnabled = true;

        // need this to correct image on Quest 3 and Quest Pro (positioned too high)
        float OffsetY = 0f;
        if (Device == QuestDevice.Quest2)
            OffsetY = -1f;
        else if (Device == QuestDevice.Quest3)
            OffsetY = -4f;
        else if (Device == QuestDevice.QuestPro)
            OffsetY = -5f;


        if (MicroscopeMonitor.IsDisplayingGrid())
        {
            Image.enabled = false;
            Grid = GameObject.Instantiate(MicroscopeMonitor.GetGrid());

            // fetch already generated rotations to ensure cells are rotated the same as on the monitor
            Grid.GetComponent<MicroscopeGrid>().RotateCells(MicroscopeMonitor.GetGridCellRotations());
            
            // make microscope overlay canvas visible only
            Grid.layer = LayerMask.NameToLayer("MicroscopeOverlay");

            // make this parent and give highest position among siblings to ensure UI elements are drawn on top
            Grid.transform.SetParent(transform);
            Grid.transform.SetAsFirstSibling();  

            RectTransform gridTransform = Grid.GetComponent<RectTransform>();

            // make overlay face player and make scale (1, 1, 1) before adjusting
            gridTransform.localEulerAngles = Vector3.zero;
            gridTransform.localScale = Vector3.one;

            // resize grid to match image and fit eye pieces
            float sizeRatio = gridTransform.sizeDelta.x / Image.GetComponent<RectTransform>().sizeDelta.x;
            gridTransform.localScale /= sizeRatio;

            // apply the same magnification as monitor
            gridTransform.localScale *= MicroscopeMonitor.GetMagnificationLevel();

            // fetch unscaled position of grid and adjust using current scaling 
            gridTransform.localPosition = new Vector3(MicroscopeMonitor.GetCurrentXY().x, MicroscopeMonitor.GetCurrentXY().y, 0) * gridTransform.localScale.x;
            gridTransform.localPosition += new Vector3(0, OffsetY, 0);
        }
        else
        {
            Image.enabled = true;
            Image.sprite = MicroscopeMonitor.GetImage();

            float ratio = MicroscopeMonitor.GetImageRectTransform().sizeDelta.x / Image.GetComponent<RectTransform>().sizeDelta.x;
            Image.GetComponent<RectTransform>().localPosition = (MicroscopeMonitor.GetImagePosition() + new Vector3(0f, OffsetY, 0f)) / ratio;

            int Scale = MicroscopeMonitor.GetMagnificationLevel();
            Image.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);
        }

        PlayerCamera.cullingMask = LayerMask.GetMask("MicroscopeOverlay");  // cull everything except for microscope overlay, making only that visible
    }

    public void DisableOverlay()
    {
        if (Grid  != null)
            GameObject.Destroy(Grid);

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
}
