using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace BrandLab360.Editor
{
    public class ApplicationEditorProvider : HUBProvider
    {
        private bool editAssortmentAccumalator = false;

        public ApplicationEditorProvider(string path) : base(path)
        {
            label = "Application";
            Domain = path;
            Order = 1;
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

            DrawAssortment();
        }

        // Register the HUBSettingsProvider
        [HUBSettingsProvider]
        public static HUBProvider CreateSettingsProvider()
        {
            var provider = new ApplicationEditorProvider("UnityEditor/Application");
            return provider;
        }

        private void DrawAssortment()
        {
            var cache = HUBWindow._APPSETTINGS;

            if (cache == null) return;

            EditorGUILayout.LabelField("Assortment", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Index Accumalotor", EditorStyles.miniLabel);

            EditorGUILayout.BeginHorizontal();

            if (!editAssortmentAccumalator)
            {
                EditorGUILayout.LabelField("Current Value: " + cache.editorTools.assortmentIndexAccumulator.ToString(), EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
            }
            else
            {
                cache.editorTools.assortmentIndexAccumulator = EditorGUILayout.IntField(cache.editorTools.assortmentIndexAccumulator, GUILayout.ExpandWidth(true));
            }

            string accumalatorString = editAssortmentAccumalator ? "Done" : "Edit";

            if (GUILayout.Button(accumalatorString, GUILayout.Width(80)))
            {
                if (accumalatorString.Equals("Edit"))
                {
                    editAssortmentAccumalator = true;
                }
                else
                {
                    editAssortmentAccumalator = false;
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
