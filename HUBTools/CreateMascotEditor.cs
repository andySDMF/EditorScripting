using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{

    public class CreateMascotEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Mascots/Canvas")]
        public static void CreateCanvasMascot()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Mascot/CanvasMascot.prefab");

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = Vector3.zero;
            go.name = "CanvasMascot_";

            CreateParentAndSelectObject("_MASCOTS", go.transform);
        }
    }
}
