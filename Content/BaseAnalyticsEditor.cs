using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    [CustomEditor(typeof(BaseAnalytics), true)]
    public class BaseAnalyticsEditor : UnityEditor.Editor
    {
        private BaseAnalytics script;

        private void OnEnable()
        {
            script = (BaseAnalytics)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("type"), true);

            SerializedProperty prop = serializedObject.FindProperty("type");

            if (prop != null)
            {
                if (prop.enumValueIndex > 0)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("message"), true);

                    serializedObject.FindProperty("label").stringValue = serializedObject.FindProperty("message").FindPropertyRelative("Label").stringValue;
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("label"), true);

                    serializedObject.FindProperty("message").FindPropertyRelative("Label").stringValue = serializedObject.FindProperty("label").stringValue;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
