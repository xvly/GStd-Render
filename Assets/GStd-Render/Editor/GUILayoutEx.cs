using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GStd.Editor
{
	// Token: 0x020000E6 RID: 230
	public static class GUILayoutEx
	{
		// Token: 0x060003CA RID: 970 RVA: 0x0001E4A4 File Offset: 0x0001C6A4
		public static bool Title(string title, bool foldout)
		{
			EditorGUILayout.BeginHorizontal(GUITheme.ListItemHeaderStyle, new GUILayoutOption[0]);
			Color backgroundColor = GUI.backgroundColor;
			if (!foldout)
			{
				GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
			}
			foldout = GUILayout.Toggle(foldout, title, EditorStyles.foldout, new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			GUI.backgroundColor = backgroundColor;
			return foldout;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000038B8 File Offset: 0x00001AB8
		public static void BeginContents(GUIStyle style)
		{
			GUILayout.BeginVertical(style, new GUILayoutOption[0]);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000038C6 File Offset: 0x00001AC6
		public static void BeginContents()
		{
			GUILayoutEx.BeginContents(GUI.skin.textArea);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000038D7 File Offset: 0x00001AD7
		public static void EndContents()
		{
			GUILayout.EndVertical();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001E504 File Offset: 0x0001C704
		public static bool ActionButton(string label)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = GUITheme.AcceptColor;
			bool result = false;
			if (GUILayout.Button(label, GUITheme.ActionButtonStyle, new GUILayoutOption[0]))
			{
				result = true;
			}
			GUI.backgroundColor = backgroundColor;
			return result;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000038DE File Offset: 0x00001ADE
		public static bool AddIconButton()
		{
			return GUILayout.Button("", GUITheme.IconButtonStyle, new GUILayoutOption[0]);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000038F5 File Offset: 0x00001AF5
		public static bool DeleteIconButton()
		{
			return GUILayout.Button("", GUITheme.IconButtonStyle, new GUILayoutOption[0]);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000390C File Offset: 0x00001B0C
		public static bool QuestionIconButton()
		{
			return GUILayout.Button("", GUITheme.IconButtonStyle, new GUILayoutOption[0]);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001E540 File Offset: 0x0001C740
		public static Rect DragBar()
		{
			Color color = GUI.color;
			GUI.color = Color.white;
			GUILayout.Label("", GUITheme.DragBarStyle, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(false),
				GUILayout.ExpandWidth(false)
			});
			Rect lastRect = GUILayoutUtility.GetLastRect();
			GUI.color = color;
			return lastRect;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00003923 File Offset: 0x00001B23
		public static void ProgressBar(float value, string text)
		{
			GUILayoutEx.ProgressBar(value, text, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001E594 File Offset: 0x0001C794
		public static void ProgressBar(float value, string text, params GUILayoutOption[] options)
		{
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, options);
			EditorGUI.ProgressBar(rect, value, text);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001E5C0 File Offset: 0x0001C7C0
		public static Vector2 ScrollViewFixHeight<T>(Vector2 scrollPosition, IList<T> list, float itemHeight, Action<T, int> draw)
		{
			if (Event.current.type == (EventType)8)
			{
				scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUI.skin.textArea, new GUILayoutOption[0]);
			}
			else
			{
				EditorGUILayout.BeginScrollView(scrollPosition, GUI.skin.textArea, new GUILayoutOption[0]);
			}
			int num = (int)(scrollPosition.y / itemHeight);
			num = Mathf.Clamp(num, 0, list.Count);
			int num2 = Mathf.Min(num + 50, list.Count);
			num2 = Mathf.Clamp(num2, 0, list.Count);
			GUILayout.Space((float)num * itemHeight);
			for (int i = num; i < num2; i++)
			{
				draw(list[i], i);
			}
			GUILayout.Space((float)(list.Count - num2) * itemHeight);
			EditorGUILayout.EndScrollView();
			return scrollPosition;
		}
	}
}
