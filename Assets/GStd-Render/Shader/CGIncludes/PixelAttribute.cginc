//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef PIXELATTRIBUTE_INCLUDED
#define PIXELATTRIBUTE_INCLUDED

// Avaliable options:
// REQUIRE_PS_UV0
// REQUIRE_PS_UV1
// REQUIRE_PS_WORLD_POSITION
// REQUIRE_PS_WORLD_NORMAL
// REQUIRE_PS_WORLD_TANGENT
// REQUIRE_PS_WORLD_BINORMAL
// REQUIRE_PS_AMBIENCE
// REQUIRE_PS_LIGHT_DIRECTION
// REQUIRE_PS_NDOTL || REQUIRE_PS_NDOTLP
// REQUIRE_PS_VIEW_DIR
// REQUIRE_PS_HALF_DIR
// REQUIRE_PS_NDOTV
// REQUIRE_PS_NDOTH
// REQUIRE_PS_HDOTV
// REQUIRE_PS_LDOTH
// REQUIRE_PS_VIEW_REFLECT

//------------------------------------------------------------------------------
// Macro dependency.
//------------------------------------------------------------------------------
#ifdef REQUIRE_PS_LDOTH
#   ifndef REQUIRE_PS_LIGHT_DIRECTION
#   define REQUIRE_PS_LIGHT_DIRECTION
#   endif
#   ifndef REQUIRE_PS_HALF_DIR
#   define REQUIRE_PS_HALF_DIR
#   endif
#endif

#ifdef REQUIRE_PS_HDOTV
#   ifndef REQUIRE_PS_HALF_DIR
#   define REQUIRE_PS_HALF_DIR
#   endif
#   ifndef REQUIRE_PS_VIEW_DIR
#   define REQUIRE_PS_VIEW_DIR
#   endif
#endif

#ifdef REQUIRE_PS_NDOTH
#   ifndef REQUIRE_PS_WORLD_NORMAL
#   define REQUIRE_PS_WORLD_NORMAL
#   endif
#   ifndef REQUIRE_PS_HALF_DIR
#   define REQUIRE_PS_HALF_DIR
#   endif
#endif

#ifdef REQUIRE_PS_NDOTV
#   ifndef REQUIRE_PS_WORLD_NORMAL
#   define REQUIRE_PS_WORLD_NORMAL
#   endif
#   ifndef REQUIRE_PS_VIEW_DIR
#   define REQUIRE_PS_VIEW_DIR
#   endif
#endif

#ifdef REQUIRE_PS_AMBIENCE
#   ifndef REQUIRE_VS_AMBIENCE
#   define REQUIRE_VS_AMBIENCE
#   endif
#endif

#ifdef REQUIRE_PS_NDOTLP
#   ifndef REQUIRE_PS_LIGHT_DIRECTION
#   define REQUIRE_PS_LIGHT_DIRECTION
#   endif
#   ifndef REQUIRE_PS_WORLD_NORMAL
#   define REQUIRE_PS_WORLD_NORMAL
#   endif
#	ifdef REQUIRE_PS_NDOTL
#	undef REQUIRE_PS_NDOTL
#	endif
#elif defined(REQUIRE_PS_NDOTL)
#   ifndef REQUIRE_VS_NDOTL
#   define REQUIRE_VS_NDOTL
#   endif
#   ifndef REQUIRE_VS_LIGHT_DIRECTION
#   define REQUIRE_VS_LIGHT_DIRECTION
#   endif
#endif

#ifdef REQUIRE_PS_WORLD_POSITION
#   ifndef REQUIRE_VS_WORLD_POSITION
#   define REQUIRE_VS_WORLD_POSITION
#   endif
#endif

#ifdef REQUIRE_PS_WORLD_NORMAL
#   ifndef REQUIRE_VS_WORLD_NORMAL
#   define REQUIRE_VS_WORLD_NORMAL
#   endif
#endif

#ifdef REQUIRE_PS_WORLD_TANGENT
#   ifndef REQUIRE_VS_WORLD_TANGENT
#   define REQUIRE_VS_WORLD_TANGENT
#   endif
#endif

#ifdef REQUIRE_PS_WORLD_BINORMAL
#   ifndef REQUIRE_VS_WORLD_BINORMAL
#   define REQUIRE_VS_WORLD_BINORMAL
#   endif
#endif

#ifdef REQUIRE_PS_HALF_DIR
#   ifndef REQUIRE_PS_VIEW_DIR
#   define REQUIRE_PS_VIEW_DIR
#   endif
#endif

