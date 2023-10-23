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
        // save normal scene fog values so OnPostRender() can make sure it is rendered normally from the player's view
        previousFogColor = RenderSettings.fogColor;
        previousFogDensity = RenderSettings.fogDensity;
        previousAmbientLight = RenderSettings.ambientLight;
        previousHaloStrength = RenderSettings.haloStrength;
        previousFlareStrength = RenderSettings.flareStrength;

        // the player head cam normally returns false here, to prevent underwater aesthetics when not in "immersive view"
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
        // restore fog settings so the world fog is not affected for the player
        RenderSettings.fogColor = previousFogColor;
        RenderSettings.fogDensity = previousFogDensity;
        RenderSettings.ambientLight = previousAmbientLight;
        RenderSettings.haloStrength = previousHaloStrength;
        RenderSettings.flareStrength = previousFlareStrength;
    }

    // used to enable effects on player head cam when teleporting to a fish cage (by grabbing the screen and entering "immersive view")
    public void EnableEffects() => enableEffects = true;
    public void DisableEffects() => enableEffects = false;
}
