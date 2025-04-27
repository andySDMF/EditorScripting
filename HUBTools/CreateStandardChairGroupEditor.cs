using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateStandardChairGroupEditor
    {
        protected static GameObject chairGroup;
        protected static GameObject VRCamera;
        protected static System.Action<string, List<ChairObject>> callback;

        //[MenuItem("BrandLab360/Core/Chair Group/Standard")]
       /* public static void Create()
        {
            callback = Callback;

            //show window to define chair options
             ChairGroupWindow window = (ChairGroupWindow)EditorWindow.GetWindow(typeof(ChairGroupWindow));
             window.setup = "Standard Chair Group";
             window.maxSize = new Vector2(800f, 500f);
             window.minSize = window.maxSize;
             window.Show();
        }*/

        protected static void PerformCreation(string name, List<ChairObject> chairs)
        {
            if(callback != null)
            {
                callback.Invoke(name, chairs);
                callback = null;
            }
        }

        public static void StandardCallback(string name, List<ChairObject> chairs)
        {
            CreateStandard(name, chairs);

            ChairGroup groupScript = chairGroup.AddComponent<ChairGroup>();
            groupScript.EditorSetGroupCamera(VRCamera);

            HUBUtils.CreateParentAndSelectObject("_CHAIRGROUPS", chairGroup.transform, false);
        }

        protected static List<Transform> CreateStandard(string name, List<ChairObject> chairs)
        {
            //create all the default things required for standard
            chairGroup = new GameObject();
            chairGroup.name = "StandardChairGroup_" + name;
            chairGroup.transform.localScale = Vector3.one;
            List<Transform> chairTransforms = new List<Transform>();

            if (chairs.Count > 0)
            {
                for(int i = 0; i < chairs.Count; i++)
                {
                    chairTransforms.Add(chairs[i].chair.transform);
                }

                chairGroup.transform.position = FindTheCenter(chairTransforms.ToArray());
            }
            else
            {
                chairGroup.transform.position = Vector3.zero;
            }

            GameObject chairContainer = new GameObject();
            chairContainer.transform.SetParent(chairGroup.transform);
            chairContainer.transform.localScale = Vector3.one;
            chairContainer.transform.localPosition = Vector3.zero;
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
                for(int i = 0; i < chairs.Count; i++)
                {
                    if (chairs[i].chair.Equals(null) || (chairs[i].quantity <= 0 && chairs[i].create)) continue;

                    if(chairs[i].create)
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
            chairCameras.transform.localPosition = Vector3.zero;
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

            return chairTransforms;
        }

        protected static Vector3 FindThePivot(Transform[] trans)
        {
            if (trans == null || trans.Length == 0)
                return Vector3.zero;
            if (trans.Length == 1)
                return trans[0].position;

            float minX = Mathf.Infinity;
            float minY = Mathf.Infinity;
            float minZ = Mathf.Infinity;

            float maxX = -Mathf.Infinity;
            float maxY = -Mathf.Infinity;
            float maxZ = -Mathf.Infinity;

            foreach (Transform tr in trans)
            {
                if (tr.position.x < minX)
                    minX = tr.position.x;
                if (tr.position.y < minY)
                    minY = tr.position.y;
                if (tr.position.z < minZ)
                    minZ = tr.position.z;

                if (tr.position.x > maxX)
                    maxX = tr.position.x;
                if (tr.position.y > maxY)
                    maxY = tr.position.y;
                if (tr.position.z > maxZ)
                    maxZ = tr.position.z;
            }

            return new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, (minZ + maxZ) / 2.0f);
        }

        protected static Vector3 FindTheCenter(Transform[] trans)
        {
            if (trans == null || trans.Length == 0)
                return Vector3.zero;
            if (trans.Length == 1)
                return trans[0].position;

            Vector3 vec = Vector3.zero;

            foreach (Transform tr in trans)
            {
                vec += tr.position;
            }

            return vec / trans.Length;
        }

        protected static Vector3 ChairArea(Transform[] trans)
        {
            Vector3 area = Vector3.zero;

            foreach (Transform tr in trans)
            {
                area += tr.position;
            }

            return area;
        }

        protected static Transform CreateChair(UnityEngine.Object prefab, GameObject chairContainer, bool create)
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
                if(prefab.name.Contains("Bench") || prefab.name.Contains("bench"))
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

                        if(col != null)
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
                        
                        if(col != null)
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

            if (go == null)
            {
                return null;
            }

            return go.transform;
        }

        [MenuItem("GameObject/BrandLab360/Create Chair Group/Standard", false)]
        protected static void ValidateCreateStandard(MenuCommand command)
        {
            if (UnityEditor.Selection.objects.Length <= 0)
            {
                return;
            }

            if(command.context == UnityEditor.Selection.objects[0])
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

                StandardCallback("", chairs);
            }
        }

        public class ChairObject
        {
            public GameObject chair;
            public int quantity = 1;
            public bool create = true;
        }

        public class ChairGroupDesigner
        {
            public string setup { get; set; }

            private List<ChairObject> chairs = new List<ChairObject>();
            private Vector2 scrollPos;
            private string groupName = "";

            public System.Action<string, List<ChairObject>> Callback { get; set; }

            public void OnGUI()
            {
                DrawTitle();
                DrawChairs();
                DrawButtons();
            }

            protected virtual void DrawTitle()
            {
                EditorGUILayout.LabelField(setup.ToUpper(), EditorStyles.boldLabel);
                groupName = EditorGUILayout.TextField("Group Name:", groupName);
                EditorGUILayout.Space();
            }

            protected virtual void DrawChairs()
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Chairs", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("If no chair prefabs added, then default chair will be used", EditorStyles.miniBoldLabel);
                EditorGUILayout.Space();

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                int remove = -1;

                for (int i = 0; i < chairs.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    chairs[i].chair = (GameObject)EditorGUILayout.ObjectField("", chairs[i].chair, typeof(GameObject), false);
                    chairs[i].quantity = EditorGUILayout.IntField("", chairs[i].quantity);

                    if (GUILayout.Button("Remove"))
                    {
                        remove = i;
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if(remove >= 0)
                {
                    chairs.RemoveAt(remove);
                    GUIUtility.ExitGUI();
                    return;
                }

                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space();

                if (GUILayout.Button("Add"))
                {
                    chairs.Add(new ChairObject());
                }

                EditorGUILayout.Space();
            }

            protected virtual void DrawButtons()
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Create"))
                {
                    Callback.Invoke(groupName, chairs);
                   //PerformCreation(groupName, chairs);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
