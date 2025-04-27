using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateLockEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Locks/Lock")]
        public static void Create()
        {
            GameObject goLock = CreateLock();

            CreateParentAndSelectObject("_LOCKS", goLock.transform);
        }

        public static GameObject CreateLock()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Lock.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "Lock_";

                return go;
            }
            else
            {
                return null;
            }
        }
    }
}
