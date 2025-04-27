using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace BrandLab360.Editor
{
    public class CreateNoticeBoardEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Boards/Noticeboard/Standard")]
        public static void CreateNoticeBoard()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/NoticeBoard/NoticeBoard.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "NoticeBoard_";

                CreateParentAndSelectObject("_NOTICEBOARDS", go.transform);
            }
        }

        //[MenuItem("BrandLab360/Core/Boards/Noticeboard/Locked")]
        public static void CreateLockedNoticeBoard()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/NoticeBoard/NoticeBoard.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "NoticeBoard_";

                prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Lock.prefab");

                if (prefab != null)
                {
                    GameObject lGO = Instantiate(prefab as GameObject, Vector3.zero, Quaternion.identity);
                    lGO.name = "Lock";

                    UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_2.png");

                    for (int j = 0; j < atlas.Length; j++)
                    {
                        if (atlas[j].name.Contains("IconLock"))
                        {
                            lGO.transform.Find("Container_Viewport/Button_Lock/Icon_Lock").GetComponent<Image>().sprite = (Sprite)atlas[j];

                            break;
                        }
                    }
                    
                    float bottom = 0 - (go.GetComponent<BoxCollider>().size.y / 2);
                    lGO.transform.rotation = go.transform.rotation;
                    lGO.transform.SetParent(go.transform);
                    lGO.transform.localPosition = new Vector3(0, bottom, -0.02f);
                }

                CreateParentAndSelectObject("_NOTICEBOARDS", go.transform);
            }
        }

        //[MenuItem("BrandLab360/Core/Boards/Pinboard/Standard")]
        public static void CreatePinboard()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/NoticeBoard/Pinboard.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "PinBoard_";

                CreateParentAndSelectObject("_NOTICEBOARDS", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Boards/Pinboard/Locked")]
        public static void CreateLockedPinboard()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/NoticeBoard/Pinboard.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.position = Vector3.zero;
                go.name = "PinBoard_";

                prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/Lock.prefab");

                if (prefab != null)
                {
                    GameObject lGO = Instantiate(prefab as GameObject, Vector3.zero, Quaternion.identity);
                    lGO.name = "Lock";

                    UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_2.png");

                    for (int j = 0; j < atlas.Length; j++)
                    {
                        if (atlas[j].name.Contains("IconLock"))
                        {
                            lGO.transform.Find("Container_Viewport/Button_Lock/Icon_Lock").GetComponent<Image>().sprite = (Sprite)atlas[j];

                            break;
                        }
                    }

                    float bottom = 0 - (go.GetComponent<BoxCollider>().size.y / 2);
                    lGO.transform.rotation = go.transform.rotation;
                    lGO.transform.SetParent(go.transform);
                    lGO.transform.localPosition = new Vector3(0, bottom, -0.02f);
                }

                CreateParentAndSelectObject("_NOTICEBOARDS", go.transform);
            }
        }
    }
}
