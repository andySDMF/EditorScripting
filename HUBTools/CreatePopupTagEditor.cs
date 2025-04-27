using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreatePopupTagEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Popup Tag/Video")]
        public static void CreateVideo()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/PopUpTag.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.GetComponentInChildren<PopupTag>().Type = InfotagType.Video;

                UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_2.png");

                for(int i = 0; i < atlas.Length; i++)
                {
                    if(atlas[i].name.Contains("IconPlay"))
                    {
                        go.GetComponentInChildren<PopupTag>().Icon.sprite = (Sprite)atlas[i];
                        break;
                    }
                }

                go.transform.position = Vector3.zero;
                go.name = "PopupTagVideo_";

                CreateParentAndSelectObject("_POPUPTAGS", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Popup Tag/Image")]
        public static void CreateImage()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/PopUpTag.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.GetComponentInChildren<PopupTag>().Type = InfotagType.Image;

                UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_1.png");

                for (int i = 0; i < atlas.Length; i++)
                {
                    if (atlas[i].name.Contains("IconPhoto"))
                    {
                        go.GetComponentInChildren<PopupTag>().Icon.sprite = (Sprite)atlas[i];
                        break;
                    }
                }

                go.transform.position = Vector3.zero;
                go.name = "PopupTagImage_";

                CreateParentAndSelectObject("_POPUPTAGS", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Popup Tag/Website")]
        public static void CreateWebsite()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/PopUpTag.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.GetComponentInChildren<PopupTag>().Type = InfotagType.Web;


                UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_1.png");

                for (int i = 0; i < atlas.Length; i++)
                {
                    if (atlas[i].name.Contains("IconWeb"))
                    {
                        go.GetComponentInChildren<PopupTag>().Icon.sprite = (Sprite)atlas[i];
                        break;
                    }
                }
                go.transform.position = Vector3.zero;
                go.name = "PopupTagWebsite_";

                CreateParentAndSelectObject("_POPUPTAGS", go.transform);
            }
        }

       // [MenuItem("BrandLab360/Core/Popup Tag/Text")]
        public static void CreateText()
        {
            UnityEngine.Object prefab = (GameObject)GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/PopUpTag.prefab");

            if (prefab != null)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.GetComponentInChildren<PopupTag>().Type = InfotagType.Text;

                UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_1.png");

                for (int i = 0; i < atlas.Length; i++)
                {
                    if (atlas[i].name.Contains("IconInformation"))
                    {
                        go.GetComponentInChildren<PopupTag>().Icon.sprite = (Sprite)atlas[i];

                        break;
                    }
                }

                go.transform.position = Vector3.zero;
                go.name = "PopupTagText_";

                CreateParentAndSelectObject("_POPUPTAGS", go.transform);
            }
        }
    }
}
