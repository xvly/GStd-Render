//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef FEATUREVERTICALFOG_INCLUDED
#define FEATUREVERTICALFOG_INCLUDED

// Color contant buffer
CBUFFER_START(ShaderVerticalFog)
    fixed4 _VerticalFogColor;
	half4 _VerticalFogParam;
CBUFFER_END

#ifdef _VERTICAL_FOG
#   ifndef REQUIRE_PS_WORLD_POSITION
#   define REQUIRE_PS_WORLD_POSITION
#   endif
#endif

inline half3 applyVerticalFog(
    half3 col,
    float3 worldPosition)
{
    half density = _VerticalFogParam.x;
    half start = _VerticalFogParam.y;
    half end = _VerticalFogParam.z;
    half fog = (worldPosition.y - start) / (end - start) * density;
    fog = saturate(fog);
    return lerp(col, _VerticalFogColor, fog);
}

#ifdef _VERTICAL_FOG
#   define ApplyVerticalFog(a, col) \
        col.rgb = applyVerticalFog(col.rgb, a.worldPosition);
#else
#   define ApplyVerticalFog(a, col)
#endif

#endif
