using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateProductPlacementTypeEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Product Placement/Wall")]
        public static void CreateWall()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/ProductPlacement/ProductPlacement_Wall.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "ProductPlacementWall_";

                CreateParentAndSelectObject("_PRODUCTPLACEMENT", go.transform);
            }
        }

       //[MenuItem("BrandLab360/Core/Product Placement/Rail")]
        public static void CreateRail()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/ProductPlacement/ProductPlacement_Rail.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "ProductPlacementRail_";

                CreateParentAndSelectObject("_PRODUCTPLACEMENT", go.transform);
            }
        }

        //[MenuItem("BrandLab360/Core/Product Placement/Table")]
        public static void CreateTable()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/ProductPlacement/ProductPlacement_Table.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "ProductPlacementTable_";

                CreateParentAndSelectObject("_PRODUCTPLACEMENT", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Product Placement/Trigger")]
        public static void CreateTrigger()
        {
            ProductPlacementTrigger trigger = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<ProductPlacementTrigger>();
            trigger.GetComponent<Collider>().isTrigger = true;
            trigger.transform.localScale = new Vector3(2, 1, 2);
            trigger.gameObject.GetComponent<Renderer>().material = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");
            trigger.name = "ProductPlacementTrigger_";

            CreateParentAndSelectObject("_PRODUCTPLACEMENT", trigger.transform);
        }
    }
}
