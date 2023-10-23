using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;

public class UnderwaterFog : MonoBehaviour
{
    //bool previousFog;
    [SerializeField] private bool enableEffects = false;
    [SerializeField] private Color fogColor = new Color(107, 204, 204, 255);
    [SerializeField] private float fogDensity;
    [SerializeField] private Color ambientLight;
    [SerializeField] private float haloStrength;
    [SerializeField] private float flareStrength;

    private Color previousFogColor;
    private float previousFogDensity;
    private Color previousAmbientLight;
    private float previousHaloStrength;
    private float previousFlareStrength;

    private void OnPreRender()
    {
        //previousFog = RenderSettings.fog;
        previousFogColor = RenderSettings.fogColor;
        previousFogDensity = RenderSettings.fogDensity;
        previousAmbientLight = RenderSettings.ambientLight;
        previousHaloStrength = RenderSettings.haloStrength;
        previousFlareStrength = RenderSettings.flareStrength;

        if (enableEffects)
        {
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.ambientLight = ambientLight;
            RenderSettings.haloStrength = haloStrength;
            RenderSettings.flareStrength = flareStrength;
        }

    }

    private void OnPostRender()
    {
        //RenderSettings.fog = previousFog;
        RenderSettings.fogColor = previousFogColor;
        RenderSettings.fogDensity = previousFogDensity;
        RenderSettings.ambientLight = previousAmbientLight;
        RenderSettings.haloStrength = previousHaloStrength;
        RenderSettings.flareStrength = previousFlareStrength;
    }

    public void EnableEffects() => enableEffects = true;
    public void DisableEffects() => enableEffects = false;
}
