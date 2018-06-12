//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using GStd.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The custom editor for <see cref="PostEffects"/>.
/// </summary>
[CustomEditor(typeof(PostEffects))]
public sealed class PostEffectsEditor : Editor
{
    private SerializedProperty downSampleShader;
    private SerializedProperty brightPassShader;
    private SerializedProperty blurPassShader;
    private SerializedProperty combinePassShader;

    private SerializedProperty enableBloom;
    private SerializedProperty bloomBlendMode;
    private SerializedProperty bloomIntensity;
    private SerializedProperty bloomThreshold;
    private SerializedProperty bloomThresholdColor;
    private SerializedProperty bloomBlurSpread;

    private SerializedProperty enableColorCurve;
    private SerializedProperty redChannelCurve;
    private SerializedProperty greenChannelCurve;
    private SerializedProperty blueChannelCurve;

    private SerializedProperty enableSaturation;
    private SerializedProperty saturation;

    private SerializedProperty enableVignette;
    private SerializedProperty vignetteIntensity;

    private SerializedProperty enableSaturationSup;
    private SerializedProperty saturationSup;

    /// <inheritdoc/>
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        // Try to find shaders if missing.
        if (this.downSampleShader.objectReferenceValue == null)
        {
            this.downSampleShader.objectReferenceValue = 
                Shader.Find("Game/PostEffect/DownSample");
        }

        if (this.brightPassShader.objectReferenceValue == null)
        {
            this.brightPassShader.objectReferenceValue =
                Shader.Find("Game/PostEffect/BrightPass");
        }

        if (this.blurPassShader.objectReferenceValue == null)
        {
            this.blurPassShader.objectReferenceValue =
                Shader.Find("Game/PostEffect/BlurPass");
        }

        if (this.combinePassShader.objectReferenceValue == null)
        {
            this.combinePassShader.objectReferenceValue =
                Shader.Find("Game/PostEffect/CombinePass");
        }

        // Show the post effect shader.
        this.downSampleShader.isExpanded = EditorGUILayout.ToggleLeft(
            "Show Shaders", 
            this.downSampleShader.isExpanded);
        if (this.downSampleShader.isExpanded)
        {
            GUILayoutEx.BeginContents();
            EditorGUILayout.PropertyField(this.downSampleShader);
            EditorGUILayout.PropertyField(this.brightPassShader);
            EditorGUILayout.PropertyField(this.blurPassShader);
            EditorGUILayout.PropertyField(this.combinePassShader);
            GUILayoutEx.EndContents();
        }

        // Bloom
        this.enableBloom.boolValue = EditorGUILayout.ToggleLeft(
            this.enableBloom.displayName, 
            this.enableBloom.boolValue);
        if (this.enableBloom.boolValue)
        {
            GUILayoutEx.BeginContents();
            EditorGUILayout.PropertyField(this.bloomBlendMode);
            EditorGUILayout.PropertyField(this.bloomIntensity);
            EditorGUILayout.PropertyField(this.bloomThreshold);
            EditorGUILayout.PropertyField(this.bloomThresholdColor);
            EditorGUILayout.PropertyField(this.bloomBlurSpread);
            GUILayoutEx.EndContents();
        }

        // Color curve.
        this.enableColorCurve.boolValue = EditorGUILayout.ToggleLeft(
            this.enableColorCurve.displayName,
            this.enableColorCurve.boolValue);
        if (this.enableColorCurve.boolValue)
        {
            GUILayoutEx.BeginContents();
            EditorGUILayout.PropertyField(this.redChannelCurve);
            EditorGUILayout.PropertyField(this.greenChannelCurve);
            EditorGUILayout.PropertyField(this.blueChannelCurve);
            GUILayoutEx.EndContents();
        }

        // Saturation
        this.enableSaturation.boolValue = EditorGUILayout.ToggleLeft(
            this.enableSaturation.displayName,
            this.enableSaturation.boolValue);
        if (this.enableSaturation.boolValue)
        {
            GUILayoutEx.BeginContents();
            EditorGUILayout.PropertyField(this.saturation);
            GUILayoutEx.EndContents();
        }

        // SaturationSup
        this.enableSaturationSup.boolValue = EditorGUILayout.ToggleLeft(
            this.enableSaturationSup.displayName,
            this.enableSaturationSup.boolValue);
        GUILayoutEx.BeginContents();
        EditorGUILayout.PropertyField(this.saturationSup);
        GUILayoutEx.EndContents();

        // Vignette
        this.enableVignette.boolValue = EditorGUILayout.ToggleLeft(
            this.enableVignette.displayName,
            this.enableVignette.boolValue);
        if (this.enableVignette.boolValue)
        {
            GUILayoutEx.BeginContents();
            EditorGUILayout.PropertyField(this.vignetteIntensity);
            GUILayoutEx.EndContents();
        }

        this.serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        var serObj = this.serializedObject;
        this.downSampleShader = serObj.FindProperty("downSampleShader");
        this.brightPassShader = serObj.FindProperty("brightPassShader");
        this.blurPassShader = serObj.FindProperty("blurPassShader");
        this.combinePassShader = serObj.FindProperty("combinePassShader");

        this.enableBloom = serObj.FindProperty("enableBloom");
        this.bloomBlendMode = serObj.FindProperty("bloomBlendMode");
        this.bloomIntensity = serObj.FindProperty("bloomIntensity");
        this.bloomThreshold = serObj.FindProperty("bloomThreshold");
        this.bloomThresholdColor = serObj.FindProperty("bloomThresholdColor");
        this.bloomBlurSpread = serObj.FindProperty("bloomBlurSpread");

        this.enableColorCurve = serObj.FindProperty("enableColorCurve");
        this.redChannelCurve = serObj.FindProperty("redChannelCurve");
        this.greenChannelCurve = serObj.FindProperty("greenChannelCurve");
        this.blueChannelCurve = serObj.FindProperty("blueChannelCurve");

        this.enableSaturation = serObj.FindProperty("enableSaturation");
        this.saturation = serObj.FindProperty("saturation");
        this.enableSaturationSup = serObj.FindProperty("enableSaturationSup");
        this.saturationSup = serObj.FindProperty("saturationSup");

        this.enableVignette = serObj.FindProperty("enableVignette");
        this.vignetteIntensity = serObj.FindProperty("vignetteIntensity");
    }
}
