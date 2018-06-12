//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATURERIM_INCLUDED
#define FEATURERIM_INCLUDED

#if defined(_RIM_COLOR) || defined(_RIM_LIGHT)
#   ifndef REQUIRE_PS_NDOTL
#   define REQUIRE_PS_NDOTL
#   endif

#   ifndef REQUIRE_PS_NDOTV
#   define REQUIRE_PS_NDOTV
#   endif
#endif

// Reflection contant buffer.
CBUFFER_START(ShaderRim)
    fixed4 _RimColor;
    fixed _RimFresnel;
    fixed _RimIntensity;

    fixed4 _RimLightColor;
    fixed _RimLightFresnel;
    fixed _RimLightIntensity;
CBUFFER_END

inline void applyRim(
    inout half4 finalColor,
    half nDotL,
    half nDotV)
{
#ifdef _RIM_COLOR
    fixed rimOpacity = pow(1 - nDotV, _RimFresnel);
    finalColor.rgb = lerp(finalColor.rgb, _RimColor.rgb * _RimIntensity, rimOpacity);
#endif

#ifdef _RIM_LIGHT
    fixed rimLightOpacity = pow(1 - nDotV, _RimLightFresnel);
    finalColor.rgb = lerp(finalColor.rgb, _RimLightColor.rgb * _RimLightIntensity, rimLightOpacity * nDotL);
#endif
}

#if defined(_RIM_COLOR) || defined(_RIM_LIGHT)
#define ApplyRim(finalColor, a) applyRim(finalColor, a.nDotL, a.nDotV)
#else
#define ApplyRim(finalColor, a)
#endif

#endif
