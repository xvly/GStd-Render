namespace GStd
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    public sealed class MaterialCache
    {
        private static MaterialCache _inst;
        public static MaterialCache Instance{
            get{
                if (_inst == null)
                    _inst = new MaterialCache();

                return _inst;}
        }

        private Dictionary<Struct6, Material> dictionary_0 = new Dictionary<Struct6, Material>(new Class72());

        public MaterialCache()
        {
#if UNITY_EDITOR            
            //EditorApplication.playModeStateChanged +=(EditorApplication.CallbackFunction)(this.method_1);
            EditorApplication.playmodeStateChanged = (EditorApplication.CallbackFunction) Delegate.Combine(EditorApplication.playmodeStateChanged, new EditorApplication.CallbackFunction(this.method_1));
#endif
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("GStd/Render/Clear Material Cache")]
        public static void ClearCache()
        {
            
            foreach (KeyValuePair<Struct6, Material> pair in MaterialCache.Instance.dictionary_0)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(pair.Value);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(pair.Value);
                }
            }
            MaterialCache.Instance.dictionary_0.Clear();
            SceneView.RepaintAll();
        }
#endif

        internal Material method_0(Material material_0, int int_0, ShaderKeywords shaderKeywords_0)
        {
            Struct6 struct2;
            Material material;
            struct2.int_0 = material_0.GetInstanceID();
            struct2.int_1 = int_0;
            struct2.shaderKeywords_0 = shaderKeywords_0;
            if (this.dictionary_0.TryGetValue(struct2, out material))
            {
                if (material != null)
                {
                    return material;
                }
                this.dictionary_0.Remove(struct2);
            }
            material = new Material(material_0) {
                hideFlags = HideFlags.DontSave
            };
            if (int_0 != -1)
            {
                material.renderQueue = int_0;
            }
            IEnumerator<int> enumerator = struct2.shaderKeywords_0.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    int current = enumerator.Current;
                    string keywordName = ShaderKeywords.GetKeywordName(current);
                    material.EnableKeyword(keywordName);
                    material.name = material.name + "[" + keywordName + "]";
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            this.dictionary_0.Add(struct2, material);
            return material;
        }

#if UNITY_EDITOR
        [CompilerGenerated]
        private void method_1()
        {
            if (!EditorApplication.isPaused)
            {
                this.dictionary_0.Clear();
            }
        }
#endif

        private class Class72 : IEqualityComparer<MaterialCache.Struct6>
        {
            public bool Equals(MaterialCache.Struct6 x, MaterialCache.Struct6 y)
            {
                if (x.int_0 != y.int_0)
                {
                    return false;
                }
                if (x.int_1 != y.int_1)
                {
                    return false;
                }
                if (!x.shaderKeywords_0.Equals(y.shaderKeywords_0))
                {
                    return false;
                }
                return true;
            }

            public int GetHashCode(MaterialCache.Struct6 obj)
            {
                int hashCode = obj.int_0.GetHashCode();
                hashCode = (0x18d * hashCode) ^ obj.int_1.GetHashCode();
                return ((0x18d * hashCode) ^ obj.shaderKeywords_0.GetHashCode());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct6
        {
            public int int_0;
            public int int_1;
            public ShaderKeywords shaderKeywords_0;
        }
    }
}

