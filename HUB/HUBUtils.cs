using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BrandLab360.Editor
{
    public static class HUBUtils
    {
        public static void GetIRayCasters(string prefab, List<PlayerControlSettings.ManagerInteraction> origin)
        {
            GameObject core = null;
            core = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/" + prefab + ".prefab");

            if (core != null)
            {
                foreach (Transform t in core.GetComponentsInChildren<Transform>(true))
                {
                    IRaycaster iray = t.GetComponent<IRaycaster>();

                    if (iray != null)
                    {
                        PlayerControlSettings.ManagerInteraction mInt = origin.FirstOrDefault(x => x.managerName.Equals(t.name));

                        if (mInt == null)
                        {
                            mInt = new PlayerControlSettings.ManagerInteraction();
                            mInt.managerName = t.name;
                            mInt.interactionDistance = 20;
                            mInt.overrideInteraction = iray.OverrideDistance;
                            mInt.userCheckKey = iray.UserCheckKey;
                            origin.Add(mInt);
                        }
                    }
                    else
                    {
                        PlayerControlSettings.ManagerInteraction mInt = origin.FirstOrDefault(x => x.managerName.Equals(t.name));

                        if (mInt != null)
                        {
                            origin.Remove(mInt);
                        }
                    }
                }
            }
        }

        public static Object GetAsset<T>(string path)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(T));

            if (obj == null)
            {
                obj = GetPackageAsset<T>(path);
            }

            return obj;
        }

        public static Object GetPackageAsset<T>(string path)
        {
            return AssetDatabase.LoadAssetAtPath(path.Replace("Assets", "Packages"), typeof(T));
        }

        public static GameObject CreateParentAndSelectObject(string parentName, Transform t, bool zeroPosittion = true)
        {
            //find object in the scene
            GameObject parentGO = GameObject.Find(parentName);

            if (parentGO == null)
            {
                parentGO = new GameObject();
                parentGO.name = parentName;
                parentGO.transform.position = Vector3.zero;
                parentGO.transform.eulerAngles = Vector3.zero;
                parentGO.transform.localScale = Vector3.one;
            }

            if (t != null)
            {
                t.transform.SetParent(parentGO.transform);

                if(zeroPosittion)
                {
                    t.localPosition = Vector3.zero;
                }

                Selection.activeTransform = t;
                SceneView.lastActiveSceneView.FrameSelected();
            }

            return parentGO;
        }

        public static List<string> GetScenes()
        {
            List<string> scenes = new List<string>();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string filename = System.IO.Path.GetFileName(EditorBuildSettings.scenes[i].path);
                string extension = System.IO.Path.GetExtension(EditorBuildSettings.scenes[i].path);
                filename = filename.Replace(extension, "");

                if (filename.Equals(HUBWindow._APPSETTINGS.projectSettings.loginSceneName) || filename.Equals(HUBWindow._APPSETTINGS.projectSettings.avatarSceneName))
                {
                    continue;
                }

                scenes.Add(filename);
            }

            return scenes;
        }

        public static void AddSceneToBuildSettings(string scenePath)
        {
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            editorBuildSettingsScenes.AddRange(EditorBuildSettings.scenes.ToList());

            EditorBuildSettingsScene exists = editorBuildSettingsScenes.FirstOrDefault(x => x.path.Equals(scenePath + ".unity"));

            if (exists == null)
            {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath + ".unity", true));
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        public static List<string> GetSceneList(AppSettings settings)
        {
            List<string> scenes = new List<string>();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i] == null || string.IsNullOrEmpty(EditorBuildSettings.scenes[i].path)) continue;

                string filename = System.IO.Path.GetFileName(EditorBuildSettings.scenes[i].path);
                string extension = System.IO.Path.GetExtension(EditorBuildSettings.scenes[i].path);
                filename = filename.Replace(extension, "");

                if (filename.Equals(settings.projectSettings.loginSceneName) || filename.Equals(settings.projectSettings.avatarSceneName))
                {
                    continue;
                }

                scenes.Add(filename);
            }

            return scenes;
        }

        public static void DrawTexture(Rect position, Texture2D tex, float widthReduction = 195)
        {
            int leftMargin = 0;
            int rigthMargin = 0;

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(leftMargin);
                if (tex != null)
                {
                    float ratio = (tex.width > tex.height) ? (float)tex.width / tex.height : (float)tex.height / tex.width;

                    int width = ((int)(position.width - leftMargin - rigthMargin));
                    int height = (int)(width / ratio);

                    GUILayout.Label(tex, GUILayout.MaxWidth(width - widthReduction), GUILayout.MaxHeight(height));
                }
            }
            EditorGUILayout.EndHorizontal();
        }


        public static UnityEngine.Object[] GetAssets(string path)
        {
            UnityEngine.Object[] obj = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);

            if (obj == null)
            {
                obj = GetPackageAssets(path);
            }

            return obj;
        }

        public static UnityEngine.Object[] GetPackageAssets(string path)
        {
            return UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path.Replace("Assets", "Packages"));
        }
    }
}