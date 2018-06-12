//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATUREMASK_INCLUDED
#define FEATUREMASK_INCLUDED

#include "LightContext.cginc"

// Request attributes
#ifdef _MASKMAP
#   ifndef REQUIRE_PS_UV0
#   define REQUIRE_PS_UV0
#   endif
#endif

// Contant buffer
CBUFFER_START(FeatureMask)
    sampler2D _MaskTex;
CBUFFER_END

inline void applyMaskMap(half2 uv, inout LightContext ctx)
{
    half4 mask = tex2D(_MaskTex, uv);
    ctx.smoothness *= mask.r;
    ctx.metallic *= mask.g;
#if _EMISSION
    ctx.emission *= mask.b;
#endif
}

#ifdef _MASKMAP
#   define ApplyMask(ctx, a) applyMaskMap(a.uv0, ctx)
#else
#   define ApplyMask(ctx, a)
#endif

#endif
