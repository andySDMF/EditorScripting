using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateNewPlayerControllerEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Player/Replicate Base Prefab")]
        public static void CreateController()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            GameObject obj =  (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Resources/BLPlayer.prefab");

            //save new player
            string guid = System.Guid.NewGuid().ToString();

            if (AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(obj), "Assets/Resources/BLPlayer_" + guid + ".prefab"))
            {
                AssetDatabase.SaveAssets();
                obj = Resources.Load<GameObject>("BLPlayer_" + guid);

                if (obj != null)
                {
                    Selection.activeTransform = obj.transform;
                    SceneView.lastActiveSceneView.FrameSelected();

                    //apply to app settings
                    AppConstReferences appRefs = Resources.Load<AppConstReferences>("AppConstReferences");

                    if(appRefs != null)
                    {
                        appRefs.Settings.playerSettings.playerController = "BLPlayer_" + guid;
                    }
                }

                if (obj != null)
                {
                    EditorGUIUtility.PingObject(obj);
                }
            }
        }

        //[MenuItem("BrandLab360/Core/Player/Replicate Avatar controller")]
        public static void CreateAnimator()
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string assetPath = "";

            if (System.IO.Directory.Exists(Application.dataPath + "/com.brandlab360.core"))
            {
                assetPath = "Assets/com.brandlab360.core/Runtime/Avatars/Animation Controllers/Resources/Avatar.controller";
            }
            else
            {
                assetPath = "Packages/com.brandlab360.core/Runtime/Avatars/Animation Controllers/Resources/Avatar.controller";
            }

            string guid = System.Guid.NewGuid().ToString();

            if (!AssetDatabase.CopyAsset(assetPath, $"Assets/Resources/Avatar_" + guid + ".controller"))
            {
                Debug.LogWarning($"Failed to copy {assetPath}");
            }
            else
            {
                AssetDatabase.Refresh();
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/Resources/Avatar_" + guid + ".controller");

                if (obj != null)
                {
                    EditorGUIUtility.PingObject(obj);
                }
            }
        }
    }
}
