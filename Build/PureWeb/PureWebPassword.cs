using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace BrandLab360
{
#if UNITY_EDITOR
    public class PureWebPassword : EditorWindow
    {
        private const string password = "pw-brandlab";

        private string m_password = "";
        private string ID;
        private string Version;
        private System.Action<string, string, bool> Callback;
        private Texture purewebIcon;

        public void Set(string id, string version, System.Action<string, string, bool> callback)
        {
            ID = id;
            Version = version;
            Callback = callback;

            purewebIcon = Resources.Load<Sprite>("pureweb").texture;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(purewebIcon);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Password", EditorStyles.boldLabel);
            m_password = EditorGUILayout.TextField("", m_password);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Delete"))
            {
                //need to check passsord
                if(m_password.Equals(password))
                {
                    if (Callback != null)
                    {
                        Callback.Invoke(ID, Version, false);
                    }
                }
            }

            if (GUILayout.Button("Cancel"))
            {
                if (Callback != null)
                {
                    Callback.Invoke(ID, Version, true);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
