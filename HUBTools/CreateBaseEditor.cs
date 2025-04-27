using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;

namespace BrandLab360.Editor
{
    public class CreateBaseEditor : EditorWindow
    {
        protected static void DownloadImage(string url, out Texture2D outTex)
        {
            using (WebClient client = new WebClient())
            {
                byte[] data = client.DownloadData(url);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(data);

                outTex = tex;
            }
        }

        protected static void SelectObject(Transform t)
        {
            //look at this a bit better
            if (Selection.activeTransform != null)
            {
                t.transform.SetParent(Selection.activeTransform);
                t.localPosition = Vector3.zero;
            }
            else
            {
                Camera view = UnityEditor.SceneView.lastActiveSceneView.camera;
                //Vector3 pos = view.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
                // t.transform.position = pos;

                SceneView es = UnityEditor.SceneView.lastActiveSceneView;
                es.AlignViewToObject(t);
            }

            Selection.activeTransform = t;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        protected static GameObject CreateParentAndSelectObject(string parentName, Transform t)
        {
            //find object in the scene
            GameObject parentGO = GameObject.Find(parentName);

            if(parentGO == null)
            {
                parentGO = new GameObject();
                parentGO.name = parentName;
                parentGO.transform.position = Vector3.zero;
                parentGO.transform.eulerAngles = Vector3.zero;
                parentGO.transform.localScale = Vector3.one;
            }

            if(t != null)
            {
                t.transform.SetParent(parentGO.transform);
                t.localPosition = Vector3.zero;

                Selection.activeTransform = t;
                SceneView.lastActiveSceneView.FrameSelected();
            }

            return parentGO;
        }

        protected static Object GetAsset<T>(string path)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(T));

            if (obj == null)
            {
                obj = GetPackageAsset<T>(path);
            }

            return obj;
        }

        protected static Object GetSamplePackageAsset<T>(string sampleID, string objectName, bool isResource = false, string objType = "prefab")
        {
            string[] guids = AssetDatabase.FindAssets("t:" + objType);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (!path.Contains(sampleID)) continue;

                if(isResource && path.Contains("Resource"))
                {
                    if (System.IO.Path.GetFileName(path).Contains(objectName))
                    {
                        return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                    }
                }

                if(System.IO.Path.GetFileName(path).Contains(objectName))
                {
                    return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                }
            }

            return null;
        }

        private static Object GetPackageAsset<T>(string path)
        {
            return AssetDatabase.LoadAssetAtPath(path.Replace("Assets", "Packages"), typeof(T));
        }

        protected static void DisplayWarningMessage(string str)
        {
            if (EditorUtility.DisplayDialog("BRANDLAB360", str, "OK"))
            {

            }
        }

        protected static UnityEngine.Object[] GetAssets(string path)
        {
            UnityEngine.Object[] obj = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);

            if (obj == null)
            {
                obj = GetPackageAssets(path);
            }

            return obj;
        }

        protected static UnityEngine.Object[] GetPackageAssets(string path)
        {
            return UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path.Replace("Assets", "Packages"));
        }
    }
}
