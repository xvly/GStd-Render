//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef LIGHTDIFFUSE_INCLUDED
#define LIGHTDIFFUSE_INCLUDED

#include "LightContext.cginc"

#if defined(_LIGHTING_DIFFUSE)
#   ifndef REQUIRE_PS_AMBIENCE
#   define REQUIRE_PS_AMBIENCE
#   endif

#   ifndef REQUIRE_PS_NDOTL
#   define REQUIRE_PS_NDOTL
#   endif
#endif

inline half4 ApplyDiffuse(
    half3 ambience,
    half nDotL,
    inout LightContext ctx)
{
    half3 diffuse = _LightColor0.rgb * nDotL;
    half3 final = ctx.albedo * (ambience + diffuse * ctx.atten);
    final *= (1 + ctx.emission);
    return half4(final, ctx.alpha);
}

#endif
