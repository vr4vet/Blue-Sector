using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeApplyTexture : MonoBehaviour
{
    private SnapZone SnapZone;
    // Start is called before the first frame update
    void Start()
    {
        SnapZone = GetComponent<SnapZone>();
    }


    public void ApplyTexture()
    {
        MicroscopeSlide CurrentMicroscopeSlide = SnapZone.HeldItem.gameObject.GetComponent<MicroscopeSlide>();
        
        if (CurrentMicroscopeSlide)
        {
            CurrentMicroscopeSlide.SetMicroscopeMonitorTexture();
            CurrentMicroscopeSlide.SetMicroscopeSlide();
        }
    }
}
