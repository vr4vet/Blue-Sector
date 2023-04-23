Shader "Nature/FasterOcean"
{
	Properties
	{
		_BaseColor("Base color", COLOR) = (.54, .95, .99, 0.5)
		_WaterColor("Water color", COLOR) = (.54, .95, .99, 0.5)
		_ReflectionColor("Reflection color", COLOR) = (.54, .95, .99, 0.5)
		[NoScaleOffset] _Foam("Foam texture", 2D) = "white" {}
		[HideInInspector] world_light_dir("", VECTOR) = (0.0, 1.0, 0.8, 0.0)
	}

		CGINCLUDE
#include "UnityCG.cginc" 

        uniform half4 _BaseColor;
		uniform half4 _WaterColor;
		uniform half4 _ReflectionColor;

#define NB_WAVE 5
		half4 waves_p[NB_WAVE];
		half4 waves_d[NB_WAVE];

#define NB_INTERACTIONS 8
#define WAVE_DURATION 4.0
#define WAVE_SPEED 3.0
#define MAX_WAVE_AMP 0.5
		float4 interactions[NB_INTERACTIONS];

		uniform float4 world_light_dir;
		uniform float4 sun_color;

#define PI 3.14159234


		half evaluateWave(half4 wave_param, half4 wave_dir, half2 pos, float t)
		{
			return wave_param.y * sin(dot(wave_dir.xy, pos) * wave_param.x + t * wave_param.z);
		}

		float get_water_height(float3 p)
		{
			float height = 0.0;
			for (int i = 0; i < NB_WAVE; i++)
				height += evaluateWave(waves_p[i], waves_d[i], p.xz, _Time.y);
			return height;
		}

		float3 get_water_normal(float3 a)
		{
			const float eps = 0.01;
			float3 b = a + float3(eps, 0.0, 0.0);
			float3 c = a + float3(0.0, 0.0, eps);

			a.y = get_water_height(a);
			b.y = get_water_height(b);
			c.y = get_water_height(c);

			float3 n = normalize(cross(c - a, b - a));
			return n;
		}

		float hash(float2 p)
		{
			float h = dot(p, float2(127.1, 311.7));
			return frac(sin(h) * 43758.5453123);
		}

		float noise2(in float2 p)
		{
			fixed2 i = floor(p);
			float2 f = frac(p);
			float2 u = f * f * (3.0 - 2.0 * f);
			return -1.0 + 2.0 * lerp(lerp(hash(i + float2(0.0, 0.0)),
				hash(i + float2(1.0, 0.0)), u.x),
				lerp(hash(i + float2(0.0, 1.0)),
					hash(i + float2(1.0, 1.0)), u.x), u.y);
		}

		float sea_octave(float2 uv, float choppy)
		{
			uv += noise2(uv);
			float2 wv = 1.0 - abs(sin(uv));
			float2 swv = abs(cos(uv));
			wv = lerp(wv, swv, wv);
			return pow(1.0 - pow(wv.x * wv.y, 0.65), choppy);
		}

		float map_detailed(float3 p)
		{
			half freq = 0.16;
			half amp = 0.6;
			half choppy = 4.0;
			float2 uv = p.xz;
			uv.x *= 0.75;

			half d, h = 0.0;
			for (int i = 0; i < 5; i++)
			{
				d = sea_octave((uv + _Time.yy) * freq, choppy);
				d += sea_octave((uv - _Time.yy) * freq, choppy);
				h += d * amp;
				uv = float2(uv.x * 1.6 + 1.2 * uv.y, uv.x * -1.2 + 1.6 * uv.y);
				freq *= 1.9;
				amp *= 0.22;
				choppy = lerp(choppy, 1.0, 0.2);
			}

			return p.y - h;
		}

		float3 get_detailed_normal(float3 a, float eps)
		{
			float3 b = a + float3(eps, 0.0, 0.0);
			float3 c = a + float3(0.0, 0.0, eps);

			a.y = map_detailed(a);
			b.y = map_detailed(b);
			c.y = map_detailed(c);

			float3 n = normalize(cross(b - a, c - a));
			n.y *= -1;
			return n;
		}

		const half3 getSkyColor(float e)
		{
			const float ey = 1.0 - max(e, 0.0);
			return half3(ey * ey, ey, 0.6 + (ey) * 0.4);
		}

		half diffuse(half3 n, half3 l, half p)
		{
			return pow(dot(n, l) * 0.4 + 0.6, p);
		}

		float specular(float3 n, float3 l, float3 e, float s)
		{
			float nrm = (s + 8.0) / (3.1415 * 8.0);
			return pow(max(dot(reflect(e, n), l), 0.0), s) * nrm;
		}

		inline void ComputeScreenAndGrabPassPos(float4 pos, out float4 screenPos, out float4 grabPassPos)
		{
#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
#else
			float scale = 1.0f;
#endif

			screenPos = ComputeScreenPos(pos);
			grabPassPos.xy = (float2(pos.x, pos.y * scale) + pos.w) * 0.5;
			grabPassPos.zw = pos.zw;
		}

		struct appdata
		{
			float4 vertex : POSITION;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 normal : NORMAL;
			float3 world_position : TEXCOORD0;
			float4 ref : TEXCOORD1;
			float4 grabPassPos : TEXCOORD2;
			UNITY_FOG_COORDS(5)
		};

		v2f vert(appdata v)
		{
			v2f o;

			float4 world_position = mul(unity_ObjectToWorld, v.vertex);
			world_position.y = get_water_height(world_position.xyz);

			float interactive = 0.0;
			for (int i = 0; i < NB_INTERACTIONS; i++)
			{
				half dist = distance(world_position.xz, interactions[i].xy);
				half elapsed = (_Time.y - interactions[i].w);
				half computed_distance = elapsed * WAVE_SPEED;
				half computed_distance_abs = abs(computed_distance - dist);
				half power = 1.0 - saturate(computed_distance_abs * computed_distance_abs * 0.3);
				power *= 1.0 - saturate(elapsed / WAVE_DURATION);
				dist += 2.0;
				interactive += power * interactions[i].z;
			}
			world_position.y += clamp(interactive, -MAX_WAVE_AMP, MAX_WAVE_AMP);

			o.world_position = world_position;
			o.normal = get_water_normal(world_position.xyz);
			o.pos = mul(UNITY_MATRIX_VP, world_position);

			ComputeScreenAndGrabPassPos(o.pos, o.ref, o.grabPassPos);
			UNITY_TRANSFER_FOG(o, o.pos);

			return o;
		}

#define REFLECTION
#define SPECULAR

		half4 frag(v2f i) : SV_Target
		{
			float3 eye_vector = i.world_position.xyz - _WorldSpaceCameraPos;
			float3 eye = normalize(eye_vector);

			half3 detail_normal = get_detailed_normal(i.world_position, 0.01 * pow(length(eye_vector), 0.8));
			half3 normal = normalize(detail_normal);

			float3 light_direction = normalize(world_light_dir);
			float3 l = normalize(light_direction);
			half4 baseColor;
			baseColor.a = 1.0;
			//baseColor.rgb=normal;
			//return baseColor;
#ifdef REFLECTION
			{

				half fresnel = clamp(1.0 - dot(normal, -half3(eye)), 0.0, 1.0);
				fresnel = (fresnel * fresnel) * (fresnel * 0.65);
				fresnel = pow(fresnel, 0.8) * 0.8;

				const half3 reflected = getSkyColor(reflect(eye, normal).y) * _ReflectionColor;
				const half3 refracted = _BaseColor + diffuse(normal, l, 80.0) * _WaterColor * 0.12;
				baseColor.rgb = lerp(refracted, reflected, fresnel);
			}


#endif // REFLECTION


			/* Possible depth color */
			{
				float atten = max(1.0 - dot(eye, eye) * 0.001, 0.0);
				baseColor.rgb += _WaterColor * (i.world_position.y + 0.6) * 0.18 * atten;
			}

#ifdef SPECULAR
			{
				float spec = specular(i.normal * 0.2 + detail_normal * 0.8, l, eye, 60.0);
				baseColor.rgb += sun_color * spec;
			}
#endif // SPECULAR

			UNITY_APPLY_FOG(i.fogCoord, baseColor);

			baseColor = saturate(baseColor);

			baseColor.a = 1.0;

			return baseColor;
		}
			ENDCG

			Subshader
		{
			Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
				Lod 300
				//ColorMask RGBA
				Pass
			{
			//ZTest LEqual
			ZWrite Off // we're not writing into the depth texture
			//Cull Off

			CGPROGRAM

			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_fog_exp2
			#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
			}
		}
		Fallback "Transparent/Diffuse"
}
