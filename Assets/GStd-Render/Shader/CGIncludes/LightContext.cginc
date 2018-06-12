//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef LIGHTCONTEXT_INCLUDED
#define LIGHTCONTEXT_INCLUDED

// The context for calculate the light.
struct LightContext
{
    half3 albedo;
    half3 emission;
    half atten;
    half smoothness;
    half metallic;
    half alpha;
};

#define LIGHT_CONTEXT_INITIALIZE(c) \
    c.albedo = half3(1, 1, 1); \
    c.emission = half3(0, 0, 0); \
    c.smoothness = 0; \
    c.metallic = 0; \
    c.atten = 1; \
    c.alpha = 1;

#endif
