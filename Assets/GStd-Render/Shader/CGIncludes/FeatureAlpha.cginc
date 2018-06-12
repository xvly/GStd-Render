//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATUREALPHA_INCLUDED
#define FEATUREALPHA_INCLUDED

#include "LightContext.cginc"

// Contant buffer
CBUFFER_START(FeatureAlpha)
    fixed _Cutoff;
CBUFFER_END

inline void ApplyAlpha(inout LightContext ctx)
{
#if defined(_ALPHATEST_ON)
    clip(ctx.alpha - _Cutoff);
#endif

#ifdef _ALPHAPREMULTIPLY_ON
    ctx.albedo *= ctx.alpha;
#endif
}

#endif
