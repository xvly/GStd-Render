//------------------------------------------------------------------------------
// Copyright (c) 2018-2018 GStd Technology Co. Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// The post effect used to control all post effects into one stack. It 
/// combine different post effects into one pass, to minimize the drawcall, 
/// and reduce the pixel shader payload.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public sealed class PostEffects : MonoBehaviour
{
    private static int threshholdID = -1;
    private static int offsetsID = -1;
    private static int bloomTexID = -1;
    private static int bloomIntensityID = -1;
    private static int saturationID = -1;
    private static int curveTexID = -1;
    private static int vignetteIntensityID = -1;

    [SerializeField]
    [Tooltip("The shader for down sample.")]
    private Shader downSampleShader;

    [SerializeField]
    [Tooltip("The shader for bright pass.")]
    private Shader brightPassShader;

    [SerializeField]
    [Tooltip("The shader for blur pass.")]
    private Shader blurPassShader;

    [SerializeField]
    [Tooltip("The shader for combine pass.")]
    private Shader combinePassShader;

    [SerializeField]
    [Tooltip("Whether to enable the bloom.")]
    private bool enableBloom;

    [SerializeField]
    [Tooltip("The bloom blend mode.")]
    private BloomBlendMode bloomBlendMode =
        BloomBlendMode.Add;

    [SerializeField]
    [Tooltip("The bloom intensity.")]
    private float bloomIntensity = 0.5f;

    [SerializeField]
    [Tooltip("The bloom threshold.")]
    [Range(-0.05f, 4.0f)]
    private float bloomThreshold = 0.5f;

    [SerializeField]
    [Tooltip("The bloom threshold color.")]
    private Color bloomThresholdColor = Color.white;

    [SerializeField]
    [Tooltip("The bloom blur spread.")]
    [Range(0.1f, 10.0f)]
    private float bloomBlurSpread = 2.5f;

    [SerializeField]
    [Tooltip("Whether to enable saturation control.")]
    private bool enableColorCurve;

    [SerializeField]
    [Tooltip("The color correction curve for red channel.")]
    private AnimationCurve redChannelCurve =
        new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

    [SerializeField]
    [Tooltip("The color correction curve for green channel.")]
    private AnimationCurve greenChannelCurve =
        new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

    [SerializeField]
    [Tooltip("The color correction curve for blue channel.")]
    private AnimationCurve blueChannelCurve =
        new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

    [SerializeField]
    [Tooltip("Whether to enable saturation control.")]
    private bool enableSaturation;

    [SerializeField]
    [Tooltip("The saturation for the image.")]
    [Range(0.0f, 5.0f)]
    private float saturation = 1.0f;

    [SerializeField]
    [Tooltip("Whether to enable vignette.")]
    private bool enableVignette;

    [SerializeField]
    [Tooltip("The intensity for vignette.")]
    private float vignetteIntensity = 0.375f;

    private new Camera camera;

    private bool rebuildResource = true;
    private Texture2D curveTex;
    private Material downSampleMaterial;
    private Material brightPassMaterial;
    private Material blurPassMaterial;
    private Material combinePassMaterial;

    [SerializeField]
    private bool enableSaturationSup;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float saturationSup = 0.1f;

    /// <summary>
    /// The bloom blend mode.
    /// </summary>
    private enum BloomBlendMode
    {
        /// <summary>
        /// Blend the bloom with the image using screen mode.
        /// </summary>
        Screen = 0,

        /// <summary>
        /// Blend the bloom with the image using add mode.
        /// </summary>
        Add = 1,
    }

    private static int ThreshholdID
    {
        get
        {
            if (threshholdID == -1)
            {
                threshholdID = Shader.PropertyToID("_Threshhold");
            }

            return threshholdID;
        }
    }

    private static int OffsetsID
    {
        get
        {
            if (offsetsID == -1)
            {
                offsetsID = Shader.PropertyToID("_Offsets");
            }

            return offsetsID;
        }
    }

    private static int BloomTexID
    {
        get
        {
            if (bloomTexID == -1)
            {
                bloomTexID = Shader.PropertyToID("_BloomTex");
            }

            return bloomTexID;
        }
    }

    private static int BloomIntensityID
    {
        get
        {
            if (bloomIntensityID == -1)
            {
                bloomIntensityID = Shader.PropertyToID("_BloomIntensity");
            }

            return bloomIntensityID;
        }
    }

    private static int SaturationID
    {
        get
        {
            if (saturationID == -1)
            {
                saturationID = Shader.PropertyToID("_Saturation");
            }

            return saturationID;
        }
    }

    private static int CurveTexID
    {
        get
        {
            if (curveTexID == -1)
            {
                curveTexID = Shader.PropertyToID("_CurveTex");
            }

            return curveTexID;
        }
    }

    private static int VignetteIntensityID
    {
        get
        {
            if (vignetteIntensityID == -1)
            {
                vignetteIntensityID = Shader.PropertyToID("_VignetteIntensity");
            }

            return vignetteIntensityID;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether enable bloom.
    /// </summary>
    public bool EnableBloom
    {
        get
        {
            return this.enableBloom;
        }

        set
        {
            if (this.enableBloom != value)
            {
                this.enableBloom = value;
                this.rebuildResource = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether enable color curve.
    /// </summary>
    public bool EnableColorCurve
    {
        get
        {
            return this.enableColorCurve;
        }

        set
        {
            if (this.enableColorCurve != value)
            {
                this.enableColorCurve = value;
                this.rebuildResource = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether enable saturation.
    /// </summary>
    public bool EnableSaturation
    {
        get
        {
            return this.enableSaturation;
        }

        set
        {
            if (this.enableSaturation != value)
            {
                this.enableSaturation = value;
                this.rebuildResource = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether enable Vignette.
    /// </summary>
    public bool EnableVignette
    {
        get
        {
            return this.enableVignette;
        }

        set
        {
            if (this.enableVignette != value)
            {
                this.enableVignette = value;
                this.rebuildResource = true;
            }
        }
    }

    public float SaturationSup
    {
        get
        {
            return saturationSup;
        }

        set
        {
            saturationSup = value;
        }
    }

    public bool EnableSaturationSup
    {
        get
        {
            return this.enableSaturationSup;
        }

        set
        {
            if (this.enableSaturationSup != value)
            {
                this.enableSaturationSup = value;
                this.rebuildResource = true;
            }
        }
    }

    private void Awake()
    {
        this.camera = this.GetComponent<Camera>();
    }

    private void OnEnable()
    {
        this.CheckSupport();
        if (this.enabled)
        {
            this.SetupResource();
            this.rebuildResource = false;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        this.rebuildResource = true;
    }
#endif

    private void CheckSupport()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            Debug.LogWarning(
                "The system does not support image effects.");
            this.enabled = false;
            return;
        }

        if (this.combinePassShader == null || 
            !this.combinePassShader.isSupported)
        {
            Debug.LogWarning(
                "The system does not support the combine pass shader.");
            this.enabled = false;
            return;
        }

        if (this.enableBloom)
        {
            if (this.downSampleShader == null || !this.downSampleShader.isSupported)
            {
                Debug.LogWarning(
                    "The system does not support the down sample shader, turn off the bloom.");
                this.enableBloom = false;
            }
        }

        if (this.enableBloom)
        {
            if (this.brightPassShader == null || !this.brightPassShader.isSupported)
            {
                Debug.LogWarning(
                    "The system does not support the bright pass shader, turn off the bloom.");
                this.enableBloom = false;
            }
        }

        if (this.enableBloom)
        {
            if (this.blurPassShader == null || !this.blurPassShader.isSupported)
            {
                Debug.LogWarning(
                    "The system does not support the blur pass shader, turn off the bloom.");
                this.enableBloom = false;
            }
        }
    }

    private void SetupResource()
    {
        if (this.enableBloom)
        {
            if (this.downSampleMaterial == null)
            {
                this.downSampleMaterial = new Material(this.downSampleShader);
            }

            if (this.brightPassMaterial == null)
            {
                this.brightPassMaterial = new Material(this.brightPassShader);
            }

            if (this.blurPassMaterial == null)
            {
                this.blurPassMaterial = new Material(this.blurPassShader);
            }
        }

        if (this.combinePassMaterial == null)
        {
            this.combinePassMaterial = new Material(this.combinePassShader);
        }

        if (this.enableBloom)
        {
            switch (this.bloomBlendMode)
            {
            case BloomBlendMode.Add:
                this.combinePassMaterial.EnableKeyword("_BLOOM_ADD");
                this.combinePassMaterial.DisableKeyword("_BLOOM_SCREEN");
                break;
            case BloomBlendMode.Screen:
                this.combinePassMaterial.DisableKeyword("_BLOOM_ADD");
                this.combinePassMaterial.EnableKeyword("_BLOOM_SCREEN");
                break;
            }
        }
        else
        {
            this.combinePassMaterial.DisableKeyword("_BLOOM_ADD");
            this.combinePassMaterial.DisableKeyword("_BLOOM_SCREEN");
        }

        if (this.enableColorCurve)
        {
            if (this.curveTex == null)
            {
                this.curveTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
                this.curveTex.hideFlags = HideFlags.DontSave;
                this.curveTex.wrapMode = TextureWrapMode.Clamp;
                this.curveTex.filterMode = FilterMode.Bilinear;
            }

            if (this.redChannelCurve != null && 
                this.greenChannelCurve != null && 
                this.blueChannelCurve != null)
            {
                for (int i = 0; i < 256; ++i)
                {
                    var k = (float)i / 256;

                    var rCh = Mathf.Clamp(this.redChannelCurve.Evaluate(k), 0.0f, 1.0f);
                    var gCh = Mathf.Clamp(this.greenChannelCurve.Evaluate(k), 0.0f, 1.0f);
                    var bCh = Mathf.Clamp(this.blueChannelCurve.Evaluate(k), 0.0f, 1.0f);

                    this.curveTex.SetPixel(i, 0, new Color(rCh, rCh, rCh));
                    this.curveTex.SetPixel(i, 1, new Color(gCh, gCh, gCh));
                    this.curveTex.SetPixel(i, 2, new Color(bCh, bCh, bCh));
                }

                this.curveTex.Apply();
            }

            this.combinePassMaterial.EnableKeyword("_COLOR_CURVE");
        }
        else
        {
            this.combinePassMaterial.DisableKeyword("_COLOR_CURVE");
        }

        if (this.enableSaturation)
        {
            this.combinePassMaterial.EnableKeyword("_SATURATION");
        }
        else
        {
            this.combinePassMaterial.DisableKeyword("_SATURATION");
        }

        if (this.enableSaturationSup)
        {
            this.combinePassMaterial.EnableKeyword("_SATURATIONSUP");
        }
        else
        {
            this.combinePassMaterial.DisableKeyword("_SATURATIONSUP");
        }

        if (this.enableVignette)
        {
            this.combinePassMaterial.EnableKeyword("_VIGNETTE_INTENSITY");
        }
        else
        {
            this.combinePassMaterial.DisableKeyword("_VIGNETTE_INTENSITY");
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!this.enableBloom &&
            !this.enableSaturation &&
            !this.enableSaturationSup &&
            !this.enableColorCurve &&
            !this.enableVignette)
        {
            Graphics.Blit(source, destination);
            return;
        }

        if (this.rebuildResource)
        {
            this.SetupResource();
            rebuildResource = false;
        }

        RenderTexture blur4 = null;
        if (this.enableBloom)
        {
            var doHdr = this.camera.allowHDR;
            var rtFormat = (doHdr) ? RenderTextureFormat.ARGBHalf : 
                RenderTextureFormat.Default;
            var rtW2 = source.width / 2;
            var rtH2 = source.height / 2;
            var rtW4 = source.width / 4;
            var rtH4 = source.height / 4;

            float widthOverHeight = (1.0f * source.width) / (1.0f * source.height);
            float oneOverBaseSize = 1.0f / 512.0f;

            // downsample
            var halfRezColorDown = RenderTexture.GetTemporary(rtW2, rtH2, 0, rtFormat);
            Graphics.Blit(source, halfRezColorDown);

            var quarterRezColor = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            Graphics.Blit(halfRezColorDown, quarterRezColor, this.downSampleMaterial, 0);
            RenderTexture.ReleaseTemporary(halfRezColorDown);

            // cut colors (thresholding)
            var secondQuarterRezColor = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            var threshColor = this.bloomThreshold * this.bloomThresholdColor;
            this.brightPassMaterial.SetVector(ThreshholdID, threshColor);
            Graphics.Blit(quarterRezColor, secondQuarterRezColor, this.brightPassMaterial, 0);
            RenderTexture.ReleaseTemporary(quarterRezColor);

            // vertical blur
            blur4 = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            var offset = new Vector4(0.0f, this.bloomBlurSpread * oneOverBaseSize, 0.0f, 0.0f);
            this.blurPassMaterial.SetVector(OffsetsID, offset);
            Graphics.Blit(secondQuarterRezColor, blur4, this.blurPassMaterial, 0);
            RenderTexture.ReleaseTemporary(secondQuarterRezColor);
            secondQuarterRezColor = blur4;

            // horizontal blur
            blur4 = RenderTexture.GetTemporary(rtW4, rtH4, 0, rtFormat);
            offset = new Vector4((this.bloomBlurSpread / widthOverHeight) * oneOverBaseSize, 0.0f, 0.0f, 0.0f);
            this.blurPassMaterial.SetVector(OffsetsID, offset);
            Graphics.Blit(secondQuarterRezColor, blur4, this.blurPassMaterial, 0);
            RenderTexture.ReleaseTemporary(secondQuarterRezColor);

            this.combinePassMaterial.SetTexture(BloomTexID, blur4);
            this.combinePassMaterial.SetFloat(BloomIntensityID, this.bloomIntensity);
        }

        // Do combine pass.
        if (this.enableSaturation || this.enableSaturationSup)
        {
            float s = 1f;
            if (this.enableSaturation)
            {
                s *= this.saturation;
            }
            if (this.enableSaturationSup)
            {
                s *= this.saturationSup;
            }
            this.combinePassMaterial.SetFloat(SaturationID, s);
        }

        if (this.enableColorCurve)
        {
            this.combinePassMaterial.SetTexture(CurveTexID, this.curveTex);
        }

        if (this.enableVignette)
        {
            this.combinePassMaterial.SetFloat(
                VignetteIntensityID, this.vignetteIntensity);
        }

        Graphics.Blit(source, destination, this.combinePassMaterial, 0);
        if (blur4 != null)
        {
            RenderTexture.ReleaseTemporary(blur4);
        }
    }
}
