// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATUREEMISSION_INCLUDED
#define FEATUREEMISSION_INCLUDED

#include "LightContext.cginc"

// Contant buffer
CBUFFER_START(FeatureEmission)
    half3 _EmissionColor;
CBUFFER_END

inline void ApplyEmission(inout LightContext ctx)
{
#if _EMISSION
    ctx.emission *= _EmissionColor;
#endif
}

#endif
