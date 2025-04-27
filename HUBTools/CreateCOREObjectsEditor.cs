using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace BrandLab360.Editor
{
    public class CreateCOREObjectsEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Scene/New Scene")]
        public static void CreateNewScene()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Scenes"))
            {
                AssetDatabase.CreateFolder("Assets", "Scenes");
            }

            string path = "";
            string sceneName = "SampleSceneTemplate";

            if (AssetDatabase.IsValidFolder($"Assets/com.brandlab360.core"))
            {
                path = $"Assets/com.brandlab360.core/Samples/HDRP Sample/Scenes/SampleSceneTemplate.unity";
            }
            else
            {
                string[] guids = AssetDatabase.FindAssets("t:SceneAsset");
                path = $"Assets/Samples/BrandLab360 Core/1.0.0/HDRP Sample/Scenes/SampleSceneTemplate.unity";

                foreach (string guid in guids)
                {
                    string absPath = AssetDatabase.GUIDToAssetPath(guid);

                    if(absPath.Contains("SampleSceneTemplate.unity"))
                    {
                        path = absPath;
                        break;
                    }
                }
            }

            Object obj = null;

#if BRANDLAB360_SAMPLE_HDRP
            if (!AssetDatabase.CopyAsset(path, $"Assets/Scenes/" + sceneName + ".unity"))
            {
                Debug.Log($"Could not create scene [" + sceneName + "]");
            }
            else
            {
                obj = (Object)GetAsset<Object>($"Assets/Scenes/" + sceneName + ".unity");
            }
#else
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCORE();
            CreateEnvironment();
            CreateTopDownCameraEditor.Create();
            CreateSpawnPointEditor.CreateSpawn();

            EditorSceneManager.SaveScene(newScene, "Assets/Scenes/" + sceneName);
#endif

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();

            if(obj != null)
            {
                EditorGUIUtility.PingObject(obj);
            }
        }

       // [MenuItem("BrandLab360/Core/CORE/BRANDLAB360 CORE")]
        public static void CreateCORE()
        {
            bool exists = FindObjectsByType<CoreManager>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length >= 1;

            if (exists)
            {
                DisplayWarningMessage("Scene already contains CORE Prefab");
            }
            else
            {
                UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/_BRANDLAB360_CORE.prefab");
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.name = "_BRANDLAB360_CORE";
                go.transform.position = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.eulerAngles = Vector3.zero;

                Selection.activeTransform = go.transform;
            }
        }

        //[MenuItem("BrandLab360/Core/CORE/Environment")]
        public static void CreateEnvironment()
        {
            bool exists = FindObjectsByType<Environment>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length >= 1;

            if (exists)
            {
                DisplayWarningMessage("Scene already contains ENVIRONMENT");
            }
            else
            {
                Environment environment = new GameObject().AddComponent<Environment>();
                environment.name = "_ENVIRONMENT";
                environment.transform.position = Vector3.zero;
                environment.transform.localScale = Vector3.one;
                environment.transform.eulerAngles = Vector3.zero;

                //add directionsal light
                Light dirLight = new GameObject("Directional Light").AddComponent<Light>();
                dirLight.type = LightType.Directional;
                dirLight.transform.SetParent(environment.transform);
                dirLight.transform.localPosition = Vector3.zero;
                dirLight.transform.localEulerAngles = Vector3.zero;
                dirLight.transform.localScale = Vector3.one;

                Selection.activeTransform = environment.transform;
            }
        }

        //[MenuItem("BrandLab360/Core/CORE/Scene Object Container")]
        public static void CreateSceneObjectContainer(string name)
        {
            GameObject parentGO = new GameObject();
            parentGO.name = name;
            parentGO.transform.position = Vector3.zero;
            parentGO.transform.eulerAngles = Vector3.zero;
            parentGO.transform.localScale = Vector3.one;

            if(Selection.activeTransform != null)
            {
                if(Selection.activeTransform.gameObject.scene != null)
                {
                    parentGO.transform.SetParent(Selection.activeTransform);
                    parentGO.transform.localPosition = Vector3.zero;
                    parentGO.transform.localEulerAngles = Vector3.zero;
                    parentGO.transform.localScale = Vector3.one;
                }
            }
        }
    }
}
