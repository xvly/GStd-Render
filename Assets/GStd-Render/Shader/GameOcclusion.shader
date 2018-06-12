//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

Shader "Game/Occlusion"
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
            "RenderType" = "Transparent"
            "Queue" = "AlphaTest+1"
        }

        UsePass "Game/Standard/SHADOWCASTER"

        Pass
        {
            Blend SrcAlpha One

            ZWrite off
            ztest greater
            Lighting off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #pragma multi_compile_instancing

            half4 _OccludeColor;
            half _OccludePower;

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = UnityObjectToClipPos(v.vertex);
                half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                half rim = 1 - saturate(dot(viewDir,v.normal));
                o.color = _OccludeColor * pow(rim, _OccludePower);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return i.color;
            }
            ENDCG
        }

        UsePass "Game/Standard/MAIN"
    }

    CustomEditor "GameStandardShaderGUI"
}
