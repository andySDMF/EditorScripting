using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace BrandLab360.Editor
{
    public class CreateTopDownCameraEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Map/Topdown Camera")]
        public static void Create()
        {
            List<MainTopDownCamera> current = FindObjectsByType<MainTopDownCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

            MapManager mapManager = FindFirstObjectByType<MapManager>();

            if(mapManager != null && mapManager.TopDownCameraExists != null)
            {
                current.Remove(mapManager.TopDownCameraExists);
            }

            bool create = false;

            if(current.Count >= 1)
            {
                if(EditorUtility.DisplayDialog("Scene already contains MainTopDownCamera!",
                "Are you sure you want to add another MainTopDownCamera?", "Yes", "Cancel"))
                {
                    create = true;
                }
            }
            else
            {
                create = true;
            }

            if(create)
            {
                UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/TopDownCamera.prefab");
                GameObject go = (GameObject)Instantiate(prefab);
                go.transform.position = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.name = "MainTopDownCamera";

                go.transform.position = new Vector3(0, 200, 0);

                CreateParentAndSelectObject("_TOPDOWNCAMERA", go.transform);
            }
        }
    }
}
