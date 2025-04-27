using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateNPCBotSpawnAreaEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/NPC Bots/Spawn Area")]
        public static void Create()
        {
            NPCBotSpawnArea botSpawn = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<NPCBotSpawnArea>();
            botSpawn.GetComponent<MeshRenderer>().enabled = false;
            botSpawn.transform.localScale = Vector3.one;
            botSpawn.transform.position = Vector3.zero;
            botSpawn.name = "NPCBotSpawnArea_";
            botSpawn.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            botSpawn.GetComponent<Collider>().isTrigger = true;

            CreateParentAndSelectObject("_NAVMESH", botSpawn.transform);
        }
    }
}
