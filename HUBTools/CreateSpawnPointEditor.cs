using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateSpawnPointEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Spawn/Spawn Point")]
        public static void CreateSpawn()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/SpawnPoint.prefab");
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "SpawnPoint_";

            CreateParentAndSelectObject("_SPAWN", go.transform);
        }
    }
}
