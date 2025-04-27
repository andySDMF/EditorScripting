using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateAssortmentEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Assortment/Wall")]
        public static void CreateWall()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Assortments/Assortment Wall.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "AssortmentWall_";

                CreateParentAndSelectObject("_ASSORTMENTS", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Assortment/Table")]
        public static void CreateTable()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Assortments/Assortment Table.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "AssortmentTable_";

                CreateParentAndSelectObject("_ASSORTMENTS", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Assortment/Rail")]
        public static void CreateRail()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Assortments/Assortment Rail.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "AssortmentRail_";

                CreateParentAndSelectObject("_ASSORTMENTS", go.transform);
            }
        }
    }
}
