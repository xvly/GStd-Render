//-----------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//-----------------------------------------------------------------------------

using GStd.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// The custom editor for shader: "Game/Water".
/// </summary>
public sealed class GameWaterShaderGUI : GStdShaderGUI
{
    private MaterialProperty waveScale;
    private MaterialProperty waveSpeed;
    private MaterialProperty bumpMap;
    private MaterialProperty reflectiveColor;

    private MaterialProperty refractionDistort;
    private MaterialProperty refractionOpacity;

    private MaterialProperty waveTex;
    private MaterialProperty waveDistort;

    private MaterialProperty specularDir;
    private MaterialProperty specularPow;

    /// <inheritdoc/>
    protected override void FindProperties(MaterialProperty[] props)
    {
        this.waveScale = ShaderGUI.FindProperty(
            "_WaveScale", props);
        this.waveSpeed = ShaderGUI.FindProperty(
            "_WaveSpeed", props);
        this.bumpMap = ShaderGUI.FindProperty(
            "_BumpMap", props);
        this.reflectiveColor = ShaderGUI.FindProperty(
            "_ReflectiveColor", props);

        this.refractionDistort = ShaderGUI.FindProperty(
            "_RefractionDistort", props);
        this.refractionOpacity = ShaderGUI.FindProperty(
            "_RefractionOpacity", props);

        this.waveTex = ShaderGUI.FindProperty(
            "_WaveTex", props);
        this.waveDistort = ShaderGUI.FindProperty(
            "_WaveDistort", props);

        this.specularDir = ShaderGUI.FindProperty(
            "_SpecularDir", props);
        this.specularPow = ShaderGUI.FindProperty(
            "_SpecularPow", props);
    }

    /// <inheritdoc/>
    protected override void OnShaderGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        materialEditor.VectorProperty(
            this.waveScale, this.waveScale.displayName);
        materialEditor.VectorProperty(
            this.waveSpeed, this.waveSpeed.displayName);
        materialEditor.TexturePropertySingleLine(
            new GUIContent(this.bumpMap.displayName), this.bumpMap);
        materialEditor.TexturePropertySingleLine(
            new GUIContent(this.reflectiveColor.displayName),
            this.reflectiveColor);

        EditorGUI.BeginChangeCheck();
        bool refraction = this.CheckOption(
            materials, "Refraction", "_REFRACTION");
        if (EditorGUI.EndChangeCheck())
        {
            if (refraction)
            {
                foreach (var material in materials)
                {
                    material.SetInt("_SrcBlend", (int)BlendMode.One);
                    material.SetInt("_DstBlend", (int)BlendMode.Zero);
                }
            }
            else
            {
                foreach (var material in materials)
                {
                    material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                }
            }
        }

        if (refraction)
        {
            materialEditor.FloatProperty(
                this.refractionDistort, this.refractionDistort.displayName);
            materialEditor.FloatProperty(
                this.refractionOpacity, this.refractionOpacity.displayName);
        }

        if (this.CheckOption(materials, "Wave Texture", "_WAVE_TEXTURE"))
        {
            materialEditor.TexturePropertySingleLine(
                new GUIContent(this.waveTex.displayName), this.waveTex);
            materialEditor.FloatProperty(
                this.waveDistort, this.waveDistort.displayName);
        }

        if (this.CheckOption(materials, "Specular", "_SPECULAR"))
        {
            var dir = this.specularDir.vectorValue;
            var euler = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
            EditorGUI.BeginChangeCheck();
            euler = EditorGUILayout.Vector3Field("Dirctional", euler);
            if (EditorGUI.EndChangeCheck())
            {
                dir = Quaternion.Euler(euler) * Vector3.forward;
                this.specularDir.vectorValue = dir.normalized;
            }

            materialEditor.FloatProperty(
                this.specularPow, this.specularPow.displayName);
        }

        this.CheckOption(materials, "Vertex Color", "_VERTEX_COLOR");
    }

    /// <inheritdoc/>
    protected override void MaterialChanged(Material material)
    {
        if (material.IsKeywordEnabled("_REFRACTION"))
        {
            material.SetInt("_SrcBlend", (int)BlendMode.One);
            material.SetInt("_DstBlend", (int)BlendMode.Zero);
        }
        else
        {
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        }
    }
}
