//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef LIGHTPBR_INCLUDED
#define LIGHTPBR_INCLUDED

#include "LightContext.cginc"
#include "UnityStandardUtils.cginc"

// Require the necessary resource.
#ifdef _LIGHTING_PBR
#   ifndef REQUIRE_PS_AMBIENCE
#   define REQUIRE_PS_AMBIENCE
#   endif

#   ifndef REQUIRE_PS_NDOTLP
#   define REQUIRE_PS_NDOTLP
#   endif

#   ifndef REQUIRE_PS_NDOTV
#   define REQUIRE_PS_NDOTV
#   endif

#   ifndef REQUIRE_PS_NDOTH
#   define REQUIRE_PS_NDOTH
#   endif

#   ifndef REQUIRE_PS_LDOTH
#   define REQUIRE_PS_LDOTH
#   endif

#   ifndef REQUIRE_PS_VIEW_REFLECT
#   define REQUIRE_PS_VIEW_REFLECT
#   endif
#endif

// Contant buffer.
CBUFFER_START(LightPBR)
    half _Smoothness;
    half _Metallic;
    half3 _SpecularColor;
    half3 _ReflectionColor;
CBUFFER_END

inline half4 ApplyPBR(
    half3 ambience,
    half nDotL,
    half nDotV,
    half nDotH,
    half lDotH,
    half3 viewReflect,
    LightContext ctx)
{
    // Basic color and diffuse.
    half3 albedo = ctx.albedo;
    half emission = ctx.emission;
    half smoothness = ctx.smoothness * _Smoothness;
    half metallic = ctx.metallic * _Metallic;
    half alpha = ctx.alpha;

    half3 specColor;
    half oneMinusReflectivity;
    half3 diffColor = DiffuseAndSpecularFromMetallic(
        albedo, metallic, specColor, oneMinusReflectivity);
    diffColor = PreMultiplyAlpha(diffColor, alpha, oneMinusReflectivity, alpha);

    // Specular term
    half perceptualRoughness = SmoothnessToPerceptualRoughness(smoothness);
    half roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

    // GGX Distribution multiplied by combined approximation of Visibility and 
    // Fresnel See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile 
	// graphics course
    // https://community.arm.com/events/1155
    half a = roughness;
    half a2 = a*a;

    half d = nDotH * nDotH * (a2 - 1.h) + 1.00001h;
#ifdef UNITY_COLORSPACE_GAMMA
    // Tighter approximation for Gamma only rendering mode!
    // DVF = sqrt(DVF);
    // DVF = (a * sqrt(.25)) / (max(sqrt(0.1), lh)*sqrt(roughness + .5) * d);
    half specularTerm = a / (max(0.32h, lDotH) * (1.5h + roughness) * d);
#else
    half specularTerm = a2 / (max(0.1h, lDotH*lDotH) * (roughness + 0.5h) * (d * d) * 4);
#endif

#if defined (SHADER_API_MOBILE)
	// on mobiles (where half actually means something) denominator have risk of 
	// overflow clamp below was added specifically to "fix" that, but dx 
	// compiler (we convert bytecode to metal/gles) sees that specularTerm have 
	// only non-negative terms, so it skips max(0,..) in clamp (leaving only 
	// min(100,...))
    specularTerm = specularTerm - 1e-4h;
	// Prevent FP16 overflow on mobiles
    specularTerm = clamp(specularTerm, 0.0, 100.0);
#endif

    // surfaceReduction = Int D(NdotH) * NdotH * Id(NdotL>0) 
	// dH = 1/(realRoughness^2+1)
    // 1-0.28*x^3 as approximation for (1/(x^4+1))^(1/2.2) on the domain [0;1]
    // 1-x^3*(0.6-0.08*x)   approximation for 1/(x^4+1)
#ifdef UNITY_COLORSPACE_GAMMA
    half surfaceReduction = 0.28;
#else
    half surfaceReduction = (0.6 - 0.08 * perceptualRoughness);
#endif

    surfaceReduction = 1.0 - roughness*perceptualRoughness*surfaceReduction;

    // The smoothness, cubemap and mipmap
    half mipLevel = 10 * (1 - smoothness);
    mipLevel = clamp(mipLevel, 0, 10);
    half3 cubemap = UNITY_SAMPLE_TEXCUBE_LOD(
        unity_SpecCube0, viewReflect, mipLevel);
    cubemap *= _ReflectionColor;

    // Combine the colors.
    half grazingTerm = saturate(smoothness + (1 - oneMinusReflectivity));
    half3 finalAmb = diffColor * ambience;
#ifdef LIGHTMAP_ON
    half3 finalDiff = (specularTerm * specColor) * _LightColor0.rgb * nDotL;
#else
    half3 finalDiff = (diffColor + specularTerm * specColor) * _LightColor0.rgb * nDotL;
#endif
    half3 finalSepc = surfaceReduction * cubemap * FresnelLerpFast(
		specColor, grazingTerm, nDotV);
    half3 color = finalAmb + (finalDiff + finalSepc) * ctx.atten;
	color *= (1 + ctx.emission);

    return half4(color, alpha);
}

#endif
