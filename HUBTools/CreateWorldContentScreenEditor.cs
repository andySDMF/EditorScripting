using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateWorldContentScreenEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Content Upload/Image")]
        public static void CreateImage()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/UploadContent_Image.prefab");
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "ImageUploadContent_";

            CreateParentAndSelectObject("_CONTENTSCREENS", go.transform);
        }

        //[MenuItem("BrandLab360/Core/Content Upload/Video")]
        public static void CreateVideo()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/UploadContent_Video.prefab");
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "VideoUploadContent_";

            CreateParentAndSelectObject("_CONTENTSCREENS", go.transform);
        }


        //[MenuItem("BrandLab360/Core/Content Upload/PDF")]
        public static void CreatePDF()
        {
            DisplayWarningMessage("This feature does not exist yet");
        }

        //[MenuItem("BrandLab360/Core/Content Upload/All")]
        public static void CreateAll()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/UploadContent_All.prefab");
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "AllUploadContent_";

            CreateParentAndSelectObject("_CONTENTSCREENS", go.transform);
        }
    }
}
