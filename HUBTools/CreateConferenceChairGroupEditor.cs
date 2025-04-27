using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace BrandLab360.Editor
{
    public class CreateConferenceChairGroupEditor : CreateStandardChairGroupEditor
    {
        private static GameObject doorObject;

        //[MenuItem("BrandLab360/Core/Chair Group/Conference")]
        /*public new static void Create()
        {
            callback = Callback;
            doorObject = null;

            //show window to define chair options
            ChairGroupWindow window = (ChairGroupWindow)EditorWindow.GetWindow(typeof(ChairGroupWindow));
            window.setup = "Conference Chair Group";
            window.maxSize = new Vector2(500f, 500f);
            window.minSize = window.maxSize;
            window.Show();
        }*/

        public static void ConferenceCallback(string name, List<ChairObject> chairs)
        {
            List<Transform> chairTransforms = CreateStandard(name, chairs);

            RectTransform rectT = null;

            chairGroup.name = "ConferenceChairGroup_" + name;

            //no do the conference
            GameObject screen = new GameObject();
            screen.name = "Screen";
            screen.transform.SetParent(chairGroup.transform);
            screen.transform.localScale = Vector3.one;
            screen.transform.position = FindThePivot(chairTransforms.ToArray());

            UnityEngine.Object prefab = (GameObject)HUBUtils.GetAsset<GameObject>("Assets/com.brandlab360.core/Runtime/Prefabs/UploadContent_Conference.prefab");
            GameObject go = null;
            List<GameObject> loaders = new List<GameObject>();

            if (prefab != null)
            {
                go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                go.transform.SetParent(screen.transform);
                go.transform.localPosition = new Vector3(0, 0, 0);
                go.transform.localScale = new Vector3(2.63654f, 1.581924f, 0.00527308f);
                go.transform.localEulerAngles = new Vector3(0, 270, 0);
                go.name = "ConferenceScreen";

                foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
                {
                    if (t.GetComponent<ContentVideoScreen>() != null)
                    {
                        loaders.Add(t.gameObject);
                    }

                    if (t.GetComponent<ContentImageScreen>() != null)
                    {
                        loaders.Add(t.gameObject);
                    }
                }
            }

            GameObject structure = new GameObject();
            structure.name = "Structure";
            structure.transform.SetParent(chairGroup.transform);
            structure.transform.localScale = Vector3.one;
            structure.transform.position = Vector3.zero;

            ConferenceChairGroup groupScript = chairGroup.AddComponent<ConferenceChairGroup>();
            groupScript.EditorSetGroupCamera(VRCamera);
            groupScript.EditorSetContentLoaders(loaders.ToArray());

            //create chair trigger
            ChairGroupTrigger trigger = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<ChairGroupTrigger>();
            trigger.GetComponent<MeshRenderer>().enabled = false;
            trigger.name = "Trigger";
            trigger.transform.SetParent(structure.transform);
            trigger.transform.localScale = Vector3.one;
            trigger.transform.localPosition = FindThePivot(chairTransforms.ToArray());

            //add canvas to trigger
            Canvas triggerCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goLockCanvasScaler = triggerCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goLockRaycaster = triggerCanvas.gameObject.AddComponent<GraphicRaycaster>();

            triggerCanvas.name = "Enter";
            triggerCanvas.transform.SetParent(trigger.transform);
            rectT = triggerCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(0.002f, 0.002f, 0.002f);
            rectT.localEulerAngles = new Vector3(0, 270, 0);
            rectT.anchoredPosition = new Vector2(0, 1.514f);
            rectT.sizeDelta = new Vector2(300, 100);

            triggerCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            triggerCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            triggerCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;

            TextMeshProUGUI triggerText = new GameObject().AddComponent<TextMeshProUGUI>();
            triggerText.color = new Color(1, 1, 1, 1);
            triggerText.name = "Text (TMP)";
            triggerText.transform.SetParent(triggerCanvas.transform);
            triggerText.transform.localPosition = new Vector3(0, 0, 0);
            triggerText.transform.localEulerAngles = new Vector3(0, 0, 0);
            triggerText.transform.localScale = new Vector3(1, 1, 1);
            triggerText.text = "Enter Meeting";
            rectT = triggerText.GetComponent<RectTransform>();
            rectT.anchorMin = new Vector2(0, 0.5f);
            rectT.anchorMax = new Vector2(1, 0.5f);
            rectT.offsetMax = new Vector2(0, 0);
            rectT.offsetMin = new Vector2(0, 0);
            rectT.sizeDelta = new Vector2(0, 50);

            AppConstReferences settings = Resources.Load<AppConstReferences>("AppConstReferences");
            FontTheme fontTheme = triggerText.gameObject.AddComponent<FontTheme>();
            ColorTheme colTheme = triggerText.gameObject.AddComponent<ColorTheme>();

            if (settings != null)
            {
                fontTheme.Apply(settings.Settings.themeSettings, 4);

                int index = 0;
                for (int i = 0; i < settings.Settings.themeSettings.colorThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.colorThemes[i].id.Equals("White"))
                    {
                        index = i;
                        break;
                    }
                }

                colTheme.Apply(settings.Settings.themeSettings, index);
            }

            triggerText.fontSize = 36;
            triggerText.alignment = TextAlignmentOptions.Center;

            Image triggerImage = new GameObject().AddComponent<Image>();
            triggerImage.color = new Color(1, 1, 1, 1);
            triggerImage.gameObject.GetComponent<CanvasRenderer>().cullTransparentMesh = false;
            triggerImage.name = "Image";
            triggerImage.transform.SetParent(triggerCanvas.transform);
            triggerImage.transform.localPosition = new Vector3(0, 0, 0);
            triggerImage.transform.localEulerAngles = new Vector3(0, 0, 0);
            triggerImage.transform.localScale = new Vector3(1, 1, 1);
            rectT = triggerImage.GetComponent<RectTransform>();

            UnityEngine.Object[] atlas = HUBUtils.GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_1.png");

            for (int i = 0; i < atlas.Length; i++)
            {
                if (atlas[i].name.Contains("IconEnterZone"))
                {
                    triggerImage.sprite = (Sprite)atlas[i];
                    break;
                }
            }

            rectT.sizeDelta = new Vector2(80, 80);
            rectT.anchoredPosition = new Vector2(0, -120f);
            trigger.gameObject.SetActive(false);
            groupScript.EditorSetTrigger(trigger.gameObject);



            //need to see if there is a door selected
            if (doorObject == null)
            {
                //create door
                prefab = (GameObject)HUBUtils.GetAsset<GameObject>("Assets/com.brandlab360.core/Samples/HDRP Sample/Models/Door.fbx");
                if (prefab != null)
                {
                    GameObject door = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    door.AddComponent<BoxCollider>();
                    door.name = "Door";
                    door.transform.SetParent(structure.transform);
                    door.transform.localEulerAngles = new Vector3(0, 0, 0);
                    door.transform.localScale = new Vector3(80, 80, 80);
                    door.transform.localPosition = new Vector3(1.1f, 2.45f, 0.0f);
                    groupScript.EditorSetDoor(door.AddComponent<Door>());
                }
            }
            else
            {
                if (doorObject != null)
                {
                    doorObject.transform.SetParent(structure.transform);

                    if (doorObject.GetComponent<Collider>() == null)
                    {
                        doorObject.AddComponent<BoxCollider>();
                    }

                    if (doorObject.GetComponent<Door>() == null)
                    {
                        groupScript.EditorSetDoor(doorObject.AddComponent<Door>());
                    }

                    //need to set the trigger by the door center position
                    trigger.transform.localPosition = doorObject.GetComponent<Renderer>().bounds.center;
                }
            }

            GameObject UI = new GameObject();
            UI.name = "UI";
            UI.transform.SetParent(structure.transform);
            UI.transform.localScale = Vector3.one;

            if (doorObject == null)
            {
                UI.transform.localPosition = new Vector3(1.1f, 1.5f, 0.6f);
            }
            else
            {
                //set UI based on door center position
                UI.transform.localPosition = doorObject.GetComponent<Renderer>().bounds.center;
            }

            groupScript.EditorSetUiObject(UI.transform);

            //create lock
            GameObject doorLock = CreateLockEditor.CreateLock();
            doorLock.transform.SetParent(UI.transform);
            doorLock.name = "Lock";

            atlas = HUBUtils.GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_2.png");

            for (int j = 0; j < atlas.Length; j++)
            {
                if (atlas[j].name.Contains("IconLock"))
                {
                    doorLock.transform.Find("Container_Viewport/Button_Lock/Icon_Lock").GetComponent<Image>().sprite = (Sprite)atlas[j];

                    break;
                }
            }

            doorLock.transform.localEulerAngles = new Vector3(0, 90, 0);
            doorLock.transform.localPosition = new Vector3(0, 0, 0);
            groupScript.EditorSetLock(doorLock.GetComponent<Lock>());


            //instruction
            Canvas instructionCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler instructionCanvasScaler = instructionCanvas.gameObject.AddComponent<CanvasScaler>();
            instructionCanvas.name = "Instruction";
            instructionCanvas.transform.SetParent(UI.transform);
            rectT = instructionCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localPosition = Vector3.zero;
            rectT.localScale = new Vector3(0.001f, 0.001f, 1f);
            rectT.localEulerAngles = new Vector3(0, 270, 0);
            rectT.localPosition = new Vector3(0, 0.268f, -0.23f);
            rectT.sizeDelta = new Vector2(0.25f, 0.25f);

            instructionCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            instructionCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            instructionCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;

            VerticalLayoutGroup vLayout = new GameObject().AddComponent<VerticalLayoutGroup>();
            vLayout.transform.SetParent(instructionCanvas.transform);
            vLayout.gameObject.name = "Layout";
            rectT = vLayout.GetComponent<RectTransform>();
            rectT.anchorMin = new Vector2(0, 1);
            rectT.anchorMax = new Vector2(0, 1);
            rectT.pivot = new Vector2(0, 1);
            rectT.localScale = new Vector3(1f, 1f, 1f);
            rectT.localEulerAngles = new Vector3(0, 0, 0);
            rectT.localPosition = new Vector3(0, 50f, 0.0f);

            vLayout.spacing = 10;
            vLayout.childAlignment = TextAnchor.UpperLeft;
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandWidth = false;
            vLayout.childForceExpandHeight = true;
            ContentSizeFitter sizeFitter = vLayout.gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Image image = new GameObject().AddComponent<Image>();
            image.name = "Label";
            image.transform.SetParent(vLayout.transform);
            image.sprite = Resources.Load<Sprite>("World/Square_Large");
            colTheme = image.gameObject.AddComponent<ColorTheme>();

            if (settings != null)
            {
                int index = 0;
                for (int i = 0; i < settings.Settings.themeSettings.colorThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.colorThemes[i].id.Equals("White"))
                    {
                        index = i;
                        break;
                    }
                }

                colTheme.Apply(settings.Settings.themeSettings, index);
            }


            vLayout = image.gameObject.AddComponent<VerticalLayoutGroup>();
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandHeight = true;
            vLayout.childForceExpandWidth = true;
            vLayout.childAlignment = TextAnchor.UpperLeft;
            vLayout.transform.localScale = new Vector3(1f, 1f, 1f);
            vLayout.transform.localEulerAngles = new Vector3(0f, 0, 0f);
            vLayout.transform.localPosition = new Vector3(0, 0, 0);
            //vLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 0);

            RectOffset rOffset = new RectOffset();
            rOffset.left = 25;
            rOffset.right = 25;
            rOffset.top = 0;
            rOffset.bottom = 0;
            vLayout.padding = rOffset;

            TextMeshProUGUI textScript = new GameObject().AddComponent<TextMeshProUGUI>();
            textScript.name = "Text (TMP)";
            textScript.transform.SetParent(image.transform);
            textScript.fontSize = 80;
            textScript.text = "Claim Room";
            textScript.color = Color.black;
            textScript.alignment = TextAlignmentOptions.Center;

            fontTheme = textScript.gameObject.AddComponent<FontTheme>();
            colTheme = textScript.gameObject.AddComponent<ColorTheme>();

            if (settings != null)
            {
                fontTheme.Apply(settings.Settings.themeSettings, 4);

                int index = 0;
                for (int i = 0; i < settings.Settings.themeSettings.colorThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.colorThemes[i].id.Equals("Black"))
                    {
                        index = i;
                        break;
                    }
                }

                colTheme.Apply(settings.Settings.themeSettings, index);
            }

            textScript.transform.localScale = new Vector3(1, 1, 1);
            textScript.transform.localEulerAngles = new Vector3(0f, 0, 0f);
            textScript.transform.localPosition = new Vector3(0, 0, 0);

            groupScript.EditorSetInstruction(textScript);


            //availabilty
            Canvas availabilityCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler availabilityCanvasScaler = availabilityCanvas.gameObject.AddComponent<CanvasScaler>();
            availabilityCanvas.name = "Availability";
            availabilityCanvas.transform.SetParent(UI.transform);
            rectT = availabilityCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(0.001f, 0.001f, 1f);
            rectT.localEulerAngles = new Vector3(0, 270, 0);
            rectT.localPosition = new Vector3(0, -0.05f, 0.03f);
            rectT.sizeDelta = new Vector2(0.25f, 0.25f);

            availabilityCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            availabilityCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            availabilityCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;

            vLayout = new GameObject().AddComponent<VerticalLayoutGroup>();
            vLayout.transform.SetParent(availabilityCanvas.transform);
            vLayout.gameObject.name = "Layout";
            rectT = vLayout.GetComponent<RectTransform>();
            rectT.anchorMin = new Vector2(0, 1);
            rectT.anchorMax = new Vector2(0, 1);
            rectT.pivot = new Vector2(0, 1);
            rectT.localScale = new Vector3(1f, 1f, 1f);
            rectT.localEulerAngles = new Vector3(0, 0, 0);
            rectT.localPosition = new Vector3(122, 178f, 0.0f);

            vLayout.spacing = 10;
            vLayout.childAlignment = TextAnchor.UpperLeft;
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandWidth = false;
            vLayout.childForceExpandHeight = true;
            sizeFitter = vLayout.gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


            image = new GameObject().AddComponent<Image>();
            image.name = "Label";
            image.transform.SetParent(vLayout.transform);
            image.sprite = Resources.Load<Sprite>("World/Square_Large");

            colTheme = image.gameObject.AddComponent<ColorTheme>();

            if (settings != null)
            {
                int index = 0;
                for (int i = 0; i < settings.Settings.themeSettings.colorThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.colorThemes[i].id.Equals("White"))
                    {
                        index = i;
                        break;
                    }
                }

                colTheme.Apply(settings.Settings.themeSettings, index);
            }

            vLayout = image.gameObject.AddComponent<VerticalLayoutGroup>();
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandHeight = true;
            vLayout.childForceExpandWidth = true;
            vLayout.childAlignment = TextAnchor.UpperLeft;
            vLayout.transform.localScale = new Vector3(1f, 1f, 1f);
            vLayout.transform.localEulerAngles = new Vector3(0f, 0, 0f);
            vLayout.transform.localPosition = new Vector3(0, 0, 0);
            //vLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 0);

            rOffset = new RectOffset();
            rOffset.left = 25;
            rOffset.right = 25;
            rOffset.top =0;
            rOffset.bottom = 0;
            vLayout.padding = rOffset;

            textScript = new GameObject().AddComponent<TextMeshProUGUI>();
            textScript.name = "Text (TMP)";
            textScript.transform.SetParent(vLayout.transform);
            textScript.fontSize = 80;
            textScript.text = "Available";
            textScript.color = Color.black;

            fontTheme = textScript.gameObject.AddComponent<FontTheme>();
            colTheme = textScript.gameObject.AddComponent<ColorTheme>();

            if (settings != null)
            {
                fontTheme.Apply(settings.Settings.themeSettings, 4);

                int index = 0;
                for (int i = 0; i < settings.Settings.themeSettings.colorThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.colorThemes[i].id.Equals("Black"))
                    {
                        index = i;
                        break;
                    }
                }

                colTheme.Apply(settings.Settings.themeSettings, index);
            }

            textScript.alignment = TextAlignmentOptions.Center;
            textScript.transform.localScale = new Vector3(1, 1, 1);
            textScript.transform.localEulerAngles = new Vector3(0f, 0, 0f);
            textScript.transform.localPosition = new Vector3(0, 0, 0);


            LayoutElement layoutEle = new GameObject().AddComponent<LayoutElement>();
            layoutEle.name = "Indicator";
            layoutEle.transform.SetParent(vLayout.transform.parent);
            layoutEle.minWidth = 100;
            layoutEle.minHeight = 100;
            layoutEle.preferredHeight = 100;
            layoutEle.preferredWidth = 100;
            rectT = layoutEle.GetComponent<RectTransform>();
            rectT.localPosition = new Vector3(0f, 0f, 0f);
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.one;
            rectT.pivot = new Vector2(0.5f, 0.5f);
            rectT.localScale = new Vector3(1f, 1f, 1f);
            rectT.localEulerAngles = new Vector3(0f, 0f, 0f);


            Image cImage = new GameObject().AddComponent<Image>();
            cImage.color = new Color(1, 1, 1, 0.9f);
            cImage.name = "Icon_Availability";
            cImage.transform.SetParent(layoutEle.transform);
            rectT = cImage.GetComponent<RectTransform>();
            rectT.localPosition = new Vector3(0f, 0f, 0f);
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.one;
            rectT.pivot = new Vector2(0.5f, 0.5f);
            rectT.offsetMax = new Vector2(0f, 0f);
            rectT.offsetMin = new Vector2(0f, 0f);
            rectT.localScale = new Vector3(1f, 1f, 1f);
            rectT.localEulerAngles = new Vector3(0f, 0f, 0f);

            cImage.sprite = Resources.Load<Sprite>("World/Square_Large");

            groupScript.EditorSetAvailablity(cImage);
            groupScript.EditorSetTrigger(trigger.gameObject);

            HUBUtils.CreateParentAndSelectObject("_CHAIRGROUPS", chairGroup.transform, false);
        }

        [MenuItem("GameObject/BrandLab360/Create Chair Group/Conference", false)]
        protected static void ValidateCreateConference(MenuCommand command)
        {
            if (UnityEditor.Selection.objects.Length <= 0)
            {
                return;
            }

            if (command.context == UnityEditor.Selection.objects[0])
            {
                doorObject = null;

                List<ChairObject> chairs = new List<ChairObject>();

                for (int i = 0; i < UnityEditor.Selection.objects.Length; i++)
                {
                    if (UnityEditor.Selection.objects[i].name.ToLower().Contains("door"))
                    {
                        doorObject = (GameObject)UnityEditor.Selection.objects[i];
                        continue;
                    }

                    ChairObject obj = new ChairObject();
                    obj.chair = (GameObject)UnityEditor.Selection.objects[i];
                    obj.create = false;

                    chairs.Add(obj);
                }

                UnityEditor.Selection.objects = new Object[0];
                Selection.activeTransform = null;

                ConferenceCallback("", chairs);
            }
        }
    }
}
