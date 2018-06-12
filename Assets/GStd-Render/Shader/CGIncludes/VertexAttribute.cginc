//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef VERTEXATTRIBUTE_INCLUDED
#define VERTEXATTRIBUTE_INCLUDED

// Avaliable options:
// REQUIRE_VS_WORLD_POSITION
// REQUIRE_VS_WORLD_NORMAL
// REQUIRE_VS_WORLD_TANGENT
// REQUIRE_VS_AMBIENCE
// REQUIRE_VS_LIGHT_DIRECTION
// REQUIRE_VS_NDOTL
// REQUIRE_VS_VIEW_DIR

//------------------------------------------------------------------------------
// Macro dependency.
//------------------------------------------------------------------------------
#ifdef REQUIRE_VS_AMBIENCE
#   ifndef REQUIRE_VS_WORLD_NORMAL
#   define REQUIRE_VS_WORLD_NORMAL
#   endif
#endif

#ifdef REQUIRE_VS_NDOTL
#   ifndef REQUIRE_VS_WORLD_NORMAL
#   define REQUIRE_VS_WORLD_NORMAL
#   endif
#   ifndef REQUIRE_VS_LIGHT_DIRECTION
#   define REQUIRE_VS_LIGHT_DIRECTION
#   endif
#endif

#ifdef REQUIRE_VS_WORLD_BINORMAL
#   ifndef REQUIRE_VS_WORLD_NORMAL
#   define REQUIRE_VS_WORLD_NORMAL
#   endif

#   ifndef REQUIRE_VS_WORLD_TANGENT
#   define REQUIRE_VS_WORLD_TANGENT
#   endif
#endif

#ifdef REQUIRE_VS_WORLD_NORMAL
#   ifndef REQUIRE_VERTEXINPUT_NORMAL
#   define REQUIRE_VERTEXINPUT_NORMAL
#   endif
#endif

#ifdef REQUIRE_VS_WORLD_TANGENT
#   ifndef REQUIRE_VERTEXINPUT_TANGENT
#   define REQUIRE_VERTEXINPUT_TANGENT
#   endif
#endif

//------------------------------------------------------------------------------
// Includes
//------------------------------------------------------------------------------
#include "VertexInput.cginc"

//------------------------------------------------------------------------------
// Define the vertex data.
//------------------------------------------------------------------------------
struct VertexAttribute
{
#ifdef REQUIRE_VS_WORLD_POSITION
    float4 worldPosition;
#endif

#ifdef REQUIRE_VS_WORLD_NORMAL
    half3 worldNormal;
#endif

#ifdef REQUIRE_VS_WORLD_TANGENT
    half3 worldTangent;
#endif

#ifdef REQUIRE_VS_WORLD_BINORMAL
    half3 worldBinormal;
#endif

#ifdef REQUIRE_VS_AMBIENCE
    half3 ambience;
#endif

#ifdef REQUIRE_VS_LIGHT_DIRECTION
    half3 lightDirection;
#endif

#ifdef REQUIRE_VS_NDOTL
    half nDotL;
#endif

#ifdef REQUIRE_VS_VIEW_DIR
    half3 viewDir;
#endif
};

// Calculate the vertex attributes on vertex shader.
// @param a The attribute to calculate.
// @param v the vertex data.
VertexAttribute CalculateVertexAttribute(VertexInput v)
{
    VertexAttribute a;

#ifdef REQUIRE_VS_WORLD_POSITION
    a.worldPosition = mul(unity_ObjectToWorld, v.vertex);
#endif

#ifdef REQUIRE_VS_WORLD_NORMAL
    a.worldNormal = UnityObjectToWorldNormal(v.normal);
#endif

#ifdef REQUIRE_VS_WORLD_TANGENT
    a.worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
#endif

#ifdef REQUIRE_VS_WORLD_BINORMAL
    a.worldBinormal = cross(a.worldNormal, a.worldTangent) * \
        v.tangent.w * unity_WorldTransformParams.w;
#endif

#ifdef REQUIRE_VS_AMBIENCE
#   ifndef LIGHTMAP_ON
    a.ambience = ShadeSH9(half4(a.worldNormal, 1.0));
#   else
    a.ambience = half3(0.0, 0.0, 0.0);
#   endif
#endif

#ifdef REQUIRE_VS_LIGHT_DIRECTION
#   ifdef USING_DIRECTIONAL_LIGHT
    a.lightDirection = normalize(_WorldSpaceLightPos0.xyz);
#   else
    a.lightDirection = normalize(_WorldSpaceLightPos0.xyz - a.worldPosition);
#   endif
#endif

#ifdef REQUIRE_VS_NDOTL
    a.nDotL = max(dot(a.worldNormal, a.lightDirection), 0.0);
#endif

#ifdef REQUIRE_VS_VIEW_DIR
    a.viewDir = normalize(WorldSpaceViewDir(v.vertex));
#endif

    return a;
}  

#endif
