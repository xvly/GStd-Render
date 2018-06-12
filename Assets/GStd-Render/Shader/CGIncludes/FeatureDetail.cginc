//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATUREDETAIL_INCLUDED
#define FEATUREDETAIL_INCLUDED

// Request attributes
#if defined(_DETAIL_MULX2) || \
    defined(_DETAIL_MUL) || \
    defined(_DETAIL_ADD) || \
    defined(_DETAIL_LERP)
#   define _DETAILMAP
#   ifndef REQUIRE_PS_UV1
#   define REQUIRE_PS_UV1
#   endif
#endif

// Contant buffer
CBUFFER_START(FeatureDetail)
    sampler2D _DetailTex;
    half4 _DetailTex_ST;
    half4 _DetailColor;
    half4 _DetailUVSpeed;
CBUFFER_END

inline half4 applyDetailMap(half2 uv, half4 color)
{
#if _DETAIL_ANIMATION
    uv += _Time.y * _DetailUVSpeed.xy;
    uv = frac(uv);
#endif

    half3 detail = tex2D(_DetailTex, uv);

#if _DETAIL_COLOR
    detail *= _DetailColor;
#endif

#if _DETAIL_ALPHA
    detail *= color.a;
#endif

#if _DETAIL_MULX2
    color.rgb *= detail * unity_ColorSpaceDouble.rgb;
#elif _DETAIL_MUL
    color.rgb *= detail;
#elif _DETAIL_ADD
    color.rgb += detail;
#elif _DETAIL_LERP
    color.rgb = lerp(color, detail, color.a);
#endif

    return color;
}

#ifdef _DETAILMAP
#   define ApplyDetail(col, a) col = applyDetailMap(a.uv1, col)
#else
#   define ApplyDetail(col, a)
#endif

#endif
