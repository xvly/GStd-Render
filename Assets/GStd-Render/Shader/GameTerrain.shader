//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

Shader "Game/Terrain"
{
    Properties
    {
        // Textures.
        _Control("Control (RGBA)", 2D) = "white" {}

        _Splat1("Layer 1", 2D) = "white" {}
        _Splat1_Normal("Normal 1", 2D) = "bump" {}

        _Splat2("Layer 2", 2D) = "white" {}
        _Splat2_Normal("Normal 2", 2D) = "bump" {}

        _Splat3("Layer 3", 2D) = "white" {}
        _Splat3_Normal("Normal 3", 2D) = "bump" {}

        _Splat4("Layer 4", 2D) = "white" {}
        _Splat4_Normal("Normal 4", 2D) = "bump" {}

        // Emission
        _EmissionColor("Emission Color", Color) = (0,0,0,0)

        // PBR Lighting
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _ReflectionColor("Reflection Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        UsePass "Game/Standard/SHADOWCASTER"

        Pass
		{
			Tags
            {
                "LightMode" = "ForwardBase"
            }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #define REQUIRE_VERTEXINPUT_UV0
            #if defined(_NORMAL1) || defined(_NORMAL2) || defined(_NORMAL3) || defined(_NORMAL4)
            #   define REQUIRE_PS_WORLD_NORMAL
            #   define REQUIRE_PS_WORLD_TANGENT
            #   define REQUIRE_PS_WORLD_BINORMAL
            #   define REQUIRE_PS_NDOTLP
            #endif

            #include "CGIncludes/Core.cginc"

			#pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
			#pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _LAYER3 _LAYER4
            #pragma shader_feature _ _NORMAL1
            #pragma shader_feature _ _NORMAL2
            #pragma shader_feature _ _NORMAL3
            #pragma shader_feature _ _NORMAL4
            #pragma shader_feature _ _SMOOTHNESS_ALPHA _METALLIC_ALPHA
            #pragma shader_feature _ _LIGHTING_DIFFUSE _LIGHTING_PBR
            #pragma shader_feature _ _EMISSION
            #pragma multi_compile _ _VERTICAL_FOG
            #pragma skip_variants DIRLIGHTMAP_SEPARATE
            #pragma skip_variants DIRLIGHTMAP_COMBINED
            #pragma skip_variants DYNAMICLIGHTMAP_ON
            #pragma skip_variants VERTEXLIGHT_ON

            sampler2D _Control;

            sampler2D _Splat1;
            half4 _Splat1_ST;
            sampler2D _Splat1_Normal;

            sampler2D _Splat2;
            half4 _Splat2_ST;
            sampler2D _Splat2_Normal;

            sampler2D _Splat3;
            half4 _Splat3_ST;
            sampler2D _Splat3_Normal;

            sampler2D _Splat4;
            half4 _Splat4_ST;
            sampler2D _Splat4_Normal;

			struct v2f
			{
                float4 pos : SV_POSITION;

#if defined(_LAYER4)
                half2 uv0 : TEXCOORD0;
                half4 uv1 : TEXCOORD1;
                half4 uv2 : TEXCOORD2;
#elif defined(_LAYER3)
                half4 uv0 : TEXCOORD0;
                half4 uv1 : TEXCOORD1;
#else
                half2 uv0 : TEXCOORD0;
                half4 uv1 : TEXCOORD2;
#endif

                V2F_VIEW_DIR(TEXCOORD3)
                V2F_WORLD_AMBIENCE(TEXCOORD4)
                V2F_WORLD_NORMAL(COLOR0)
                V2F_WORLD_POSITION(COLOR1)
                V2F_WORLD_TANGENT(COLOR2)
                V2F_WORLD_BINORMAL(COLOR3)

                LIGHTING_COORDS(5, 6)
                UNITY_FOG_COORDS(7)
                UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(VertexInput v)
			{
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                // Calculate position.
                o.pos = UnityObjectToClipPos(v.vertex);

                // Calculate vertex attributes.
                VertexAttribute a = CalculateVertexAttribute(v);

                // Calculate uv for terrain.
#if defined(_LAYER4)
                o.uv0 = v.uv0;
                o.uv1 = half4(
                    TRANSFORM_TEX(v.uv0, _Splat1),
                    TRANSFORM_TEX(v.uv0, _Splat2));
                o.uv2 = half4(
                    TRANSFORM_TEX(v.uv0, _Splat3),
                    TRANSFORM_TEX(v.uv0, _Splat4));
#elif defined(_LAYER3)
                o.uv0 = half4(v.uv0,
                    TRANSFORM_TEX(v.uv0, _Splat1));
                o.uv1 = half4(
                    TRANSFORM_TEX(v.uv0, _Splat2),
                    TRANSFORM_TEX(v.uv0, _Splat3));
#else
                o.uv0 = v.uv0;
                o.uv1 = half4(
                    TRANSFORM_TEX(v.uv0, _Splat1),
                    TRANSFORM_TEX(v.uv0, _Splat2));
#endif

                TRANSFER_VERTEX_ATTRIBUTE(o, a, v);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
                // Calcualte pixel geometry attributes.
                PixelAttribute a;
                CALCULATE_PIXEL_GEO_ATTRIBUTE(a, i);

                // Prepare the light context.
                LightContext ctx;
                LIGHT_CONTEXT_INITIALIZE(ctx);

                // Calculate the layer.
#if defined(_LAYER4)
                half2 uv0 = i.uv0;
                half2 uv1 = i.uv1.xy;
                half2 uv2 = i.uv1.zw;
                half2 uv3 = i.uv2.xy;
                half2 uv4 = i.uv2.zw;

                half4 control = tex2D(_Control, uv0);
                half4 layer1 = tex2D(_Splat1, uv1);
                half4 layer2 = tex2D(_Splat2, uv2);
                half4 layer3 = tex2D(_Splat3, uv3);
                half4 layer4 = tex2D(_Splat4, uv4);

                half4 mix = (layer1 * control.r + layer2 * control.g + layer3 * control.b + layer4 * control.a);
                ctx.albedo = mix.rgb;
                ctx.alpha = mix.a;

#if defined(_NORMAL1) || defined(_NORMAL2) || defined(_NORMAL3) || defined(_NORMAL4)
                half4 normal = half4(0, 0, 0, 0);
#   ifdef _NORMAL1
                normal += tex2D(_Splat1_Normal, uv1) * control.r;
#	else
				normal += half4(0.5, 0.5, 1, 0) * control.r;
#   endif

#   ifdef _NORMAL2
                normal += tex2D(_Splat2_Normal, uv2) * control.g;
#	else
				normal += half4(0.5, 0.5, 1, 0) * control.g;
#   endif

#   ifdef _NORMAL3
                normal += tex2D(_Splat3_Normal, uv3) * control.b;
#	else
				normal += half4(0.5, 0.5, 1, 0) * control.b;
#   endif

#   ifdef _NORMAL4
                normal += tex2D(_Splat4_Normal, uv4) * control.a;
#	else
				normal += half4(0.5, 0.5, 1, 0) * control.a;
#   endif

                half3 mixedNormal = UnpackNormal(normal);
                half3 worldNormal = a.worldTangent * mixedNormal.x +
                    a.worldBinormal * mixedNormal.y +
                    a.worldNormal * mixedNormal.z;
                a.worldNormal = normalize(worldNormal.xyz);
#endif

#elif defined(_LAYER3)
                half2 uv0 = i.uv0.xy;
                half2 uv1 = i.uv0.zw;
                half2 uv2 = i.uv1.xy;
                half2 uv3 = i.uv1.zw;

                half3 control = tex2D(_Control, uv0);
                half4 layer1 = tex2D(_Splat1, uv1);
                half4 layer2 = tex2D(_Splat2, uv2);
                half4 layer3 = tex2D(_Splat3, uv3);

                half4 mix = (layer1 * control.r + layer2 * control.g + layer3 * control.b);
                ctx.albedo = mix.rgb;
                ctx.alpha = mix.a;

#if defined(_NORMAL1) || defined(_NORMAL2) || defined(_NORMAL3)
                half4 normal = half4(0, 0, 0, 0);
#   ifdef _NORMAL1
                normal += tex2D(_Splat1_Normal, uv1) * control.r;
#   endif

#   ifdef _NORMAL2
                normal += tex2D(_Splat2_Normal, uv2) * control.g;
#   endif

#   ifdef _NORMAL3
                normal += tex2D(_Splat3_Normal, uv3) * control.b;
#   endif

                half3 mixedNormal = UnpackNormal(normal);
                half3 worldNormal = a.worldTangent * mixedNormal.x +
                    a.worldBinormal * mixedNormal.y +
                    a.worldNormal * mixedNormal.z;
                a.worldNormal = normalize(worldNormal.xyz);
#endif

#else
                half2 uv0 = i.uv0;
                half2 uv1 = i.uv1.xy;
                half2 uv2 = i.uv1.zw;

                half2 control = tex2D(_Control, uv0);
                half4 layer1 = tex2D(_Splat1, uv1);
                half4 layer2 = tex2D(_Splat2, uv2);

                half4 mix = (layer1 * control.r + layer2 * control.g);
                ctx.albedo = mix.rgb;
                ctx.alpha = mix.a;

#if defined(_NORMAL1) || defined(_NORMAL2)
                half4 normal = half4(0, 0, 0, 0);
#   ifdef _NORMAL1
                normal += tex2D(_Splat1_Normal, uv1) * control.r;
#   endif

#   ifdef _NORMAL2
                normal += tex2D(_Splat2_Normal, uv2) * control.g;
#   endif

                half3 mixedNormal = UnpackNormal(normal);
                half3 worldNormal = a.worldTangent * mixedNormal.x +
                    a.worldBinormal * mixedNormal.y +
                    a.worldNormal * mixedNormal.z;
                a.worldNormal = normalize(worldNormal.xyz);
#endif

#endif

                // Calculate the lighting parameters.
#ifdef _SMOOTHNESS_ALPHA
                ctx.smoothness = ctx.alpha;
#else
                ctx.smoothness = 1.0;
#endif

#ifdef _METALLIC_ALPHA
                ctx.metallic = ctx.alpha;
#else
                ctx.metallic = 1.0;
#endif

#if _EMISSION
#   ifdef _EMISSION_ALPHA
                ctx.emission = half3(ctx.alpha, ctx.alpha, ctx.alpha);
#   else
                ctx.emission = half3(1, 1, 1);
#   endif
#endif

                // Calcualte pixel light attributes.
                CALCULATE_PIXEL_LIGHT_ATTRIBUTE(a, i);

                // Calculate the light attenuation.
                ctx.atten = LIGHT_ATTENUATION(i);

                // Calculate lighting.
                ApplyEmission(ctx);
                half4 finalColor = ApplyLighting(ctx, a);

                // Calculate fog for final color.
                ApplyVerticalFog(a, finalColor);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
			}
            ENDCG
        }
    }

    CustomEditor "GameTerrainShaderGUI"
}
