//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATUREALBEDO_INCLUDED
#define FEATUREALBEDO_INCLUDED

#include "LightContext.cginc"

// Request attributes
#ifdef _ALBEDOMAP
#   ifndef REQUIRE_PS_UV0
#   define REQUIRE_PS_UV0
#   endif
#endif

// Contant buffer
CBUFFER_START(FeatureAlbedo)
	sampler2D _AlbedoTex;
	half4 _AlbedoTex_ST;
	fixed4 _AlbedoColor;
CBUFFER_END

inline void applyAlbedoMap(half2 uv, inout LightContext ctx)
{
    half4 albedoColor = tex2D(_AlbedoTex, uv);
    ctx.albedo = albedoColor.rgb;
    ctx.alpha = albedoColor.a;
#ifdef _SMOOTHNESS_ALPHA
    ctx.smoothness = ctx.alpha;
#else
    ctx.smoothness = 1.0;
#endif

#ifdef _METALLIC_ALPHA
    ctx.metallic = ctx.alpha;
#else
    ctx.metallic = 1.0;
#endif

#if _EMISSION
#   ifdef _EMISSION_ALPHA
        ctx.emission = half3(ctx.alpha, ctx.alpha, ctx.alpha);
#   else
        ctx.emission = half3(1, 1, 1);
#   endif
#endif
}

inline void applyAlbedoColor(inout LightContext ctx)
{
#ifdef _ALBEDOMAP
    ctx.albedo *= _AlbedoColor.rgb;
    ctx.alpha *= _AlbedoColor.a;
#else
    ctx.albedo = _AlbedoColor.rgb;
    ctx.alpha = _AlbedoColor.a;
#endif
}

#ifdef _ALBEDOMAP
#   define ApplyAlbedoMap(ctx, a) applyAlbedoMap(a.uv0, ctx)
#else
#   define ApplyAlbedoMap(ctx, a)
#endif

#ifdef _ALBEDO_COLOR
#   define ApplyAlbedoColor(ctx) applyAlbedoColor(ctx)
#else
#   define ApplyAlbedoColor(ctx)
#endif

#define ApplyAlbedo(ctx, a) \
    ApplyAlbedoMap(ctx, a); \
    ApplyAlbedoColor(ctx)

#endif
