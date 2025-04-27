using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateTeleportEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Teleport/Point")]
        public static void CreatePoint()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Navigation/TeleportPoint.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "TeleportPoint_";

                CreateParentAndSelectObject("_TELEPORTPOINTS", go.transform);
            }
        }
    }
}