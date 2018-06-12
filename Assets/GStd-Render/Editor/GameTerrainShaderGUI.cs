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
/// The custom editor for shader: "Game/Terrain".
/// </summary>
public class GameTerrainShaderGUI : GStdShaderGUI
{
    private MaterialProperty control;
    private MaterialProperty splat1;
    private MaterialProperty splat1Normal;
    private MaterialProperty splat2;
    private MaterialProperty splat2Normal;
    private MaterialProperty splat3;
    private MaterialProperty splat3Normal;
    private MaterialProperty splat4;
    private MaterialProperty splat4Normal;

    private MaterialProperty emissionColor;

    private MaterialProperty smoothness;
    private MaterialProperty specularColor;

    private MaterialProperty metallic;
    private MaterialProperty reflectionColor;

    /// <inheritdoc/>
    protected override void FindProperties(MaterialProperty[] props)
    {
        this.control = ShaderGUI.FindProperty("_Control", props);
        this.splat1 = ShaderGUI.FindProperty("_Splat1", props);
        this.splat1Normal = ShaderGUI.FindProperty("_Splat1_Normal", props);
        this.splat2 = ShaderGUI.FindProperty("_Splat2", props);
        this.splat2Normal = ShaderGUI.FindProperty("_Splat2_Normal", props);
        this.splat3 = ShaderGUI.FindProperty("_Splat3", props);
        this.splat3Normal = ShaderGUI.FindProperty("_Splat3_Normal", props);
        this.splat4 = ShaderGUI.FindProperty("_Splat4", props);
        this.splat4Normal = ShaderGUI.FindProperty("_Splat4_Normal", props);

        this.emissionColor = ShaderGUI.FindProperty("_EmissionColor", props);

        this.smoothness = ShaderGUI.FindProperty("_Smoothness", props);
        this.specularColor = ShaderGUI.FindProperty("_SpecularColor", props);

        this.metallic = ShaderGUI.FindProperty("_Metallic", props);
        this.reflectionColor = ShaderGUI.FindProperty("_ReflectionColor", props);
    }

    /// <inheritdoc/>
    protected override void OnShaderGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        this.LayerGUI(materialEditor, materials);
        this.ColorGUI(materialEditor, materials);
        this.LightingGUI(materialEditor, materials);
    }

    private void LayerGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        materialEditor.TexturePropertySingleLine(
            new GUIContent("Control"), this.control);

        var layerOptions = new GUIContent[]
        {
            new GUIContent("Layer2"),
            new GUIContent("Layer3"),
            new GUIContent("Layer4"),
        };
        var layerKeys = new string[]
        {
            "_",
            "_LAYER3",
            "_LAYER4",
        };

        EditorGUI.BeginChangeCheck();
        int index = this.ListOptions(materials, layerOptions, layerKeys, true);
        if (EditorGUI.EndChangeCheck())
        {
            if (index == 0)
            {
                foreach (var m in materials)
                {
                    m.DisableKeyword("_NORMAL3");
                    m.DisableKeyword("_NORMAL4");
                }
            }
            else if (index == 1)
            {
                foreach (var m in materials)
                {
                    m.DisableKeyword("_NORMAL4");
                }
            }
        }

        EditorGUILayout.LabelField("Layer 1:", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(
            new GUIContent("Albedo"), this.splat1);
        this.TextureGUIWithKeyword(
            materialEditor,
            materials,
            this.splat1Normal,
            "Normal",
            "_NORMAL1");

        materialEditor.TextureScaleOffsetProperty(this.splat1);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Layer 2:", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(
            new GUIContent("Albedo"), this.splat2);
        this.TextureGUIWithKeyword(
            materialEditor,
            materials,
            this.splat2Normal,
            "Normal",
            "_NORMAL2");

        materialEditor.TextureScaleOffsetProperty(this.splat2);
        EditorGUILayout.Space();

        if (index > 0)
        {
            EditorGUILayout.LabelField("Layer 3:", EditorStyles.boldLabel);
            materialEditor.TexturePropertySingleLine(
                new GUIContent("Albedo"), this.splat3);
            this.TextureGUIWithKeyword(
                materialEditor,
                materials,
                this.splat3Normal,
                "Normal",
                "_NORMAL3");

            materialEditor.TextureScaleOffsetProperty(this.splat3);
            EditorGUILayout.Space();
        }

        if (index > 1)
        {
            EditorGUILayout.LabelField("Layer 4:", EditorStyles.boldLabel);
            materialEditor.TexturePropertySingleLine(
                new GUIContent("Albedo"), this.splat4);
            this.TextureGUIWithKeyword(
                materialEditor,
                materials,
                this.splat4Normal,
                "Normal",
                "_NORMAL4");

            materialEditor.TextureScaleOffsetProperty(this.splat4);
            EditorGUILayout.Space();
        }

        var contents = new GUIContent[]
        {
            new GUIContent("Normal"),
            new GUIContent("Emission"),
            new GUIContent("Smoothess"),
            new GUIContent("Metallic"),
        };
        var keys = new string[]
        {
                "_",
                "_EMISSION_ALPHA",
                "_SMOOTHNESS_ALPHA",
                "_METALLIC_ALPHA",
        };

        EditorGUI.indentLevel = 1;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Alpha Usage:");
        this.ListOptions(materials, contents, keys, true);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel = 0;
    }

    private void ColorGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (this.CheckOption(materials, "Emission", "_EMISSION"))
        {
            materialEditor.ColorProperty(this.emissionColor, string.Empty);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void LightingGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUILayout.LabelField("Lighting", EditorStyles.boldLabel);

        var contents = new GUIContent[]
        {
            new GUIContent("Unlit"),
            new GUIContent("Diffuse"),
            new GUIContent("PBR"),
        };
        var keys = new string[]
        {
            "_",
            "_LIGHTING_DIFFUSE",
            "_LIGHTING_PBR",
        };

        int index = this.ListOptions(materials, contents, keys, true);
        if (index == 2)
        {
            materialEditor.RangeProperty(this.smoothness, "Smoothness");
            materialEditor.RangeProperty(this.metallic, "Metallic");

            materialEditor.ColorProperty(
                this.specularColor, "Specular Color");
            materialEditor.ColorProperty(
                this.reflectionColor, "Reflection Color");
        }

        EditorGUILayout.Space();
    }
}
