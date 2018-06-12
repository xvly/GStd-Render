//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

Shader "Game/Standard"
{
    Properties
    {
        // Rendering mode.
        _RenderingMode("Rendering Mode", Float) = 0.0
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.1
        _SrcBlend("Alpha Source Blend", Float) = 0.0
        _DstBlend("Alpha Destination Blend", Float) = 0.0
        _ZWrite("Z Write", Float) = 1.0

        // Albedo
        _AlbedoTex("Albedo Texture", 2D) = "white" {}
        _AlbedoColor("Albedo Color", Color) = (1,1,1,1)

        // Normal
        _NormalTex("Normal Texture", 2D) = "bump" {}
        _NormalScale("Normal Scale", Float) = 1.0

        // Mask
        _MaskTex("Mask Texture", 2D) = "white" {}

        // Detail
        _DetailTex("Detail Texture", 2D) = "white" {}
        _DetailColor("Detail Color", Color) = (1,1,1,1)
        _DetailUVSpeed("UV Speed", Vector) = (0, 0, 0, 0)

        // Emission
        [HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)

        // PBR Lighting
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _ReflectionColor("Reflection Color", Color) = (1,1,1,1)

        // Rim
        _RimColor("Rim Color (A)Opacity", Color) = (1,1,1,1)
        _RimIntensity("Rim Intensity", Range(0, 10)) = 1
        _RimFresnel("Rim Fresnel", Range(0, 5)) = 1

        // Rim Light
        _RimLightColor("Rim Light Color (A)Opacity", Color) = (1,1,1,1)
        _RimLightIntensity("Rim Light Intensity", Range(0, 10)) = 1
        _RimLightFresnel("Rim Light Fresnel", Range(0, 5)) = 1

        // The occulde color
        _OccludeColor("Occlusion Color", Color) = (0,0,1,1)
        _OccludePower("Occlusion Power", Range(0.1, 10)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            CGPROGRAM
            #pragma vertex vert   
            #pragma fragment frag

            #include "UnityCG.cginc"

            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _ _ALBEDO_COLOR
            #pragma skip_variants LIGHTMAP_ON
            #pragma skip_variants DIRLIGHTMAP_SEPARATE
            #pragma skip_variants DIRLIGHTMAP_COMBINED
            #pragma skip_variants DYNAMICLIGHTMAP_ON
            #pragma skip_variants VERTEXLIGHT_ON

            #if defined(_ALPHATEST_ON) || defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
            #   define REQUIRE_SHADOW_ALPHA
            #endif

            fixed _Cutoff;

            sampler2D _AlbedoTex;
            half4 _AlbedoTex_ST;
            fixed4 _AlbedoColor;

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
#ifdef REQUIRE_SHADOW_ALPHA
                half2 uv : TEXCOORD0;
#endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
#ifdef REQUIRE_SHADOW_ALPHA
                half2 uv : TEXCOORD1;
#endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)

#ifdef REQUIRE_SHADOW_ALPHA
                o.uv = TRANSFORM_TEX(v.uv, _AlbedoTex);
#endif

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
#ifdef REQUIRE_SHADOW_ALPHA
                fixed4 col = tex2D(_AlbedoTex, i.uv);
#   ifdef _ALBEDO_COLOR
                col *= _AlbedoColor;
#   endif

#   ifdef _ALPHAPREMULTIPLY_ON
                col.rgb *= col.a;
#   endif

#   if !defined(_ALPHATEST_ON) && \
       !defined(_ALPHABLEND_ON) && \
       !defined(_ALPHAPREMULTIPLY_ON)
                UNITY_OPAQUE_ALPHA(col.a);
#   endif

#   if defined(_ALPHATEST_ON)
                clip(col.a - _Cutoff);
#   endif
#endif
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        Pass
        {
            Name "Main"
            Tags
            {
                "LightMode" = "ForwardBase"
            }

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "CGIncludes/Core.cginc"

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _ _ALBEDOMAP
            #pragma shader_feature _ _ALBEDO_COLOR
            #pragma shader_feature _ _SMOOTHNESS_ALPHA _METALLIC_ALPHA _EMISSION_ALPHA _DETAIL_ALPHA
            #pragma shader_feature _ _NORMALMAP
            #pragma shader_feature _ _MASKMAP
            #pragma shader_feature _ _DETAIL_MULX2 _DETAIL_MUL _DETAIL_ADD _DETAIL_LERP
            #pragma shader_feature _ _DETAIL_UV1
            #pragma shader_feature _ _DETAIL_COLOR
            #pragma shader_feature _ _DETAIL_ANIMATION
            #pragma shader_feature _ _EMISSION
            #pragma shader_feature _ _LIGHTING_DIFFUSE _LIGHTING_PBR
            #pragma shader_feature _ _RIM_COLOR
            #pragma shader_feature _ _RIM_LIGHT
            #pragma multi_compile _ _VERTICAL_FOG
            #pragma skip_variants DIRLIGHTMAP_SEPARATE
            #pragma skip_variants DIRLIGHTMAP_COMBINED
            #pragma skip_variants DYNAMICLIGHTMAP_ON
            #pragma skip_variants VERTEXLIGHT_ON

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                UNITY_POSITION(pos);
                V2F_VERTEX_ATTRIBUTE(TEXCOORD0, TEXCOORD1, TEXCOORD2, \
                    TEXCOORD3, TEXCOORD4, COLOR0, COLOR1)
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

                // Transfer data to pixel shader.
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

                // Apply features.
                ApplyAlbedo(ctx, a);
                ApplyAlpha(ctx);
                ApplyNormal(a);
                ApplyMask(ctx, a);

                // Calcualte pixel light attributes.
                CALCULATE_PIXEL_LIGHT_ATTRIBUTE(a, i);

                // Calculate the light attenuation.
                ctx.atten = LIGHT_ATTENUATION(i);

                // Calculate lighting.
                ApplyEmission(ctx);
                half4 finalColor = ApplyLighting(ctx, a);

                // Calculate fog and rim for final color.
                ApplyDetail(finalColor, a);
                ApplyRim(finalColor, a);
                ApplyVerticalFog(a, finalColor);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }

    CustomEditor "GameStandardShaderGUI"
}
