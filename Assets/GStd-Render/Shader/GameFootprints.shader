//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

Shader "Game/Footprint"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "ForceNoShadowCasting" = "True"
        }

        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
            }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Offset -4, -4

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define _ALPHATEST_ON
            #define _ALPHABLEND_ON

            #define REQUIRE_VERTEXINPUT_UV0
            #define REQUIRE_VERTEXINPUT_UV1
            #include "CGIncludes/Core.cginc"

            #pragma multi_compile_fwdbase
            #pragma multi_compile_instancing
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile _ _VERTICAL_FOG
            #pragma skip_variants LIGHTMAP_ON
            #pragma skip_variants DIRLIGHTMAP_SEPARATE
            #pragma skip_variants DIRLIGHTMAP_COMBINED
            #pragma skip_variants DYNAMICLIGHTMAP_ON
            #pragma skip_variants VERTEXLIGHT_ON

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct v2f
            {
                float4 pos : SV_POSITION;
                half3 uv : TEXCOORD0;
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

                // Calculate position and uv.
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv0, _MainTex);

                // Calculate fadeout.
                half fadeout = (_Time.y - v.uv1.x) / v.uv1.y;
                o.uv.z = lerp(1.0, 0.0, clamp(fadeout, 0.0, 1.0));

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

                half4 mainColor = tex2D(_MainTex, i.uv.xy);
                ctx.albedo = mainColor.rgb;
                ctx.alpha *= mainColor.a * i.uv.z;

                // Apply the alpha.
                clip(ctx.alpha - 0.05);

                // Calcualte pixel light attributes.
                CALCULATE_PIXEL_LIGHT_ATTRIBUTE(a, i);

                // Calculate the light attenuation.
                ctx.atten = LIGHT_ATTENUATION(i);

                // Apply the lighting
                half4 finalColor = ApplyLighting(ctx, a);

                // Calculate fog for final color.
                ApplyVerticalFog(a, finalColor);
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }
}
