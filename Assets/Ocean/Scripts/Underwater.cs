// Written by Trym Lund Flogard, 2023
// Licensed under MIT; see readme.
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(UnderwaterRenderer), PostProcessEvent.AfterStack, "Custom/Underwater")]
public sealed class Underwater : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Underwater effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    public ColorParameter waterColor = new ColorParameter { value = new Color(.1f, .68f, .72f) };
}
public sealed class UnderwaterRenderer : PostProcessEffectRenderer<Underwater>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Underwater"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetColor("_WaterColor", settings.waterColor);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

    public override DepthTextureMode GetCameraFlags()
    {
        // We use a depth map for simulating fog.
        return DepthTextureMode.Depth;
    }
}
