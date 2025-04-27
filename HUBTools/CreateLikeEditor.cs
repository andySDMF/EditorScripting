using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateLikeEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Likes/Like")]
        public static void Create()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Canvas_Like.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "CanvasLike_";

                CreateParentAndSelectObject("_LIKES", go.transform);
            }
        }
    }
}
