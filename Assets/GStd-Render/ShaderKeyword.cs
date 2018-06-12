//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

using GStd;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// The keywords for GStd shader.
/// </summary>
public enum ShaderKeyword
{
    /// <summary>
    /// Enable the rim feature.
    /// </summary>
    ENABLE_RIM,
}

/// <summary>
/// The extensions for GStd shader.
/// </summary>
public static class ShaderKeywordExtensions
{
    /// <summary>
    /// Register all keyword when the game is launch.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        ShaderKeywords.SetKeywordName(
            (int)ShaderKeyword.ENABLE_RIM, "ENABLE_RIM");
    }
}
