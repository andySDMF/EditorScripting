using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateTriggerChairGroupEditor : CreateStandardChairGroupEditor
    {
        //[MenuItem("BrandLab360/Core/Chair Group/Trigger")]
       /* public static new void Create()
        {
            callback = Callback;

            //show window to define chair options
            ChairGroupWindow window = (ChairGroupWindow)EditorWindow.GetWindow(typeof(ChairGroupWindow));
            window.setup = "Trigger Chair Group";
            window.maxSize = new Vector2(500f, 500f);
            window.minSize = window.maxSize;
            window.Show();
        }*/

        public static void TriggerCallback(string name, List<ChairObject> chairs)
        {
            CreateTrigger(name, chairs);

            ChairGroup groupScript = chairGroup.AddComponent<ChairGroup>();
            groupScript.EditorSetGroupCamera(VRCamera);

            HUBUtils.CreateParentAndSelectObject("_CHAIRGROUPS", chairGroup.transform, false);
        }

        protected static void CreateTrigger(string name, List<ChairObject> chairs)
        {
            //create all the default things required for standard
            chairGroup = new GameObject();
            chairGroup.name = "TriggerChairGroup_" + name;
            chairGroup.transform.localScale = Vector3.one;
            List<Transform> chairTransforms = new List<Transform>();

            if (chairs.Count > 0)
            {
                for (int i = 0; i < chairs.Count; i++)
                {
                    chairTransforms.Add(chairs[i].chair.transform);
                }

                chairGroup.transform.position = FindThePivot(chairTransforms.ToArray());
            }
            else
            {
                chairGroup.transform.position = Vector3.zero;
            }

            BoxCollider boxCollider = chairGroup.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            if(chairs.Count > 0)
            {
                Vector3 area = ChairArea(chairTransforms.ToArray());
                boxCollider.size = new Vector3(area.x * 1.5f, 2, area.z * 2.0f);
            }
            else
            {
                boxCollider.size = new Vector3(2, 2, 2);
            }
            
            boxCollider.center = new Vector3(0, 0.5f, 0);

            GameObject chairContainer = new GameObject();
            chairContainer.transform.SetParent(chairGroup.transform);
            chairContainer.transform.localScale = Vector3.one;
            chairContainer.transform.position = Vector3.zero;
            chairContainer.name = "Chairs";

            //create chair prefab
            UnityEngine.Object prefab = null;
            chairTransforms = new List<Transform>();

            if (chairs == null || chairs.Count <= 0)
            {
                prefab = (GameObject)HUBUtils.GetAsset<GameObject>("Assets/com.brandlab360.core/Samples/HDRP Sample/Models/Chair.fbx");
                CreateChair(prefab, chairContainer, true);
            }
            else
            {
                for (int i = 0; i < chairs.Count; i++)
                {
                    if (chairs[i].chair.Equals(null) || (chairs[i].quantity <= 0 && chairs[i].create)) continue;

                    if (chairs[i].create)
                    {
                        for (int j = 0; j < chairs[i].quantity; j++)
                        {
                            chairTransforms.Add(CreateChair(chairs[i].chair, chairContainer, chairs[i].create));
                        }
                    }
                    else
                    {
                        chairTransforms.Add(CreateChair(chairs[i].chair, chairContainer, chairs[i].create));
                    }
                }
            }

            GameObject chairCameras = new GameObject();
            chairCameras.transform.SetParent(chairGroup.transform);
            chairCameras.transform.localScale = Vector3.one;
            chairCameras.transform.position = Vector3.zero;
            chairCameras.name = "Cameras";

            //add camera to chairCameres
            prefab = (GameObject)HUBUtils.GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/VirtualCamera.prefab");
            VRCamera = (GameObject)UnityEngine.Object.Instantiate(prefab);
            VRCamera.transform.SetParent(chairCameras.transform);

            Vector3 vec = FindThePivot(chairTransforms.ToArray());
            VRCamera.transform.position = chairTransforms.Count > 0 ? new Vector3(vec.x, 1.7f, vec.z) : new Vector3(-3, 1.7f, 0.15f);
            VRCamera.transform.localScale = Vector3.one;
            VRCamera.transform.localEulerAngles = new Vector3(30, 90, 0);
            VRCamera.name = "ChairCamera_Main";
            VRCamera.SetActive(false);
        }

        [MenuItem("GameObject/BrandLab360/Create Chair Group/Trigger", false)]
        protected static void ValidateCreateTrigger(MenuCommand command)
        {
            if (UnityEditor.Selection.objects.Length <= 0)
            {
                return;
            }

            if (command.context == UnityEditor.Selection.objects[0])
            {
                List<ChairObject> chairs = new List<ChairObject>();

                for (int i = 0; i < UnityEditor.Selection.objects.Length; i++)
                {
                    ChairObject obj = new ChairObject();
                    obj.chair = (GameObject)UnityEditor.Selection.objects[i];
                    obj.create = false;

                    chairs.Add(obj);
                }

                UnityEditor.Selection.objects = new Object[0];
                Selection.activeTransform = null;

                TriggerCallback("", chairs);
            }
        }
    }
}
