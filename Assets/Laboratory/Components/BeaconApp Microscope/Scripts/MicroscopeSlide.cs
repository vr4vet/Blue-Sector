using System.Collections.Generic;
using UnityEngine;

public class MicroscopeSlide : MonoBehaviour
{
    [SerializeField] private Sprite texture;
    [SerializeField] private MicroscopeMonitor microscopeMonitor;
    [SerializeField] public bool UseSeparateMagnificationTextures = false;
    [SerializeField] public List<Sprite> textures = new List<Sprite>();

    public void SetMicroscopeMonitorTexture()
    {
        if (UseSeparateMagnificationTextures)
            microscopeMonitor.SetTexture(textures[0]);
        else
            microscopeMonitor.SetTexture(texture);
    }

    public void SetMicroscopeSlide()
    {
        microscopeMonitor.SetCurrentSlide(this);
    }

    public void RemoveMicroscopeSlide()
    {
        microscopeMonitor.SetCurrentSlide(null);
    }

}
