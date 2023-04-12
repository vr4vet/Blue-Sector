// Written by Trym Lund Flogard, 2023
// Licensed under MIT; see readme.
Shader "Hidden/Custom/Underwater"
{
	HLSLINCLUDE
		// StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

#define DARKNESS 0.9

	// Lerp the pixel color with the water color using the _Blend uniform.
	float _Blend;
	float4 _WaterColor;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		// Get the color and distance
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float distance = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r;

		// Make the foreground darker
		color.rgb *= DARKNESS;

		// Blend with the water color
		color.rgb = lerp(_WaterColor.rgb, color.rgb, min(1, sqrt(distance * 1/_Blend) * 1/_Blend));

		// Return the result
		return color;
	}
	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always
			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDefault
				#pragma fragment Frag
			ENDHLSL
		}
	}
}