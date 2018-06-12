using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace GStd.Editor
{
	// Token: 0x020000E7 RID: 231
	public static class GUITheme
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x0001E67C File Offset: 0x0001C87C
		static GUITheme()
		{
			if (EditorGUIUtility.isProSkin)
			{
				GUITheme.TitleBackground = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/GStd/Utility/Editor/bg_title_dark.png");
				GUITheme.ContainerBackground = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/GStd/Utility/Editor/bg_container_dark.png");
			}
			else
			{
				GUITheme.TitleBackground = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/GStd/Utility/Editor/bg_title_light.png");
				GUITheme.ContainerBackground = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/GStd/Utility/Editor/bg_container_light.png");
			}
			GUITheme.DefaultColor = Color.white;
			GUITheme.AcceptColor = Color.green;
			GUITheme.WarningColor = Color.yellow;
			GUITheme.ErrorColor = Color.red;
			GUITheme.TitleStyle = new GUIStyle
			{
				border = new RectOffset(2, 2, 2, 1),
				margin = new RectOffset(5, 5, 5, 0),
				padding = new RectOffset(5, 5, 3, 3),
				alignment = (TextAnchor)3
			};
			GUITheme.TitleStyle.normal.background = GUITheme.TitleBackground;
			GUITheme.TitleStyle.normal.textColor = ((!EditorGUIUtility.isProSkin) ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.8f, 0.8f, 0.8f));
			GUITheme.ListItemHeaderStyle = new GUIStyle
			{
				border = new RectOffset(2, 2, 2, 1),
				margin = new RectOffset(5, 5, 5, 0),
				padding = new RectOffset(5, 5, 0, 0),
				alignment = (TextAnchor)3
			};
			GUITheme.ListItemHeaderStyle.normal.background = GUITheme.TitleBackground;
			GUITheme.ListItemHeaderStyle.normal.textColor = ((!EditorGUIUtility.isProSkin) ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.8f, 0.8f, 0.8f));
			GUITheme.IconButtonStyle = new GUIStyle(GUI.skin.button)
			{
				stretchWidth = false,
				fixedHeight = 20f,
				margin = new RectOffset(5, 5, 0, 0),
				font = GUITheme.FontAwesome,
				fontSize = 10,
				alignment = (TextAnchor)4
			};
			GUITheme.IconButtonStyle.normal.background = null;
			GUITheme.IconToolbarStyle = new GUIStyle(GUI.skin.label)
			{
				stretchWidth = false,
				fixedHeight = 20f,
				border = new RectOffset(8, 8, 4, 4),
				padding = new RectOffset(4, 4, 2, 2),
				font = GUITheme.FontAwesome,
				fontSize = 12,
				alignment = (TextAnchor)4
			};
			GUITheme.IconToolbarStyle.active.background = GUI.skin.button.active.background;
			GUITheme.IconToolbarStyle.focused.background = GUI.skin.button.focused.background;
			GUITheme.IconToolbarStyle.hover.background = GUI.skin.button.hover.background;
			GUITheme.IconToolbarStyle.normal.background = GUI.skin.button.normal.background;
			GUITheme.IconToolbarStyle.onActive.background = GUI.skin.button.onActive.background;
			GUITheme.IconToolbarStyle.onFocused.background = GUI.skin.button.onFocused.background;
			GUITheme.IconToolbarStyle.onHover.background = GUI.skin.button.onHover.background;
			GUITheme.IconToolbarStyle.onNormal.background = GUI.skin.button.onNormal.background;
			GUITheme.ActionButtonStyle = new GUIStyle(GUI.skin.button)
			{
				fixedHeight = 20f,
				margin = new RectOffset(50, 50, 10, 0)
			};
			GUITheme.ColorLabelStyle = new GUIStyle(GUI.skin.label)
			{
				padding = new RectOffset(0, 0, 0, 0),
				margin = new RectOffset(0, 0, 0, 0),
				border = new RectOffset(0, 0, 0, 0),
				alignment = (TextAnchor)4
			};
			GUITheme.ColorLabelStyle.normal.background = Texture2D.whiteTexture;
			GUITheme.DragBarStyle = new GUIStyle(GUI.skin.label)
			{
				stretchWidth = false,
				fixedHeight = 20f,
				padding = new RectOffset(7, 7, 2, 2),
				margin = new RectOffset(0, 0, 0, 0),
				border = new RectOffset(0, 0, 0, 0),
				font = GUITheme.FontAwesome,
				fontSize = 10,
				alignment = (TextAnchor)4
			};
			GUITheme.LabelTitleStyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = 18,
				alignment = (TextAnchor)4,
				padding = new RectOffset(0, 0, 0, 0)
			};
			GUITheme.ContainerStyle = new GUIStyle(GUI.skin.textArea)
			{
				margin = new RectOffset(5, 5, 0, 0),
				border = new RectOffset(2, 2, 2, 2),
				padding = new RectOffset(2, 2, 2, 2)
			};
			GUITheme.ContainerStyle.normal.background = GUITheme.ContainerBackground;
			GUITheme.FooterStyle = new GUIStyle
			{
				margin = new RectOffset(0, 0, 0, 0),
				border = new RectOffset(0, 0, 0, 0),
				padding = new RectOffset(0, 0, 0, 0)
			};
			GUITheme.OverwriteTextFieldStyle = new GUIStyle(EditorStyles.textField);
			GUITheme.OverwriteTextFieldStyle.fontStyle =  (FontStyle)1;
			GUITheme.OverwritePopupStyle = new GUIStyle(EditorStyles.popup);
			GUITheme.OverwritePopupStyle.fontStyle = (FontStyle)1;
			GUITheme.OverwriteToggleStyle = new GUIStyle(EditorStyles.toggle);
			GUITheme.OverwriteToggleStyle.fontStyle = (FontStyle)1;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000393B File Offset: 0x00001B3B
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x00003942 File Offset: 0x00001B42
		public static Font FontAwesome { get; private set; } //= AssetDatabase.LoadAssetAtPath<Font>("Assets/GStd/fontawesome-webfont.ttf");

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000394A File Offset: 0x00001B4A
		// (set) Token: 0x060003DA RID: 986 RVA: 0x00003951 File Offset: 0x00001B51
		public static Color DefaultColor { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00003959 File Offset: 0x00001B59
		// (set) Token: 0x060003DC RID: 988 RVA: 0x00003960 File Offset: 0x00001B60
		public static Color AcceptColor { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00003968 File Offset: 0x00001B68
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0000396F File Offset: 0x00001B6F
		public static Color WarningColor { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00003977 File Offset: 0x00001B77
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0000397E File Offset: 0x00001B7E
		public static Color ErrorColor { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00003986 File Offset: 0x00001B86
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0000398D File Offset: 0x00001B8D
		public static GUIStyle TitleStyle { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00003995 File Offset: 0x00001B95
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x0000399C File Offset: 0x00001B9C
		public static GUIStyle ListItemHeaderStyle { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000039A4 File Offset: 0x00001BA4
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x000039AB File Offset: 0x00001BAB
		public static GUIStyle IconButtonStyle { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x000039B3 File Offset: 0x00001BB3
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x000039BA File Offset: 0x00001BBA
		public static GUIStyle IconToolbarStyle { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x000039C2 File Offset: 0x00001BC2
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x000039C9 File Offset: 0x00001BC9
		public static GUIStyle ActionButtonStyle { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000039D1 File Offset: 0x00001BD1
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x000039D8 File Offset: 0x00001BD8
		public static GUIStyle ColorLabelStyle { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000039E0 File Offset: 0x00001BE0
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x000039E7 File Offset: 0x00001BE7
		public static GUIStyle DragBarStyle { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000039EF File Offset: 0x00001BEF
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x000039F6 File Offset: 0x00001BF6
		public static GUIStyle LabelTitleStyle { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000039FE File Offset: 0x00001BFE
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x00003A05 File Offset: 0x00001C05
		public static GUIStyle ContainerStyle { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00003A0D File Offset: 0x00001C0D
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00003A14 File Offset: 0x00001C14
		public static GUIStyle FooterStyle { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00003A1C File Offset: 0x00001C1C
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00003A23 File Offset: 0x00001C23
		public static GUIStyle OverwriteTextFieldStyle { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00003A2B File Offset: 0x00001C2B
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00003A32 File Offset: 0x00001C32
		public static GUIStyle OverwritePopupStyle { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00003A3A File Offset: 0x00001C3A
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x00003A41 File Offset: 0x00001C41
		public static GUIStyle OverwriteToggleStyle { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00003A49 File Offset: 0x00001C49
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x00003A50 File Offset: 0x00001C50
		public static Texture2D TitleBackground { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00003A58 File Offset: 0x00001C58
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x00003A5F File Offset: 0x00001C5F
		public static Texture2D ContainerBackground { get; private set; }

		// Token: 0x040004E7 RID: 1255
		[CompilerGenerated]
		private static Font font_0;

		// Token: 0x040004E8 RID: 1256
		[CompilerGenerated]
		private static Color color_0;

		// Token: 0x040004E9 RID: 1257
		[CompilerGenerated]
		private static Color color_1;

		// Token: 0x040004EA RID: 1258
		[CompilerGenerated]
		private static Color color_2;

		// Token: 0x040004EB RID: 1259
		[CompilerGenerated]
		private static Color color_3;

		// Token: 0x040004EC RID: 1260
		[CompilerGenerated]
		private static GUIStyle guistyle_0;

		// Token: 0x040004ED RID: 1261
		[CompilerGenerated]
		private static GUIStyle guistyle_1;

		// Token: 0x040004EE RID: 1262
		[CompilerGenerated]
		private static GUIStyle guistyle_2;

		// Token: 0x040004EF RID: 1263
		[CompilerGenerated]
		private static GUIStyle guistyle_3;

		// Token: 0x040004F0 RID: 1264
		[CompilerGenerated]
		private static GUIStyle guistyle_4;

		// Token: 0x040004F1 RID: 1265
		[CompilerGenerated]
		private static GUIStyle guistyle_5;

		// Token: 0x040004F2 RID: 1266
		[CompilerGenerated]
		private static GUIStyle guistyle_6;

		// Token: 0x040004F3 RID: 1267
		[CompilerGenerated]
		private static GUIStyle guistyle_7;

		// Token: 0x040004F4 RID: 1268
		[CompilerGenerated]
		private static GUIStyle guistyle_8;

		// Token: 0x040004F5 RID: 1269
		[CompilerGenerated]
		private static GUIStyle guistyle_9;

		// Token: 0x040004F6 RID: 1270
		[CompilerGenerated]
		private static GUIStyle guistyle_10;

		// Token: 0x040004F7 RID: 1271
		[CompilerGenerated]
		private static GUIStyle guistyle_11;

		// Token: 0x040004F8 RID: 1272
		[CompilerGenerated]
		private static GUIStyle guistyle_12;

		// Token: 0x040004F9 RID: 1273
		[CompilerGenerated]
		private static Texture2D texture2D_0;

		// Token: 0x040004FA RID: 1274
		[CompilerGenerated]
		private static Texture2D texture2D_1;
	}
}
