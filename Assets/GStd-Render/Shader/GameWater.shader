//-----------------------------------------------------------------------------
// Copyright c 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//-----------------------------------------------------------------------------

Shader "Game/Water"
{
	Properties
	{
        // Basic.
        _SrcBlend("Alpha Source Blend", Float) = 1.0
        _DstBlend("Alpha Destination Blend", Float) = 0.0
		_WaveScale("Wave scale", Vector) = (0.2,0.15,0.73,0.76)
        _WaveSpeed("Wave speed", Vector) = (1.3,2.5,5.7,3.8)
		[NoScaleOffset] _BumpMap("Normalmap ", 2D) = "bump" {}
		[NoScaleOffset] _ReflectiveColor("Reflective color (RGB) fresnel (A) ", 2D) = "" {}

        // Refraction
		_RefractionDistort("Refraction Distort", Range(0, 0.5)) = 0.05
		_RefractionOpacity("Refraction Opacity", Range(0, 1.0)) = 0.5

        // Wave texture
		_WaveTex("Wave texture", 2D) = "white" {}
		_WaveDistort("Wave distort", Range(0,1.0)) = 0.25

        // Specular
		_SpecularDir("Specular direction", Vector) = (0,1,0,0)
		_SpecularPow("Specular power", Range(0.0, 20.0)) = 10.0
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		GrabPass{ "_Refraction" }

		Pass
		{
            Blend [_SrcBlend] [_DstBlend]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #pragma multi_compile_fwdbase
			#pragma multi_compile_fog
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _REFRACTION
            #pragma shader_feature _ _WAVE_TEXTURE
            #pragma shader_feature _ _SPECULAR
            #pragma shader_feature _ _VERTEX_COLOR

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half4 uv : TEXCOORD0;
				half3 normal : NORMAL;
#ifdef _VERTEX_COLOR
				half4 color : COLOR;
#endif
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half4 uv : TEXCOORD0;
				half2 waveUV : TEXCOORD1;
				half3 viewDir : TEXCOORD2;
#ifdef _SPECULAR
				half3 halfDir : TEXCOORD3;
#endif
#ifdef _REFRACTION
				half4 screenPos : TEXCOORD4;
#endif
#ifdef _VERTEX_COLOR
				half4 color : COLOR0;
#endif
				UNITY_FOG_COORDS(4)
			};

			half4 _WaveScale;
			half4 _WaveSpeed;

			sampler2D _BumpMap;
			sampler2D _ReflectiveColor;

			sampler2D _Refraction;
			half _RefractionDistort;
			half _RefractionOpacity;

			sampler2D _WaveTex;
			half4 _WaveTex_ST;
			half _WaveDistort;

			half3 _SpecularDir;
			half _SpecularPow;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				// scroll bump waves
				float4 wpos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = wpos.xzxz * _WaveScale + _WaveSpeed * _Time.x;
				o.waveUV = TRANSFORM_TEX(v.uv, _WaveTex);

				// object space view direction (will normalize per pixel)
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
#ifdef _SPECULAR
				o.halfDir = normalize(_SpecularDir + o.viewDir);
#endif
#ifdef _REFRACTION
				o.screenPos = ComputeScreenPos(o.vertex);
#endif
#ifdef _VERTEX_COLOR
				o.color = v.color;
#endif

				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				// normalize the view dir.
				i.viewDir = normalize(i.viewDir);

				// combine two scrolling bumpmaps into one
				half3 bump1 = UnpackNormal(tex2D(_BumpMap, i.uv.xy)).rgb;
				half3 bump2 = UnpackNormal(tex2D(_BumpMap, i.uv.zw)).rgb;
				half3 bump = (bump1 + bump2) * 0.5;

				// fresnel factor
				half fresnelFac = dot(i.viewDir, bump);

				// reflect.
				half3 viewReflect = reflect(-i.viewDir, bump);
				half4 refl = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, viewReflect);

				half4 water = tex2D(_ReflectiveColor, half2(fresnelFac, fresnelFac));
#ifdef _VERTEX_COLOR
				water.rgb *= i.color.rgb;
#endif
				half4 color = half4(
					lerp(water.rgb, refl.rgb, water.a), 
					refl.a * water.a);

				// wave texture.
#if _WAVE_TEXTURE
                half2 waveUV = i.waveUV + bump.xz * _WaveDistort;
                half4 waveColor = tex2D(_WaveTex, waveUV);
#   ifdef _VERTEX_COLOR
                waveColor *= 1 - i.color.a;
#   endif
				color += waveColor;
#endif

				// specular
#if _SPECULAR
				half nDotH = max(dot(bump, i.halfDir), 0.0);
				half3 specular = pow(nDotH, _SpecularPow);
				color.rgb += specular;
#endif

				// refraction
#if _REFRACTION
				half4 distort = half4(bump.xy * _RefractionDistort, 0, 0);
				half4 distortScreenPos = i.screenPos + distort;

				half4 refraction = tex2Dproj(
                    _Refraction, UNITY_PROJ_COORD(distortScreenPos));
#   ifdef _VERTEX_COLOR
                color = lerp(refraction, color, _RefractionOpacity * i.color.a);
#   else
                color = lerp(refraction, color, _RefractionOpacity);
#   endif
#elif defined(_VERTEX_COLOR)
                color.a *= i.color.a;
#endif

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, color);
				return color;
			}
			ENDCG
		}
	}

    CustomEditor "GameWaterShaderGUI"
}
