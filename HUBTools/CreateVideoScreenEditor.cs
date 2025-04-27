using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateVideoScreenEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Screens/Video")]
        public static void Create()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Screen_Video.prefab");
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = new Vector3(0, 2, 0);
            go.name = "VideoScreen_";

            CreateParentAndSelectObject("_VIDEOSCREENS", go.transform);
        }
    }
}
