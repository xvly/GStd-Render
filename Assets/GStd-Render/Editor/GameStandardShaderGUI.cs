//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using System;
using GStd.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// The custom editor for shader: "Game/Standard".
/// </summary>
public class GameStandardShaderGUI : GStdShaderGUI
{
    private static readonly string[] BlendNames =
        Enum.GetNames(typeof(RenderingMode));

    private MaterialProperty renderingMode;
    private MaterialProperty cutoff;

    private MaterialProperty albedoTex;
    private MaterialProperty albedoColor;

    private MaterialProperty normalTex;
    private MaterialProperty normalScale;

    private MaterialProperty maskTex;

    private MaterialProperty detailTex;
    private MaterialProperty detailColor;
    private MaterialProperty detailUVSpeed;

    private MaterialProperty emissionColor;

    private MaterialProperty smoothness;
    private MaterialProperty metallic;
    private MaterialProperty specularColor;
    private MaterialProperty reflectionColor;

    private MaterialProperty rimColor;
    private MaterialProperty rimIntensity;
    private MaterialProperty rimFresnel;

    private MaterialProperty rimLightColor;
    private MaterialProperty rimLightIntensity;
    private MaterialProperty rimLightFresnel;

    private MaterialProperty occludeColor;
    private MaterialProperty occludePower;

    /// <summary>
    /// The rendering mode enumeration.
    /// </summary>
    private enum RenderingMode
    {
        /// <summary>
        /// Render the opaque solid object.
        /// </summary>
        Opaque,

        /// <summary>
        /// The transparent object without semi-transparent areas.
        /// </summary>
        Cutout,

        /// <summary>
        /// The soft edge material for transparent.
        /// </summary>
        SoftEdge,

        /// <summary>
        /// The transparent object like glass, the diffuse color will fade out
        /// but the specular will maintain.
        /// </summary>
        Transparent,

        /// <summary>
        /// Totally fade out an object, include diffuse and specular, make it
        /// completely fade out.
        /// </summary>
        Fade,
    }

    /// <inheritdoc/>
    protected override void FindProperties(MaterialProperty[] props)
    {
        this.renderingMode = ShaderGUI.FindProperty("_RenderingMode", props);
        this.cutoff = ShaderGUI.FindProperty("_Cutoff", props);

        this.albedoTex = ShaderGUI.FindProperty("_AlbedoTex", props);
        this.albedoColor = ShaderGUI.FindProperty("_AlbedoColor", props);

        this.normalTex = ShaderGUI.FindProperty("_NormalTex", props);
        this.normalScale = ShaderGUI.FindProperty("_NormalScale", props);

        this.maskTex = ShaderGUI.FindProperty("_MaskTex", props);

        this.detailTex = ShaderGUI.FindProperty("_DetailTex", props);
        this.detailColor = ShaderGUI.FindProperty("_DetailColor", props);
        this.detailUVSpeed = ShaderGUI.FindProperty("_DetailUVSpeed", props);

        this.emissionColor = ShaderGUI.FindProperty("_EmissionColor", props);

        this.smoothness = ShaderGUI.FindProperty("_Smoothness", props);
        this.metallic = ShaderGUI.FindProperty("_Metallic", props);

        this.specularColor = ShaderGUI.FindProperty("_SpecularColor", props);
        this.reflectionColor = ShaderGUI.FindProperty("_ReflectionColor", props);

        this.rimColor = ShaderGUI.FindProperty("_RimColor", props);
        this.rimIntensity = ShaderGUI.FindProperty("_RimIntensity", props);
        this.rimFresnel = ShaderGUI.FindProperty("_RimFresnel", props);

        this.rimLightColor = ShaderGUI.FindProperty("_RimLightColor", props);
        this.rimLightIntensity = ShaderGUI.FindProperty("_RimLightIntensity", props);
        this.rimLightFresnel = ShaderGUI.FindProperty("_RimLightFresnel", props);

        this.occludeColor = ShaderGUI.FindProperty("_OccludeColor", props);
        this.occludePower = ShaderGUI.FindProperty("_OccludePower", props);
    }

