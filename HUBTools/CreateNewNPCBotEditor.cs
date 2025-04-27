using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateNewNPCBotEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/NPC Bots/Replicate NPC BOT Prefab")]
        public static void Create()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            GameObject obj = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Resources/NPC/NPCBot.prefab");

            string guid = System.Guid.NewGuid().ToString();

            //save new player
            if (AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(obj), "Assets/Resources/NPCBot_" + guid + ".prefab"))
            {
                AssetDatabase.SaveAssets();
                obj = Resources.Load<GameObject>("NPCBot_" + guid);

                if (obj != null)
                {
                    Selection.activeTransform = obj.transform;
                    SceneView.lastActiveSceneView.FrameSelected();
                }
            }
        }

        //[MenuItem("BrandLab360/Core/NPC Bots/Replicate NPC BOT INTERACT Prefab")]
        public static void CreateInteract()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            GameObject obj = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Resources/NPC/NPCBotInteract.prefab");

            string guid = System.Guid.NewGuid().ToString();

            //save new player
            if (AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(obj), "Assets/Resources/NPCBotInteract_" + guid + ".prefab"))
            {
                AssetDatabase.SaveAssets();
                obj = Resources.Load<GameObject>("NPCBotInteract_" + guid);

                if (obj != null)
                {
                    Selection.activeTransform = obj.transform;
                    SceneView.lastActiveSceneView.FrameSelected();
                }
            }
        }
    }
}
