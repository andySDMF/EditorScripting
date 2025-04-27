using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BrandLab360.Editor
{
    public class AddChairLockEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Chair Group/Add Chair Lock")]
        public static void AddLock()
        {
            if(Selection.objects.Length > 0)
            {
                List<ChairGroup> temp = new List<ChairGroup>();

                for(int i = 0; i < Selection.objects.Length; i++)
                {
                    if(((GameObject)Selection.objects[i]).GetComponent<IChairObject>() != null)
                    {
                        IChairObject chair = ((GameObject)Selection.objects[i]).GetComponent<IChairObject>();
                        ChairGroup cGroup = chair.GO.transform.GetComponentInParent<ChairGroup>();

                        //cannot add lock to conference or trigger group
                        if (cGroup is ConferenceChairGroup || cGroup.gameObject.GetComponent<BoxCollider>() != null) continue;

                        //if group is a locked group do not add individual locks
                        if (cGroup.IsLockedType()) continue;

                        //create lock
                        GameObject chairLock = CreateLockEditor.CreateLock();
                        chairLock.transform.SetParent(chair.GO.transform);
                        chairLock.name = "Lock";

                        UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_2.png");

                        for (int j = 0; j < atlas.Length; j++)
                        {
                            if (atlas[j].name.Contains("IconLock"))
                            {
                                chairLock.transform.Find("Container_Viewport/Button_Lock/Icon_Lock").GetComponent<Image>().sprite = (Sprite)atlas[j];

                                break;
                            }
                        }

                        if(chair.HasSittingSpot)
                        {
                            chairLock.transform.eulerAngles = chair.SittingDirection();
                            chairLock.transform.position = chair.SittingPosition();
                        }
                        else
                        {
                            chairLock.transform.eulerAngles = chair.SittingDirection();
                            chairLock.transform.localPosition = chair.GO.transform.up;
                        }
                        
                        chair.ChairLock = chairLock.GetComponent<Lock>();

                        if(!temp.Contains(cGroup))
                        {
                            temp.Add(cGroup);
                        }
                    }
                }

                if (temp.Count > 0)
                {
                    //open up GUI window to add password
                    ChairLockPasswordWindow window = (ChairLockPasswordWindow)EditorWindow.GetWindow(typeof(ChairLockPasswordWindow));
                    window.Set(temp);
                    window.maxSize = new Vector2(1024f, 400f);
                    window.minSize = window.maxSize;
                    window.Show();
                }
            }
            else
            {
                Debug.LogError("You need to select a chair first");
            }
        }

       // [MenuItem("BrandLab360/Core/Chair Group/Remove Chair Lock")]
        public static void RemoveLock()
        {
            if (Selection.objects.Length > 0)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    if (((GameObject)Selection.objects[i]).GetComponent<IChairObject>() != null)
                    {
                        IChairObject chair = ((GameObject)Selection.objects[i]).GetComponent<IChairObject>();
                        GameObject lockGO;

                        if(chair.ChairLock != null)
                        {
                            lockGO = chair.ChairLock.gameObject;
                        }
                        else
                        {
                            lockGO = chair.GO.GetComponentInChildren<Lock>(true).gameObject;
                        }

                        if(lockGO != null)
                        {
                            DestroyImmediate(lockGO);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("You need to select a chair first");
            }
        }

        [MenuItem("GameObject/BrandLab360/Add Chair Lock", false)]
        protected static void ValidateAddChairLock(MenuCommand command)
        {
            if (UnityEditor.Selection.objects.Length <= 0)
            {
                return;
            }

            if (command.context == UnityEditor.Selection.objects[0])
            {
                AddLock();
            }
        }

        [MenuItem("GameObject/BrandLab360/Remove Chair Lock", false)]
        protected static void ValidateRemoveChairLock(MenuCommand command)
        {
            if (UnityEditor.Selection.objects.Length <= 0)
            {
                return;
            }

            if (command.context == UnityEditor.Selection.objects[0])
            {
                RemoveLock();
            }
        }

        public class ChairLockPasswordWindow : EditorWindow
        {
            private string m_password = "";
            private List<ChairGroup> m_objs;
            private SerializedObject m_asset;

            public void Set(List<ChairGroup> objs)
            {
                m_password = "";
                m_objs = objs;
            }

            private void OnGUI()
            {
                if (Application.isPlaying)
                {
                    Close();
                    return;
                }

                if (HUBWindow._APPSETTINGS != null && HUBWindow._APPSETTINGS.brandlabLogo_Banner != null)
                {
                    GUILayout.Box(HUBWindow._APPSETTINGS.brandlabLogo_Banner.texture, GUILayout.ExpandWidth(true));
                }
                else
                {
                    HUBWindow._APPSETTINGS.brandlabLogo_Banner = (Sprite)HUBUtils.GetAsset<Sprite>("Assets/com.brandlab360.core/Editor/Sprites/BrandLab360_Banner.png");
                }

                EditorGUILayout.LabelField("CHAIR LOCK PASSWORD", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Password:", EditorStyles.boldLabel, GUILayout.Width(100));
                m_password = EditorGUILayout.TextField(m_password, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Apply"))
                {
                    for (int i = 0; i < m_objs.Count; i++)
                    {
                        m_objs[i].EditorSetGroupLockPassword(m_password);
                        m_asset = new SerializedObject(m_objs[i]);

                        if (m_asset != null) m_asset.ApplyModifiedProperties();

                        if (m_asset != null)
                        {
                            EditorUtility.SetDirty(m_objs[i]);
                        }

                        HUBWindow._APPINSTANCES.AddIOObject(m_objs[i].ID, m_objs[i].GetSettings());
                        HUBWindow._SOINSTANCES.ApplyModifiedProperties();
                    }

                    Close();
                }

                if (GUILayout.Button("Skip"))
                {
                    Close();
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