    /// <inheritdoc/>
    protected override void OnShaderGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        this.BlendModeGUI(materialEditor, materials);
        this.MainMapGUI(materialEditor, materials);
        this.SecondaryMapGUI(materialEditor, materials);
        this.ColorGUI(materialEditor, materials);
        this.LightingGUI(materialEditor, materials);
        this.RimGUI(materialEditor, materials);
        this.OcclusionGUI(materialEditor, materials);
    }

    /// <inheritdoc/>
    protected override void MaterialChanged(Material material)
    {
        if (this.renderingMode != null)
        {
            var renderingMode = (RenderingMode)this.renderingMode.floatValue;
            this.UpdateRenderingMode(renderingMode, material);
        }
    }

    private void UpdateRenderingMode(
        RenderingMode renderingMode, Material material)
    {
        switch (renderingMode)
        {
        case RenderingMode.Opaque:
            material.SetInt("_SrcBlend", (int)BlendMode.One);
            material.SetInt("_DstBlend", (int)BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.SetOverrideTag("RenderType", "Opaque");
            material.renderQueue = -1;
            break;
        case RenderingMode.Cutout:
            material.SetInt("_SrcBlend", (int)BlendMode.One);
            material.SetInt("_DstBlend", (int)BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.EnableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.SetOverrideTag("RenderType", "TransparentCutout");
            material.renderQueue = -1;
            break;
        case RenderingMode.SoftEdge:
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 1);
            material.EnableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.SetOverrideTag("RenderType", "TransparentCutout");
            material.renderQueue = 2500;
            break;
        case RenderingMode.Transparent:
            material.SetInt("_SrcBlend", (int)BlendMode.One);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = 3000;
            break;
        case RenderingMode.Fade:
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = 3000;
            break;
        }
    }

    private void BlendModeGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUI.showMixedValue = this.renderingMode.hasMixedValue;
        var renderingMode = (RenderingMode)this.renderingMode.floatValue;
        EditorGUI.BeginChangeCheck();
        renderingMode = (RenderingMode)EditorGUILayout.Popup(
            "Rendering Mode", (int)renderingMode, BlendNames);
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo("Rendering Mode");
            this.renderingMode.floatValue = (float)renderingMode;
            foreach (var mat in materials)
            {
                this.UpdateRenderingMode(renderingMode, mat);
            }
        }

        EditorGUI.showMixedValue = false;

        if (renderingMode != RenderingMode.Opaque && 
            renderingMode != RenderingMode.Transparent && 
            renderingMode != RenderingMode.Fade)
        {
            materialEditor.RangeProperty(this.cutoff, "Cutoff");
        }
    }

    private void MainMapGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUILayout.LabelField("Main Maps", EditorStyles.boldLabel);
        bool albedoEnable = this.TextureGUIWithKeyword(
            materialEditor,
            materials,
            this.albedoTex,
            "Albedo Texture",
            "_ALBEDOMAP");
        if (albedoEnable)
        {
            var contents = new GUIContent[]
            {
                new GUIContent("Normal"),
                new GUIContent("Emission"),
                new GUIContent("Smoothess"),
                new GUIContent("Metallic"),
                new GUIContent("Detail"),
            };
            var keys = new string[]
            {
                "_",
                "_EMISSION_ALPHA",
                "_SMOOTHNESS_ALPHA",
                "_METALLIC_ALPHA",
                "_DETAIL_ALPHA",
            };

            EditorGUI.indentLevel = 1;
            EditorGUILayout.PrefixLabel("Alpha Usage:");
            this.ListOptions(materials, contents, keys, true);
            EditorGUI.indentLevel = 0;
        }

        bool normalEnable = this.TextureGUIWithKeyword(
            materialEditor,
            materials,
            this.normalTex,
            "Normal Texture",
            "_NORMALMAP");
        if (normalEnable)
        {
            EditorGUI.indentLevel = 1;
            materialEditor.FloatProperty(this.normalScale, "Scale");
            EditorGUI.indentLevel = 0;
        }

        this.TextureGUIWithKeyword(
            materialEditor,
            materials,
            this.maskTex,
            new GUIContent("Mask Texture", "smoothness(R), metallic(G), emission(B)"),
            "_MASKMAP");

        if (this.albedoTex.textureValue != null ||
            this.normalTex.textureValue != null ||
            this.maskTex.textureValue != null)
        {
            materialEditor.TextureScaleOffsetProperty(this.albedoTex);
        }

        EditorGUILayout.Space();
    }

    private void SecondaryMapGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUILayout.LabelField("Secondary Maps", EditorStyles.boldLabel);
        var oldDetail = this.detailTex.textureValue;
        EditorGUI.BeginChangeCheck();
        materialEditor.TexturePropertySingleLine(
            new GUIContent("Detail Texture"), this.detailTex);
        if (EditorGUI.EndChangeCheck())
        {
            if (this.detailTex.textureValue == null)
            {
                foreach (var m in materials)
                {
                    m.DisableKeyword("_DETAIL_MULX2");
                    m.DisableKeyword("_DETAIL_MUL");
                    m.DisableKeyword("_DETAIL_ADD");
                    m.DisableKeyword("_DETAIL_LERP");
                }
            }
            else if (oldDetail == null)
            {
                foreach (var m in materials)
                {
                    m.EnableKeyword("_DETAIL_MULX2");
                }
            }
        }

        if (this.detailTex.textureValue != null)
        {
            var contents = new GUIContent[]
            {
                new GUIContent("Mul X2"),
                new GUIContent("Mul"),
                new GUIContent("Add"),
                new GUIContent("Lerp"),
            };

            var keywords = new string[]
            {
                "_DETAIL_MULX2",
                "_DETAIL_MUL",
                "_DETAIL_ADD",
                "_DETAIL_LERP",
            };

            this.ListOptions(materials, contents, keywords, true);

            EditorGUILayout.BeginHorizontal();
            if (this.CheckOption(materials, "Detail Color", "_DETAIL_COLOR"))
            {
                materialEditor.ColorProperty(this.detailColor, string.Empty);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }

            if (this.CheckOption(
                materials,
                "UV Animation",
                "_DETAIL_ANIMATION"))
            {
                materialEditor.VectorProperty(this.detailUVSpeed, "UV Speed");
            }

            materialEditor.TextureScaleOffsetProperty(this.detailTex);

            var uvContents = new GUIContent[]
            {
                new GUIContent("UV 0"),
                new GUIContent("UV 1"),
            };

            var uvKeywords = new string[]
            {
                "_",
                "_DETAIL_UV1",
            };

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("UV Set");
            this.ListOptions(materials, uvContents, uvKeywords);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
    }

    private void ColorGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (this.CheckOption(materials, "Albedo Color", "_ALBEDO_COLOR"))
        {
            materialEditor.ColorProperty(this.albedoColor, string.Empty);
        }

        EditorGUILayout.EndHorizontal();

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

    private void RimGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        EditorGUILayout.LabelField("Rim", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (this.CheckOption(materials, "Rim Color", "_RIM_COLOR"))
        {
            materialEditor.ColorProperty(this.rimColor, string.Empty);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = 1;
            materialEditor.RangeProperty(this.rimIntensity, "Intensity");
            materialEditor.RangeProperty(this.rimFresnel, "Fresnel");
            EditorGUI.indentLevel = 0;
        }
        else
        {
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        if (this.CheckOption(materials, "Rim Light", "_RIM_LIGHT"))
        {
            materialEditor.ColorProperty(this.rimLightColor, string.Empty);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = 1;
            materialEditor.RangeProperty(this.rimLightIntensity, "Intensity");
            materialEditor.RangeProperty(this.rimLightFresnel, "Fresnel");
            EditorGUI.indentLevel = 0;
        }
        else
        {
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
    }

    private bool OcclusionGUI(
        MaterialEditor materialEditor, Material[] materials)
    {
        var isOcclusion = this.IsOcclusion(materials[0]);
        EditorGUI.BeginChangeCheck();
        isOcclusion = EditorGUILayout.ToggleLeft("Is Occlusion", isOcclusion);
        if (EditorGUI.EndChangeCheck())
        {
            if (isOcclusion)
            {
                this.ChangeShader(materialEditor, materials, "Game/Occlusion");
            }
            else
            {
                this.ChangeShader(materialEditor, materials, "Game/Standard");
            }
        }

        if (isOcclusion)
        {
            materialEditor.ColorProperty(this.occludeColor, "Occlude Color");
            materialEditor.RangeProperty(this.occludePower, "Occlude Power");
        }

        return isOcclusion;
    }

    private bool IsOcclusion(Material material)
    {
        return material.shader.name == "Game/Occlusion";
    }
}
