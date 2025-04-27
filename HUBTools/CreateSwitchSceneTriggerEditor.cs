using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateSwitchSceneTriggerEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Switch Scene/Trigger")]
        public static void CreateTrigger()
        {
            SwitchSceneTrigger trigger = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<SwitchSceneTrigger>();
            trigger.name = "SwitchSceneTrigger_";
            trigger.transform.position = Vector3.zero;
            trigger.transform.localScale = Vector3.one;
            trigger.GetComponent<Renderer>().material = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");
            trigger.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            CreateParentAndSelectObject("_SWITCHSCENES", trigger.transform);
        }

      //  [MenuItem("BrandLab360/Core/Switch Scene/Button")]
        public static void CreateButton()
        {
            RectTransform rectT = null;

            Canvas goCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();
            goCanvas.gameObject.AddComponent<BoxCollider>().size = new Vector3(0.25f, 0.25f, 0.025f);
            goRaycaster.ignoreReversedGraphics = false;

            goCanvas.name = "CanvasSwitchScene_";
            rectT = goCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(5f, 5f, 0.25f);
            rectT.localPosition = Vector3.zero;
            rectT.sizeDelta = new Vector2(0.25f, 0.25f);

            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;

            SwitchSceneTrigger triggerScript = goCanvas.gameObject.AddComponent<SwitchSceneTrigger>();

            Image goImage = new GameObject().AddComponent<Image>();
            goImage.name = "Button_SwitchScene";
            goImage.GetComponent<CanvasRenderer>().cullTransparentMesh = false;

            goImage.transform.SetParent(goCanvas.transform);
            rectT = goImage.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.one;
            rectT.pivot = new Vector2(0.5f, 0.5f);
            rectT.offsetMax = new Vector2(0, 0);
            rectT.offsetMin = new Vector2(0, 0);
            rectT.localScale = new Vector3(1f, 1f, 1f);
            rectT.localEulerAngles = Vector3.zero;

            goImage.color = Color.white;

            Button but = goCanvas.gameObject.AddComponent<Button>();
            but.targetGraphic = goImage;
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            but.navigation = nav;

            Image buttonImage = new GameObject().AddComponent<Image>();
            buttonImage.color = new Color(0, 0, 0, 1);

            UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_1.png");

            for (int i = 0; i < atlas.Length; i++)
            {
                if (atlas[i].name.Contains("IconEnterZone"))
                {
                    buttonImage.sprite = (Sprite)atlas[i];

                    break;
                }
            }

            buttonImage.name = "Icon_Enter";
            buttonImage.transform.SetParent(goImage.transform);
            rectT = buttonImage.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.one;
            rectT.offsetMax = new Vector2(0, 0);
            rectT.offsetMin = new Vector2(0, 0);
            rectT.localScale = Vector3.one;
            rectT.localEulerAngles = Vector3.zero;

            AppConstReferences settings = Resources.Load<AppConstReferences>("AppConstReferences");
            ButtonTheme theme = but.gameObject.AddComponent<ButtonTheme>();

            if (settings != null)
            {
                int index = 0;

                for (int i = 0; i < settings.Settings.themeSettings.buttonThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.buttonThemes[i].id.Equals("White"))
                    {
                        index = i;
                        break;
                    }
                }

                theme.Apply(settings.Settings.themeSettings, index);
            }

            ButtonAppearance bApperance = but.gameObject.AddComponent<ButtonAppearance>();
            bApperance.Apply(ButtonAppearance.Appearance._Round);

            CreateParentAndSelectObject("_SWITCHSCENES", goCanvas.transform);
        }
    }
}
