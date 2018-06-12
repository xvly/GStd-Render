//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#ifndef VERTEXINPUT_INCLUDED
#define VERTEXINPUT_INCLUDED

// Avaliable options:
// REQUIRE_VERTEXINPUT_NORMAL
// REQUIRE_VERTEXINPUT_TANGENT
// REQUIRE_VERTEXINPUT_UV0
// REQUIRE_VERTEXINPUT_UV1
// REQUIRE_VERTEXINPUT_COLOR0

//------------------------------------------------------------------------------
// Macro dependency.
//------------------------------------------------------------------------------
#if defined(LIGHTMAP_ON) || defined(_DETAIL_UV1)
#   define REQUIRE_VERTEXINPUT_UV1
#endif

//------------------------------------------------------------------------------
// The standard appdata.
//------------------------------------------------------------------------------
struct VertexInput
{
    float4 vertex : POSITION;

#if defined(REQUIRE_VERTEXINPUT_NORMAL)
    half3 normal : NORMAL;
#endif

#if defined(REQUIRE_VERTEXINPUT_TANGENT)
    half4 tangent : TANGENT;
#endif

#if defined(REQUIRE_VERTEXINPUT_UV0)
    half2 uv0 : TEXCOORD0;
#endif

#if defined(REQUIRE_VERTEXINPUT_UV1)
    half2 uv1 : TEXCOORD1;
#endif

#if defined(REQUIRE_VERTEXINPUT_COLOR0)
    fixed3 color0 : COLOR0;
#endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

#endif
