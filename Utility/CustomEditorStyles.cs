using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public static class CustomEditorStyles
    {
        public static GUIStyle h1 = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20
        };

        public static GUIStyle h2 = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter
        };

        public static GUIStyle h3 = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleLeft
        };

        public static GUIStyle h4 = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleLeft
        };

        public static GUIStyle tableButton = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            fixedHeight = 20
        };

        public static GUIStyle backgroundBox = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(0, 0, 0, 0)
        };

        public static GUIStyle HUBDisplayBarButton = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            fixedHeight = 25,
        };

        public static GUIStyle panelBackround = new GUIStyle(GUI.skin.window);

        public static GUIStyle horizontalLine = new GUIStyle()
        {
            margin = new RectOffset(0, 0, 0, 0),
            fixedHeight = 1,
            normal = new GUIStyleState()
            {
                background = EditorGUIUtility.whiteTexture
            }
        };

        public static Color bl360Color = new Color(0.46f, 0.75f, 0.81f, 1);

        public static GUIStyle tabSelected = new GUIStyle("toolbarbutton") {
            normal = new GUIStyleState()
            {
                background = MakeTex2D(2, 2, bl360Color)
            }
        };

        public static GUIStyle tabUnselected = new GUIStyle("toolbarbutton");

        private static Texture2D MakeTex2D(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public static void DrawHorizontalLine(Color color)
        {
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, CustomEditorStyles.horizontalLine);
            GUI.color = c;
        }

        public static GUIStyle getSelectedTabStyle(bool selected)
        {
            if (selected) return tabSelected;

            return tabUnselected;
        }
    }

    public class EditorButtonData
    {
        public string Label { get; private set; }
        public Texture2D Icon { get; private set; }
        public System.Action Callback { get; private set; }

        public EditorButtonData(string label, Texture2D icon, System.Action callback)
        {
            Label = label;
            Icon = icon;
            Callback = callback;
        }
    }
}