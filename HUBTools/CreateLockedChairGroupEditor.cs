using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateLockedChairGroupEditor : CreateStandardChairGroupEditor
    {
        //[MenuItem("BrandLab360/Core/Chair Group/Locked")]
       /* public static new void Create()
        {
            callback = Callback;

            //show window to define chair options
            ChairGroupWindow window = (ChairGroupWindow)EditorWindow.GetWindow(typeof(ChairGroupWindow));
            window.setup = "Locked Chair Group";
            window.maxSize = new Vector2(500f, 500f);
            window.minSize = window.maxSize;
            window.Show();
        }*/

        public static void LockedCallback(string name, List<ChairObject> chairs)
        {
            CreateLocked(name, chairs);

            ChairGroup groupScript = chairGroup.AddComponent<ChairGroup>();
            groupScript.EditorSetGroupCamera(VRCamera);

            HUBUtils.CreateParentAndSelectObject("_CHAIRGROUPS", chairGroup.transform, false);
        }

        protected static void CreateLocked(string name, List<ChairObject> chairs)
        {
            //create all the default things required for locked
            chairGroup = new GameObject();
            chairGroup.name = "LockedChairGroup_" + name;
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

            GameObject chairContainer = new GameObject();
            chairContainer.transform.SetParent(chairGroup.transform);
            chairContainer.transform.localScale = Vector3.one;
            chairContainer.transform.position = Vector3.zero;
            chairContainer.name = "Chairs";

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
                            chairTransforms.Add(CreateLockChair(chairs[i].chair, chairContainer, chairs[i].create));
                        }
                    }
                    else
                    {
                        chairTransforms.Add(CreateLockChair(chairs[i].chair, chairContainer, chairs[i].create));
                    }
                }
            }

            GameObject chairCameras = new GameObject();
            chairCameras.transform.SetParent(chairGroup.transform);
            chairCameras.transform.localScale = Vector3.one;
            chairCameras.transform.position = Vector3.zero;
            chairCameras.name = "Cameras";

            //add camera to chairCameres
            prefab = (GameObject)HUBUtils .GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/VirtualCamera.prefab");
            VRCamera = (GameObject)UnityEngine.Object.Instantiate(prefab);
            VRCamera.transform.SetParent(chairCameras.transform);

            Vector3 vec = FindThePivot(chairTransforms.ToArray());
            VRCamera.transform.position = chairTransforms.Count > 0 ? new Vector3(vec.x, 1.7f, vec.z) : new Vector3(-3, 1.7f, 0.15f);
            VRCamera.transform.localScale = Vector3.one;
            VRCamera.transform.localEulerAngles = new Vector3(30, 90, 0);
            VRCamera.name = "ChairCamera_Main";
            VRCamera.SetActive(false);

            //create global lock for the chair group
            GameObject groupLock = CreateLockEditor.CreateLock();
            groupLock.transform.SetParent(chairGroup.transform);
            groupLock.name = "Lock";

            UnityEngine.Object[] atlas = HUBUtils.GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_2.png");

            for (int j = 0; j < atlas.Length; j++)
            {
                if (atlas[j].name.Contains("IconLock"))
                {
                    groupLock.transform.Find("Container_Viewport/Button_Lock/Icon_Lock").GetComponent<Image>().sprite = (Sprite)atlas[j];

                    break;
                }
            }

            groupLock.transform.localPosition = Vector3.up;
            groupLock.transform.eulerAngles = chairGroup.transform.eulerAngles;
        }

        protected static Transform CreateLockChair(UnityEngine.Object prefab, GameObject chairContainer, bool create)
        {
            GameObject go = null;

            if (prefab != null)
            {
                if (create)
                {
                    go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    go.transform.SetParent(chairContainer.transform);
                    go.transform.position = new Vector3(0, 0, 0);
                    go.transform.localScale = Vector3.one;
                    go.transform.localEulerAngles = new Vector3(270, 0, 0);

                    //need to check if this is bench or not
                    if (prefab.name.Contains("Bench") || prefab.name.Contains("bench"))
                    {
                        go.transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                }
                else
                {
                    go = (GameObject)prefab;
                    go.transform.SetParent(chairContainer.transform);
                }

                if (!go.GetComponent<Collider>())
                {
                    go.AddComponent<BoxCollider>();
                }

                //need to check if this is bench or not
                if (prefab.name.Contains("Bench") || prefab.name.Contains("bench"))
                {
                    Bench ch = go.GetComponent<Bench>();

                    if (ch == null)
                    {
                        ch = go.AddComponent<Bench>();
                    }

                    if (!ch.HasSittingSpot)
                    {
                        Transform[] spots = new Transform[1];
                        BoxCollider col = go.GetComponent<BoxCollider>();

                        if (col != null)
                        {
                            col.size = new Vector3(2, 1, 1);
                            col.center = new Vector3(0.0f, 0.5f, 0.0f);
                        }

                        for (int j = 0; j < 1; j++)
                        {
                            GameObject sittingPoint = new GameObject();
                            sittingPoint.transform.SetParent(go.transform);
                            sittingPoint.name = "SittingPoint";
                            sittingPoint.transform.localScale = Vector3.one;

                            if (col != null)
                            {
                                sittingPoint.transform.eulerAngles = col.transform.eulerAngles;
                                sittingPoint.transform.localPosition = new Vector3(0.0f, col.center.y, col.center.z);
                            }
                            else
                            {
                                sittingPoint.transform.eulerAngles = Vector3.zero;
                                sittingPoint.transform.localPosition = Vector3.zero;
                            }

                            spots[j] = sittingPoint.transform;
                        }

                        ch.EditorSetChairVars(spots);
                    }
                }
                else
                {
                    Chair ch = go.GetComponent<Chair>();

                    if (ch == null)
                    {
                        ch = go.AddComponent<Chair>();
                    }

                    if (!ch.HasSittingSpot)
                    {
                        GameObject sittingPoint = new GameObject();
                        sittingPoint.transform.SetParent(go.transform);
                        sittingPoint.name = "SittingPoint";

                        BoxCollider col = go.GetComponent<BoxCollider>();
                        sittingPoint.transform.localScale = Vector3.one;

                        if (col != null)
                        {
                            sittingPoint.transform.eulerAngles = col.transform.eulerAngles;
                            sittingPoint.transform.localPosition = new Vector3(0.0f, col.center.y, col.center.z);
                        }
                        else
                        {
                            sittingPoint.transform.eulerAngles = Vector3.zero;
                            sittingPoint.transform.localPosition = Vector3.zero;
                        }

                        ch.EditorSetChairVars(sittingPoint.transform);
                    }
                }
            }

            return go.transform;
        }

        [MenuItem("GameObject/BrandLab360/Create Chair Group/Locked", false)]
        protected static void ValidateCreateLocked(MenuCommand command)
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

                LockedCallback("", chairs);
            }
        }
    }
}
