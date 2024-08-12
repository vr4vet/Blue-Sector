using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeSlide : MonoBehaviour
{
    [SerializeField] private Texture texture;
    [SerializeField] private MicroscopeMonitor microscopeMonitor;

    public void SetMicroscopeMonitorTexture()
    {
        microscopeMonitor.SetTexture(texture);
    }

}
