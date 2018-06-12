using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace GStd.Editor
{
	// Token: 0x020000C9 RID: 201
	public abstract class GStdShaderGUI : ShaderGUI
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0001C3E0 File Offset: 0x0001A5E0
		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			this.FindProperties(properties);
			Material[] array = new Material[materialEditor.targets.Length];
			for (int i = 0; i < materialEditor.targets.Length; i++)
			{
				array[i] = (Material)materialEditor.targets[i];
			}
			bool flag = false;
			EditorGUI.BeginChangeCheck();
			this.OnShaderGUI(materialEditor, array);
			if (EditorGUI.EndChangeCheck())
			{
				flag = true;
			}
			if ((Event.current.type == (EventType)13 || Event.current.type == (EventType)14) && Event.current.commandName == "UndoRedoPerformed")
			{
				flag = true;
			}
			if (flag)
			{
				MaterialCache.ClearCache();
			}
			if (this.bool_0)
			{
				foreach (Material material in array)
				{
					this.MaterialChanged(material);
				}
				this.bool_0 = false;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000035DE File Offset: 0x000017DE
		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			base.AssignNewShaderToMaterial(material, oldShader, newShader);
			if (oldShader == null)
			{
				return;
			}
			this.MaterialChanged(material);
		}

		// Token: 0x06000354 RID: 852
		protected abstract void FindProperties(MaterialProperty[] properties);

		// Token: 0x06000355 RID: 853
		protected abstract void OnShaderGUI(MaterialEditor materialEditor, Material[] materials);

		// Token: 0x06000356 RID: 854 RVA: 0x000035FA File Offset: 0x000017FA
		protected virtual void MaterialChanged(Material material)
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000035FC File Offset: 0x000017FC
		protected bool HasKeyword(Material[] materials, string key)
		{
			return Array.IndexOf<string>(materials[0].shaderKeywords, key) != -1;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0001C4AC File Offset: 0x0001A6AC
		protected bool HasAnyKeywords(Material[] materials, params string[] keys)
		{
			foreach (string value in keys)
			{
				int num = Array.IndexOf<string>(materials[0].shaderKeywords, value);
				if (num != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00003612 File Offset: 0x00001812
		protected bool CheckOption(Material[] materials, string content, string key)
		{
			return this.CheckOption(materials, new GUIContent(content), key);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00003622 File Offset: 0x00001822
		protected bool CheckOption(Material[] materials, string content, params string[] keys)
		{
			return this.CheckOption(materials, new GUIContent(content), keys);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001C4E8 File Offset: 0x0001A6E8
		protected bool CheckOption(Material[] materials, GUIContent content, string key)
		{
			bool flag = this.HasKeyword(materials, key);
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.ToggleLeft(content, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (flag)
				{
					foreach (Material material in materials)
					{
						material.EnableKeyword(key);
					}
				}
				else
				{
					foreach (Material material2 in materials)
					{
						material2.DisableKeyword(key);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001C560 File Offset: 0x0001A760
		protected bool CheckOption(Material[] materials, GUIContent content, params string[] keys)
		{
			bool flag = this.HasAnyKeywords(materials, keys);
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.ToggleLeft(content, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				string text = keys[0];
				if (flag)
				{
					foreach (Material material in materials)
					{
						material.EnableKeyword(text);
					}
				}
				else
				{
					foreach (string text2 in keys)
					{
						foreach (Material material2 in materials)
						{
							material2.DisableKeyword(text2);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00003632 File Offset: 0x00001832
		protected bool SwitchOption(Material[] materials, string content, string keyEnable, string keyDisable)
		{
			return this.SwitchOption(materials, new GUIContent(content), keyEnable, keyDisable);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0001C600 File Offset: 0x0001A800
		protected bool SwitchOption(Material[] materials, GUIContent content, string keyEnable, string keyDisable)
		{
			bool flag = this.HasKeyword(materials, keyEnable);
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.ToggleLeft(content, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (flag)
				{
					foreach (Material material in materials)
					{
						material.EnableKeyword(keyEnable);
						material.DisableKeyword(keyDisable);
					}
				}
				else
				{
					foreach (Material material2 in materials)
					{
						material2.EnableKeyword(keyDisable);
						material2.DisableKeyword(keyEnable);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001C688 File Offset: 0x0001A888
		protected int ListOptions(Material[] materials, GUIContent[] contents, string[] keys, bool toolbar = false)
		{
			int num = -1;
			foreach (string value in materials[0].shaderKeywords)
			{
				num = Array.IndexOf<string>(keys, value);
				if (num >= 0)
				{
					break;
				}
			}
			if (num < 0)
			{
				num = Array.IndexOf<string>(keys, "_");
			}
			if (num < 0)
			{
				num = Array.IndexOf<string>(keys, "__");
			}
			EditorGUI.BeginChangeCheck();
			if (toolbar)
			{
				num = GUILayout.Toolbar(num, contents, new GUILayoutOption[0]);
			}
			else
			{
				num = EditorGUILayout.Popup(num, contents, new GUILayoutOption[0]);
			}
			if (EditorGUI.EndChangeCheck())
			{
				string text = keys[num];
				foreach (Material material in materials)
				{
					foreach (string text2 in keys)
					{
						material.DisableKeyword(text2);
					}
				}
				if (text != "_" && text != "__")
				{
					foreach (Material material2 in materials)
					{
						material2.EnableKeyword(text);
					}
				}
			}
			return num;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00003644 File Offset: 0x00001844
		protected bool TextureGUIWithKeyword(MaterialEditor materialEditor, Material[] materials, MaterialProperty prop, string label, string keyword)
		{
			return this.TextureGUIWithKeyword(materialEditor, materials, prop, new GUIContent(label), keyword);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0001C79C File Offset: 0x0001A99C
		protected bool TextureGUIWithKeyword(MaterialEditor materialEditor, Material[] materials, MaterialProperty prop, GUIContent label, string keyword)
		{
			EditorGUI.BeginChangeCheck();
			materialEditor.TexturePropertySingleLine(label, prop);
			if (EditorGUI.EndChangeCheck())
			{
				foreach (Material material in materials)
				{
					if (prop.textureValue != null)
					{
						material.EnableKeyword(keyword);
					}
					else
					{
						material.DisableKeyword(keyword);
					}
				}
			}
			return prop.textureValue != null;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001C800 File Offset: 0x0001AA00
		protected void ChangeShader(MaterialEditor materialEditor, Material[] materials, string shaderName)
		{
			Shader shader = Shader.Find(shaderName);
			if (shader == null)
			{
				Debug.LogError("Can not find shader: " + shaderName);
				return;
			}
			materialEditor.RegisterPropertyChangeUndo("Change Shader: " + shaderName);
			foreach (Material material in materials)
			{
				material.shader = shader;
			}
		}

		// Token: 0x040004BD RID: 1213
		private bool bool_0 = true;

		// Token: 0x020000CA RID: 202
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct ShaderOption
		{
			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000363 RID: 867 RVA: 0x00003658 File Offset: 0x00001858
			// (set) Token: 0x06000364 RID: 868 RVA: 0x00003660 File Offset: 0x00001860
			public string Key { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000365 RID: 869 RVA: 0x00003669 File Offset: 0x00001869
			// (set) Token: 0x06000366 RID: 870 RVA: 0x00003671 File Offset: 0x00001871
			public GUIContent Content { get; set; }

			// Token: 0x040004BE RID: 1214
			[CompilerGenerated]
			private string string_0;

			// Token: 0x040004BF RID: 1215
			[CompilerGenerated]
			private GUIContent guicontent_0;
		}
	}
}