#ifdef REQUIRE_PS_VIEW_DIR
#   ifndef REQUIRE_PS_LIGHT_DIRECTION
#   define REQUIRE_PS_LIGHT_DIRECTION
#   endif
#   ifndef REQUIRE_VS_VIEW_DIR
#   define REQUIRE_VS_VIEW_DIR
#   endif
#endif

#ifdef REQUIRE_PS_VIEW_REFLECT
#   ifndef REQUIRE_VS_VIEW_DIR
#   define REQUIRE_VS_VIEW_DIR
#   endif
#endif

#ifdef REQUIRE_PS_UV0
#   ifndef REQUIRE_VERTEXINPUT_UV0
#   define REQUIRE_VERTEXINPUT_UV0
#   endif
#endif

#ifdef REQUIRE_PS_UV1
#   ifndef REQUIRE_VERTEXINPUT_UV1
#   define REQUIRE_VERTEXINPUT_UV1
#   endif
#endif

//------------------------------------------------------------------------------
// Transfer requirement values from vertex to fragment shader.
//------------------------------------------------------------------------------
struct PixelAttribute
{
#ifdef REQUIRE_PS_UV0
    half2 uv0;
#endif

#ifdef REQUIRE_PS_UV1
    half2 uv1;
#endif

#ifdef REQUIRE_PS_WORLD_POSITION
    float3 worldPosition;
#endif

#ifdef REQUIRE_PS_WORLD_NORMAL
    half3 worldNormal;
#endif

#ifdef REQUIRE_PS_WORLD_TANGENT
    half3 worldTangent;
#endif

#ifdef REQUIRE_PS_WORLD_BINORMAL
    half3 worldBinormal;
#endif

#ifdef REQUIRE_PS_AMBIENCE
    half3 ambience;
#endif

#ifdef REQUIRE_PS_LIGHT_DIRECTION
    half3 lightDirection;
#endif

#if defined(REQUIRE_PS_NDOTL) || defined(REQUIRE_PS_NDOTLP)
    half nDotL;
#endif

#ifdef REQUIRE_PS_VIEW_DIR
    half3 viewDir;
#endif

#ifdef REQUIRE_PS_HALF_DIR
    half3 halfDir;
#endif

#ifdef REQUIRE_PS_NDOTV
    half nDotV;
#endif

#ifdef REQUIRE_PS_NDOTH
    half nDotH;
#endif

#ifdef REQUIRE_PS_HDOTV
    half hDotV;
#endif

#ifdef REQUIRE_PS_LDOTH
    half lDotH;
#endif

#ifdef REQUIRE_PS_VIEW_REFLECT
    half3 viewReflect;
#endif
};

#if defined(REQUIRE_PS_UV0) && defined(REQUIRE_PS_UV1)
#   define V2F_UV(semantic) half4 uv : semantic;
#   ifdef _DETAIL_UV1
#       define TRANSFER_UV(o, v) \
            o.uv.xy = TRANSFORM_TEX(v.uv0, _AlbedoTex); \
		    o.uv.zw = TRANSFORM_TEX(v.uv1, _DetailTex);
#   else
#       define TRANSFER_UV(o, v) \
            o.uv.xy = TRANSFORM_TEX(v.uv0, _AlbedoTex); \
		    o.uv.zw = TRANSFORM_TEX(v.uv0, _DetailTex);
#   endif
#   define PS_UV(a, i) \
        a.uv0 = i.uv.xy; \
        a.uv1 = i.uv.zw;
#elif defined(REQUIRE_PS_UV0)
#   define V2F_UV(semantic) half2 uv : semantic;
#   define TRANSFER_UV(o, v) o.uv = TRANSFORM_TEX(v.uv0, _AlbedoTex);
#   define PS_UV(a, i) a.uv0 = i.uv.xy;
#elif defined(REQUIRE_PS_UV1)
#   define V2F_UV(semantic) half2 uv : semantic;
#   ifdef _DETAIL_UV1
#       define TRANSFER_UV(o, v) o.uv = TRANSFORM_TEX(v.uv1, _DetailTex);
#   else
#       define TRANSFER_UV(o, v) o.uv = TRANSFORM_TEX(v.uv0, _DetailTex);
#   endif
#   define PS_UV(a, i) a.uv1 = i.uv.xy;
#else
#   define V2F_UV(semantic)
#   define TRANSFER_UV(o, v)
#   define PS_UV(a, i)
#endif

