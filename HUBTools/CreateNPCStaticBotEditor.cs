using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace BrandLab360.Editor
{
    public class CreateNPCStaticBotEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/NPC Bots/Static Bot")]
        public static void Create()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Resources/NPC/NPCBot.prefab");

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = new Vector3(0,1,0);
            go.transform.localScale = Vector3.one;
            go.name = "StaticNPCBot_";

            NPCBot script = go.GetComponent<NPCBot>();
            script.IsInteractable = true;

            BoxCollider collider = go.AddComponent<BoxCollider>();
            collider.size = new Vector3(1, 2, 1);

            NPCBotFollow follow = go.AddComponent<NPCBotFollow>();
            NPCBotInteraction interact = go.AddComponent<NPCBotInteraction>();
            interact.SpeechFader = go.transform.Find("Canvas_Speech").GetComponentInChildren<CanvasGroup>();
            interact.SpeechText = go.transform.Find("Canvas_Speech").GetComponentInChildren<TextMeshProUGUI>();

            CreateParentAndSelectObject("_NPCSTATICBOTS", go.transform);
        }
    }
}
