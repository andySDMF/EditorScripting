using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BrandLab360.Editor
{
    public class HUBGUIStyle
    {
        public static GUIStyle H1 = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 30,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };

        public static GUIStyle H2 = new GUIStyle()
        {
            fontSize = 20,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };

        public static GUIStyle H3 = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };


        public static GUIStyle TabSelected = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 40,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
                
            }
        };

        public static GUIStyle TabNormal = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 40,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            active = new GUIStyleState()
            {
                background = Texture2D.whiteTexture
            }
        };

        public static GUIStyle DomainSelected = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 40,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white

            }
        };

        public static GUIStyle DomainNormal = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 40,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            active = new GUIStyleState()
            {
                background = Texture2D.whiteTexture
            }
        };


        public static GUIStyle ButtonStandard = new GUIStyle(GUI.skin.button)
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            active = new GUIStyleState()
            {
                background = Texture2D.whiteTexture
            }
        };

        public static GUIStyle ButtonHyperlink = new GUIStyle(GUI.skin.button)
        {
            normal = new GUIStyleState()
            {
                textColor = new Color32(0, 148, 255, 255)
            },
            active = new GUIStyleState()
            {
                textColor = new Color32(0, 148, 255, 255)
            },
            hover = new GUIStyleState()
            {
                textColor = new Color32(0, 148, 255, 255)
            }
        };


        public static GUIStyle ButtonMini = new GUIStyle(GUI.skin.button)
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            active = new GUIStyleState()
            {
                background = Texture2D.whiteTexture
            }
        };

        public static Texture2D MakeTexture(int width, int height, Color col)
        {
            Color[] pixels = new Color[width * height];

            for(int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = col;
            }

            Texture2D tex = new Texture2D(width, height);
            tex.SetPixels(pixels);
            tex.Apply();

            return tex;
        }

        private Object GetAsset<T>(string path)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(T));

            if (obj == null)
            {
                obj = GetPackageAsset<T>(path);
            }

            return obj;
        }

        private Object GetPackageAsset<T>(string path)
        {
            return AssetDatabase.LoadAssetAtPath(path.Replace("Assets", "Packages"), typeof(T));
        }
    }
}
