using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreatePickupObjectsEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Pickup/Primitive/Cube")]
        public static void CreateCube()
        {
            PickupItem trigger = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<PickupItem>();
            trigger.name = "Pickup_Cube";
            trigger.transform.localPosition = Vector3.zero;
            trigger.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            trigger.GetComponent<Renderer>().material = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Pickup.mat");

            CreateParentAndSelectObject("_PICKUPITEMS", trigger.transform);
        }

        //[MenuItem("BrandLab360/Core/Pickup/Primitive/Sphere")]
        public static void CreateSphere()
        {
            PickupItem trigger = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PickupItem>();
            trigger.name = "Pickup_Sphere";
            trigger.transform.localPosition = Vector3.zero;
            trigger.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            trigger.GetComponent<Renderer>().material = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Pickup.mat");

            CreateParentAndSelectObject("_PICKUPITEMS", trigger.transform);
        }

        //[MenuItem("BrandLab360/Core/Pickup/Primitive/Cylinder")]
        public static void CreateCylinder()
        {
            PickupItem trigger = GameObject.CreatePrimitive(PrimitiveType.Cylinder).AddComponent<PickupItem>();
            trigger.name = "Pickup_Cylinder";
            trigger.transform.localPosition = Vector3.zero;
            trigger.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            trigger.GetComponent<Renderer>().material = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Pickup.mat");

            CreateParentAndSelectObject("_PICKUPITEMS", trigger.transform);
        }

        //[MenuItem("BrandLab360/Core/Pickup/Drop Point/2D")]
        public static void Create2DDropPoint()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/DropPoint.prefab");

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = Vector3.zero;
            go.name = "DropPoint_";

            CreateParentAndSelectObject("_DROPPOINTS", go.transform);
        }

        //[MenuItem("BrandLab360/Core/Pickup/Drop Point/3D")]
        public static void Create3DDropPoint()
        {
            DropPoint dp = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<DropPoint>();
            dp.name = "3DDropPoint_";
            dp.transform.localPosition = Vector3.zero;
            dp.transform.localScale = new Vector3(1f, 0.1f, 1f);
            dp.GetComponent<Renderer>().material = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");

            CreateParentAndSelectObject("_DROPPOINTS", dp.transform);
        }
    }
}
