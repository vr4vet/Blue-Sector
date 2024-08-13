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
        Debug.Log("Applying texture");
        if (SnapZone.HeldItem.gameObject.GetComponent<MicroscopeSlide>())
        {
            Debug.Log("Held item found!");
            SnapZone.HeldItem.gameObject.GetComponent<MicroscopeSlide>().SetMicroscopeMonitorTexture();
            SnapZone.HeldItem.gameObject.GetComponent<MicroscopeSlide>().SetMicroscopeSlide();
        }
    }
}
