namespace GStd
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Texture2DExtensions
    {
        public static bool HasAlpha(this Texture2D texture)
        {
            switch (texture.format)
            {
                case TextureFormat.ETC2_RGBA1:
                case TextureFormat.ETC2_RGBA8:
                case TextureFormat.ASTC_RGBA_4x4:
                case TextureFormat.ASTC_RGBA_5x5:
                case TextureFormat.ASTC_RGBA_6x6:
                case TextureFormat.ASTC_RGBA_8x8:
                case TextureFormat.ASTC_RGBA_10x10:
                case TextureFormat.ASTC_RGBA_12x12:
                case TextureFormat.ETC_RGBA8_3DS:
                case TextureFormat.BC6H:
                case TextureFormat.BC7:
                case TextureFormat.DXT5Crunched:
                case TextureFormat.PVRTC_RGBA2:
                case TextureFormat.PVRTC_RGBA4:
                case TextureFormat.DXT5:
                case TextureFormat.RGBA4444:
                case TextureFormat.BGRA32:
                case TextureFormat.RGBAHalf:
                case TextureFormat.RGBAFloat:
                case TextureFormat.Alpha8:
                case TextureFormat.ARGB4444:
                case TextureFormat.RGBA32:
                case TextureFormat.ARGB32:
                //case TextureFormat.ETC2_RGBA8:
                    return true;
            }
            return false;
        }
    }
}

