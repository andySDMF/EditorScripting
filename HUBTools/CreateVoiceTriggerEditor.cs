using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateVoiceTriggerEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Voice/Trigger")]
        public static void Create()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/VoiceTrigger.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "VoiceTrigger_";

                CreateParentAndSelectObject("_VOICETRIGGERS", go.transform);
            }
        }
    }
}
