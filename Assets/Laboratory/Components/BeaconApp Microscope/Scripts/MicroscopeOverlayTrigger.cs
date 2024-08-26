using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MicroscopeOverlayTrigger : MonoBehaviour
{
    private Collider HeadCollider;
    //private Vector3 HeadColliderImpactPosition;

    private MicroscopeScreenSpaceOverlay MicroscopeOverlay;
    private BoxCollider BoxCollider;
    private PostProcessVolume VignetteVolume;

    // Start is called before the first frame update
    void Start()
    {
        MicroscopeOverlay = GetComponentInChildren<MicroscopeScreenSpaceOverlay>();
        VignetteVolume = GetComponentInChildren<PostProcessVolume>();
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            HeadCollider = other; 
            MicroscopeOverlay.EnableOverlay();
            VignetteVolume.isGlobal = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollision")
        {
            HeadCollider = null;
            MicroscopeOverlay.DisableOverlay();
            VignetteVolume.isGlobal = false;
        }
    }
}
