using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeSlide : MonoBehaviour
{
    [SerializeField] private Texture texture;
    [SerializeField] private MicroscopeMonitor microscopeMonitor;
    [SerializeField] public bool UseSeparateMagnificationTextures = false;
    [SerializeField] private Texture texture2x;
    [SerializeField] private Texture texture4x;
    [SerializeField] private Texture texture8x;
    [SerializeField] private Texture texture16x;

    public void SetMicroscopeMonitorTexture()
    {
        microscopeMonitor.SetTexture(texture);
    }

    public void SetMicroscopeSlide()
    {
        microscopeMonitor.SetCurrentSlide(this);
    }

}