#ifdef REQUIRE_PS_WORLD_POSITION
#   define V2F_WORLD_POSITION(semantic) float3 worldPosition : semantic;
#   define TRANSFER_WORLD_POSITION(o, a) o.worldPosition = a.worldPosition;
#   define PS_WORLD_POSITION(a, i) a.worldPosition = i.worldPosition;
#else
#   define V2F_WORLD_POSITION(semantic)
#   define TRANSFER_WORLD_POSITION(o, a)
#   define PS_WORLD_POSITION(a, i)
#endif

#ifdef REQUIRE_PS_WORLD_NORMAL
#   define V2F_WORLD_NORMAL(semantic) half3 worldNormal : semantic;
#   define TRANSFER_WORLD_NORMAL(o, a) o.worldNormal = a.worldNormal;
#   define PS_WORLD_NORMAL(a, i) a.worldNormal = i.worldNormal;
#else
#   define V2F_WORLD_NORMAL(semantic)
#   define TRANSFER_WORLD_NORMAL(o, a)
#   define PS_WORLD_NORMAL(a, i)
#endif

#ifdef REQUIRE_PS_WORLD_TANGENT
#   define V2F_WORLD_TANGENT(semantic) half3 worldTangent : semantic;
#   define TRANSFER_WORLD_TANGENT(o, a) o.worldTangent = a.worldTangent;
#   define PS_WORLD_TANGENT(a, i) a.worldTangent = i.worldTangent;
#else
#   define V2F_WORLD_TANGENT(semantic)
#   define TRANSFER_WORLD_TANGENT(o, a)
#   define PS_WORLD_TANGENT(a, i)
#endif

#ifdef REQUIRE_PS_WORLD_BINORMAL
#   define V2F_WORLD_BINORMAL(semantic) half3 worldBinormal : semantic;
#   define TRANSFER_WORLD_BINORMAL(o, a) o.worldBinormal = a.worldBinormal;
#   define PS_WORLD_BINORMAL(a, i) a.worldBinormal = i.worldBinormal;
#else
#   define V2F_WORLD_BINORMAL(semantic)
#   define TRANSFER_WORLD_BINORMAL(o, a)
#   define PS_WORLD_BINORMAL(a, i)
#endif

#ifdef REQUIRE_PS_AMBIENCE
#   ifdef REQUIRE_PS_NDOTL
#       define V2F_WORLD_AMBIENCE(semantic) half4 ambientOrLightmapUV : semantic;
#   else
#       define V2F_WORLD_AMBIENCE(semantic) half3 ambientOrLightmapUV : semantic;
#   endif
#   ifdef LIGHTMAP_ON
#       define TRANSFER_AMBIENCE(o, a, v) o.ambientOrLightmapUV.xy = v.uv1 * unity_LightmapST.xy + unity_LightmapST.zw;
#       define PS_AMBIENCE(a, i) a.ambience = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.ambientOrLightmapUV.xy));
#   else
#       define TRANSFER_AMBIENCE(o, a, v) o.ambientOrLightmapUV.rgb = a.ambience;
#       define PS_AMBIENCE(a, i) a.ambience = i.ambientOrLightmapUV.rgb;
#   endif
#else
#   define V2F_WORLD_AMBIENCE(semantic)
#   define TRANSFER_AMBIENCE(o, a, v)
#   define PS_AMBIENCE(a, i)
#endif

#ifdef REQUIRE_PS_LIGHT_DIRECTION
#   ifdef USING_DIRECTIONAL_LIGHT
#       define PS_LIGHT_DIRECTION(a, i) \
            a.lightDirection = normalize(_WorldSpaceLightPos0.xyz);
#   else
#       define PS_LIGHT_DIRECTION(a, i) \
            a.lightDirection = normalize(_WorldSpaceLightPos0.xyz - a.worldPosition);
#   endif
#else
#   define PS_LIGHT_DIRECTION(a, i)
#endif

#ifdef REQUIRE_PS_NDOTLP
#   define TRANSFER_NDOTL(o, a)
#   define PS_NDOTL(a, i) a.nDotL = max(dot(a.worldNormal, a.lightDirection), 0.0);
#elif defined(REQUIRE_PS_NDOTL)
#   ifdef REQUIRE_PS_AMBIENCE
#       define TRANSFER_NDOTL(o, a) o.ambientOrLightmapUV.a = a.nDotL;
#       define PS_NDOTL(a, i) a.nDotL = i.ambientOrLightmapUV.a;
#   else
#       define TRANSFER_NDOTL(o, a) o.nDotL = a.nDotL;
#       define PS_NDOTL(a, i) a.nDotL = i.nDotL;
#   endif
#else
#   define TRANSFER_NDOTL(o, a)
#   define PS_NDOTL(a, i)
#endif

