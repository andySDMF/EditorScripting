using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

#if UNITY_PIPELINE_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

using System.IO;

namespace BrandLab360.Editor
{
    public static class Pipeline
    {
        public static void AddDiffusionProfilesHDRP()
        {
#if UNITY_PIPELINE_HDRP

            RenderPipelineGlobalSettings pipelineSettings = GraphicsSettings.GetSettingsForRenderPipeline<HDRenderPipeline>();
            if (!pipelineSettings) return;
            string assetPath = AssetDatabase.GetAssetPath(pipelineSettings);

            SerializedObject hdrp = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(assetPath)[0]);

            if (hdrp == null) return;

            int index;
            bool modified = false;
            string[] profiles = new string[] { "RL_Skin_Profile", "RL_Teeth_Profile", "RL_Eye_Profile", "RL_SSS_Profile" };

            SerializedProperty list = hdrp.FindProperty("diffusionProfileSettingsList");

            if (list == null)
            {
                return;
            }

            SerializedProperty item;            

            foreach (string profile in profiles)
            {
                Object asset = FindAsset(profile);

                if (asset)
                {
                    bool add = true;

                    foreach (SerializedProperty p in list)
                    {
                        if (p.objectReferenceValue == asset) add = false;
                    }

                    if (add)
                    {
                        index = list.arraySize;
                        if (index < 15)
                        {
                            list.InsertArrayElementAtIndex(index);
                            item = list.GetArrayElementAtIndex(index);
                            item.objectReferenceValue = asset;
                            modified = true;
                        }
                        else
                        {
                            Debug.LogWarning("Maximum number of diffusion profiles reached! Unable to add profile: " + profile);
                        }
                    }
                }
            }

            if (modified) hdrp.ApplyModifiedProperties();

#endif
        }

        private static Object FindAsset(string search, string[] folders = null)
        {
            if (folders == null) folders = new string[] { "Assets", "Packages" };

            string[] guids = AssetDatabase.FindAssets(search, folders);

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string assetName = Path.GetFileNameWithoutExtension(assetPath);

                if (assetName.Equals(search))
                {
                    return AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                }
            }

            return null;
        }
    }
}
