//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATURENORMAL_INCLUDED
#define FEATURENORMAL_INCLUDED

// Request attributes
#ifdef _NORMALMAP
#   ifndef REQUIRE_PS_UV0
#   define REQUIRE_PS_UV0
#   endif

#   ifndef REQUIRE_PS_WORLD_NORMAL
#   define REQUIRE_PS_WORLD_NORMAL
#   endif

#   ifndef REQUIRE_PS_WORLD_TANGENT
#   define REQUIRE_PS_WORLD_TANGENT
#   endif

#   ifndef REQUIRE_PS_WORLD_BINORMAL
#   define REQUIRE_PS_WORLD_BINORMAL
#   endif

#   ifndef REQUIRE_PS_NDOTLP
#   define REQUIRE_PS_NDOTLP
#   endif
#endif

// Contant buffer
CBUFFER_START(FeatureNormal)
    sampler2D _NormalTex;
    half _NormalScale;
CBUFFER_END

half3 applyNormal(half2 uv, half3 normal, half3 tangent, half3 binormal)
{
    // Gets the tangent normal.
    half3 normalTangent = UnpackScaleNormal(tex2D(_NormalTex, uv), _NormalScale);

    // Calculate the world normal.
    half3 worldNormal = tangent * normalTangent.x +
        binormal * normalTangent.y +
        normal * normalTangent.z;
    return normalize(worldNormal);
}

#ifdef _NORMALMAP
#   define ApplyNormal(a) \
    a.worldNormal = applyNormal(a.uv0, a.worldNormal, a.worldTangent, a.worldBinormal)
#else
#   define ApplyNormal(a)
#endif

#endif
