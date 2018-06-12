//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATURELIGHTING_INCLUDED
#define FEATURELIGHTING_INCLUDED

#include "LightContext.cginc"
#include "LightDiffuse.cginc"
#include "LightPBR.cginc"

inline half4 ApplyUnlit(inout LightContext ctx)
{
    half3 final = ctx.albedo * (1 + ctx.emission);
    return half4(final, ctx.alpha);
}

#if defined(_LIGHTING_PBR)
#define ApplyLighting(ctx, a) ApplyPBR(\
	a.ambience, a.nDotL, a.nDotV, a.nDotH, a.lDotH, a.viewReflect, ctx);
#elif defined(_LIGHTING_DIFFUSE)
#   ifdef LIGHTMAP_ON
#       define ApplyLighting(ctx, a) ApplyUnlit(ctx);
#   else
#       define ApplyLighting(ctx, a) ApplyDiffuse(a.ambience, a.nDotL, ctx);
#   endif
#else
#   define ApplyLighting(ctx, a) ApplyUnlit(ctx);
#endif

#endif
