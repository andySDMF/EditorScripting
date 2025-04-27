using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BrandLab360.Editor
{
    public class SimulatorEditorProvider : HUBProvider
    {
        public SimulatorEditorProvider(string path) : base(path)
        {
            label = "Simulator";
            Domain = path;
            Order = 0;
            Tab = HUBTab.Editor;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {

        }

        public override void OnHeader(Rect position)
        {
            if (HUBWindow._APPSETTINGS == null)
            {
                EditorGUILayout.LabelField("Settings for the project has not been created!");
                return;
            }
        }

        public override void OnGUI(Rect position)
        {
            if (HUBWindow._APPSETTINGS == null) return;

            DrawSimulator();
        }


        // Register the HUBSettingsProvider
        [HUBSettingsProvider]
        public static HUBProvider CreateSettingsProvider()
        {
            var provider = new SimulatorEditorProvider("UnityEditor/Simulator");
            return provider;
        }

        private void DrawSimulator()
        {
            var m_variableSize = HUBWindow._LABELWIDTH;
            var cache = HUBWindow._APPSETTINGS;

            if (cache == null) return;

            EditorGUILayout.LabelField("Editor Play Mode", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ignore Intro Scene", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
            cache.editorTools.ignoreIntroScene = EditorGUILayout.Toggle(cache.editorTools.ignoreIntroScene, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Instantiation", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Create Webclient Simulator", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
            cache.editorTools.createWebClientSimulator = EditorGUILayout.Toggle(cache.editorTools.createWebClientSimulator, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Simulator", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
            cache.editorTools.simulator = EditorGUILayout.TextField("", cache.editorTools.simulator, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Simulation", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("URL Params", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
            cache.editorTools.simulateURLParams = EditorGUILayout.Toggle(cache.editorTools.simulateURLParams, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Admin", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
            cache.editorTools.simulateAdmin = EditorGUILayout.Toggle(cache.editorTools.simulateAdmin, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mobile", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
            cache.editorTools.simulateMobile = EditorGUILayout.Toggle(cache.editorTools.simulateMobile, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            if(cache.editorTools.simulateMobile == true)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Orientation", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
                cache.editorTools.simulateOrientation = (OrientationType)EditorGUILayout.EnumPopup(cache.editorTools.simulateOrientation, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Screen Size", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(m_variableSize));
                cache.editorTools.simulateScreenSize = EditorGUILayout.Vector2Field("", cache.editorTools.simulateScreenSize, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
