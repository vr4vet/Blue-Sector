using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeOverlayTrigger : MonoBehaviour
{
    private MicroscopeScreenSpaceOverlay MicroscopeOverlay;
    // Start is called before the first frame update
    void Start()
    {
        MicroscopeOverlay = GetComponentInChildren<MicroscopeScreenSpaceOverlay>();
        Debug.Log(MicroscopeOverlay.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.name == "HeadCollision")
            MicroscopeOverlay.EnableOverlay();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollision")
            MicroscopeOverlay.DisableOverlay();

    }
}
