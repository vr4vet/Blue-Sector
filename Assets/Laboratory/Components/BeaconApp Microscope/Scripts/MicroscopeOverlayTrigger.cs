using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MicroscopeOverlayTrigger : MonoBehaviour
{
    private Collider HeadCollider;
    private MicroscopeScreenSpaceOverlay MicroscopeOverlay;
    private BoxCollider BoxCollider;
    private PostProcessVolume VignetteVolume;
    private PostProcessVolume DarkenVolume;

    // Start is called before the first frame update
    void Start()
    {
        MicroscopeOverlay = GetComponentInChildren<MicroscopeScreenSpaceOverlay>();
        VignetteVolume = transform.Find("Vignette").GetComponent<PostProcessVolume>();
        DarkenVolume = transform.Find("Darken").GetComponent<PostProcessVolume>();

        BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            MicroscopeOverlay.SetHeadCollider(other); 
            MicroscopeOverlay.EnableOverlay();
            VignetteVolume.isGlobal = true;
            DarkenVolume.isGlobal = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            MicroscopeOverlay.SetHeadCollider(null);
            MicroscopeOverlay.DisableOverlay();
            VignetteVolume.isGlobal = false;
            DarkenVolume.isGlobal = false;
        }
    }

    public void AdjustDarkening(float adjustment)
    {
        DarkenVolume.weight = adjustment;
    }

    public float GetCurrentDarkening()
    {
        return DarkenVolume.weight;
    }
}