#ifdef REQUIRE_PS_VIEW_DIR
#   define V2F_VIEW_DIR(semantic) half3 viewDir : semantic;
#   define TRANSFER_VIEW_DIR(o, a) o.viewDir = a.viewDir;
#   define PS_VIEW_DIR(a, i) a.viewDir = i.viewDir;
#else
#   define V2F_VIEW_DIR(semantic)
#   define TRANSFER_VIEW_DIR(o, a)
#   define PS_VIEW_DIR(a, i)
#endif

#ifdef REQUIRE_PS_VIEW_DIR
#   define PS_HALF_DIR(a, i) a.halfDir = normalize(a.lightDirection + a.viewDir);
#else
#   define PS_HALF_DIR(a, i) 
#endif

#ifdef REQUIRE_PS_NDOTV
#   define PS_NDOTV(a, i) a.nDotV = max(dot(a.worldNormal, a.viewDir), 0.0);
#else
#   define PS_NDOTV(a, i)
#endif

#ifdef REQUIRE_PS_NDOTH
#   define PS_NDOTH(a, i) a.nDotH = max(dot(a.worldNormal, a.halfDir), 0.0);
#else
#   define PS_NDOTH(a, i)
#endif

#ifdef REQUIRE_PS_HDOTV
#   define PS_HDOTV(a, i) a.hDotV = max(dot(a.halfDir, a.viewDir), 0.0);
#else
#   define PS_HDOTV(a, i)
#endif

#ifdef REQUIRE_PS_LDOTH
#   define PS_LDOTH(a, i) a.lDotH = max(dot(a.lightDirection, a.halfDir), 0.0);
#else
#   define PS_LDOTH(a, i)
#endif

#ifdef REQUIRE_PS_VIEW_REFLECT
#   define PS_VIEW_REFLECT(a, i) a.viewReflect = reflect(-i.viewDir, a.worldNormal);
#else
#   define PS_VIEW_REFLECT(a, i)
#endif

// This macro put to the v2f structure, used to transfer packed attributes from 
// vertex shader to pixel shader.
#define V2F_VERTEX_ATTRIBUTE(semantic1, semantic2, semantic3, semantic4, semantic5, semantic6, semantic7) \
    V2F_UV(semantic1) \
    V2F_VIEW_DIR(semantic2) \
    V2F_WORLD_AMBIENCE(semantic3) \
    V2F_WORLD_NORMAL(semantic4) \
    V2F_WORLD_POSITION(semantic5) \
    V2F_WORLD_TANGENT(semantic6) \
    V2F_WORLD_BINORMAL(semantic7)

// Transfer the vertex attributes to pixel shader.
#define TRANSFER_VERTEX_ATTRIBUTE(o, a, v) \
    TRANSFER_UV(o, v) \
    TRANSFER_WORLD_POSITION(o, a) \
    TRANSFER_WORLD_NORMAL(o, a) \
    TRANSFER_WORLD_TANGENT(o, a) \
    TRANSFER_WORLD_BINORMAL(o, a) \
    TRANSFER_AMBIENCE(o, a, v) \
    TRANSFER_NDOTL(o, a) \
    TRANSFER_VIEW_DIR(o, a)

// Calculate the pixel attributes.
#define CALCULATE_PIXEL_GEO_ATTRIBUTE(a, i) \
    PS_UV(a, i) \
    PS_WORLD_POSITION(a, i) \
    PS_WORLD_NORMAL(a, i) \
    PS_WORLD_TANGENT(a, i) \
    PS_WORLD_BINORMAL(a, i)

// Calculate the pixel attributes.
#define CALCULATE_PIXEL_LIGHT_ATTRIBUTE(a, i) \
    PS_AMBIENCE(a, i) \
    PS_LIGHT_DIRECTION(a, i) \
    PS_NDOTL(a, i) \
    PS_VIEW_DIR(a, i) \
    PS_HALF_DIR(a, i) \
    PS_NDOTV(a, i) \
    PS_NDOTH(a, i) \
    PS_HDOTV(a, i) \
    PS_LDOTH(a, i) \
    PS_VIEW_REFLECT(a, i)

#endif
