using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class UpdateSamplesUIEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Samples/UI/Update")]
        public static void UpdateSamples()
        {
            //get CORE prefab
            CoreManager CORE = Selection.activeTransform.GetComponent<CoreManager>();

            if(CORE != null)
            {
                //get hud manage from prefab
                HUDManager hud = CORE.gameObject.GetComponentInChildren<HUDManager>();
                SerializedObject sObject = new SerializedObject(hud);

                if(sObject != null)
                {
                    GameObject go;
                    GameObject prefab;
                    bool success;

                    //loop through all the hud controls, messages, screens
                    for (int i = 0; i < sObject.FindProperty("HUDControls").arraySize; i++)
                    {
                        string id = sObject.FindProperty("HUDControls").GetArrayElementAtIndex(i).FindPropertyRelative("id").stringValue;
                        go = sObject.FindProperty("HUDControls").GetArrayElementAtIndex(i).FindPropertyRelative("display").objectReferenceValue as GameObject;
                        prefab = Instantiate(go, Vector3.zero, Quaternion.identity, go.transform.parent);

                        if(PrefabUtility.SaveAsPrefabAsset(prefab, $"{ "Assets/com.brandlab360.core/Samples/UI Sample/Controls"}/{ "CUSTOM_" + id }.prefab", out success))
                        {

                        }

                        DestroyImmediate(prefab);
                    }

                    for (int i = 0; i < sObject.FindProperty("HUDMessages").arraySize; i++)
                    {
                        string id = sObject.FindProperty("HUDMessages").GetArrayElementAtIndex(i).FindPropertyRelative("id").stringValue;
                        go = sObject.FindProperty("HUDMessages").GetArrayElementAtIndex(i).FindPropertyRelative("display").objectReferenceValue as GameObject;
                        prefab = Instantiate(go, Vector3.zero, Quaternion.identity, go.transform.parent);

                        if (PrefabUtility.SaveAsPrefabAsset(prefab, $"{ "Assets/com.brandlab360.core/Samples/UI Sample/Messages"}/{ "CUSTOM_" + id }.prefab", out success))
                        {

                        }

                        DestroyImmediate(prefab);
                    }

                    for (int i = 0; i < sObject.FindProperty("HUDScreens").arraySize; i++)
                    {
                        string id = sObject.FindProperty("HUDScreens").GetArrayElementAtIndex(i).FindPropertyRelative("id").stringValue;
                        go = sObject.FindProperty("HUDScreens").GetArrayElementAtIndex(i).FindPropertyRelative("display").objectReferenceValue as GameObject;
                        prefab = Instantiate(go, Vector3.zero, Quaternion.identity, go.transform.parent);

                        if (PrefabUtility.SaveAsPrefabAsset(prefab, $"{ "Assets/com.brandlab360.core/Samples/UI Sample/Screens"}/{ "CUSTOM_" + id }.prefab", out success))
                        {

                        }

                        DestroyImmediate(prefab);
                    }

                    //welcome panel
                    go = sObject.FindProperty("welcomePanel").objectReferenceValue as GameObject;
                    prefab = Instantiate(go, Vector3.zero, Quaternion.identity, go.transform.parent);

                    if (PrefabUtility.SaveAsPrefabAsset(prefab, $"{ "Assets/com.brandlab360.core/Samples/UI Sample/Overlay"}/{ "CUSTOM_WELCOME_OVERLAY" }.prefab", out success))
                    {

                    }

                    DestroyImmediate(prefab);

                    //player settings panel
                    go = sObject.FindProperty("settingsPanel").objectReferenceValue as GameObject;
                    prefab = Instantiate(go, Vector3.zero, Quaternion.identity, go.transform.parent);

                    if (PrefabUtility.SaveAsPrefabAsset(prefab, $"{ "Assets/com.brandlab360.core/Samples/UI Sample/Overlay"}/{ "CUSTOM_PLAYERSETTINGS_OVERLAY" }.prefab", out success))
                    {

                    }

                    DestroyImmediate(prefab);
                }

                //save the assets
                AssetDatabase.SaveAssets();
            }
            else
            {
                //show pop up
                EditorUtility.DisplayDialog("Warning", "Please select the CORE.prefab in the current scene.", "OK");
            }
        }
    }
}
