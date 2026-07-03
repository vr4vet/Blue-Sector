using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MicroscopeOverlayTrigger : MonoBehaviour
{
    private MicroscopeScreenSpaceOverlay MicroscopeOverlay;
    [SerializeField] private GameObject VignetteCanvas;
    [SerializeField] private Image Vignette;
    [SerializeField] private Image Overlay;
    private bool _cameraSet = false;

    public UnityEvent m_OnEyepiecesUsed;


    // Start is called before the first frame update
    void Start()
    {
        m_OnEyepiecesUsed ??= new UnityEvent();

        MicroscopeOverlay = GetComponentInChildren<MicroscopeScreenSpaceOverlay>();
    }

    private void Update()
    {
        // need to wait a bit before setting fetching the CenterEyeAnchor camera
        if (!_cameraSet)
        {
            if (VignetteCanvas.GetComponent<Canvas>().worldCamera = Camera.main)
                _cameraSet = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            MicroscopeOverlay.SetHeadCollider(other); 
            MicroscopeOverlay.EnableOverlay();
            VignetteCanvas.GetComponent<Canvas>().enabled = true;

            m_OnEyepiecesUsed.Invoke();
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            MicroscopeOverlay.SetHeadCollider(null);
            MicroscopeOverlay.DisableOverlay();
            VignetteCanvas.GetComponent<Canvas>().enabled = false;
        }
    }

    public void AdjustDarkening(float adjustment)
    {
        Color overlayColor = Overlay.color;
        overlayColor.a = adjustment;
        Overlay.color = overlayColor;
    }

    public float GetCurrentDarkening()
    {
        return Overlay.color.a;
    }
}
