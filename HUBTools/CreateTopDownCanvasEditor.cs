using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace BrandLab360.Editor
{
    public class CreateTopDownCanvasEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Map/Topdown Canvas")]
        public static void Create()
        {
            RectTransform rectT = null;

            Canvas goCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goLockCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goLockRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();
            goCanvas.gameObject.layer = LayerMask.NameToLayer("TopDownVisible");

            goCanvas.name = "TopDownCanvas_";
            rectT = goCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;

            VerticalLayoutGroup vLayout = goCanvas.gameObject.AddComponent<VerticalLayoutGroup>();
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandHeight = true;
            vLayout.childForceExpandWidth = true;
            vLayout.childAlignment = TextAnchor.MiddleCenter;

            RectOffset rOffset = new RectOffset();
            rOffset.left = 20;
            rOffset.right = 20;
            rOffset.top = 20;
            rOffset.bottom = 20;

            vLayout.padding = rOffset;

            ContentSizeFitter csFitter = goCanvas.gameObject.AddComponent<ContentSizeFitter>();
            csFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            csFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Image goImage = goCanvas.gameObject.AddComponent<Image>();
            goImage.GetComponent<CanvasRenderer>().cullTransparentMesh = false;

            goImage.sprite = Resources.Load<Sprite>("World/Square");

            AppConstReferences settings = Resources.Load<AppConstReferences>("AppConstReferences");
            ColorTheme colTheme = goImage.gameObject.AddComponent<ColorTheme>();

            if (settings != null)
            {
                int index = 0;
                for (int i = 0; i < settings.Settings.themeSettings.colorThemes.Count; i++)
                {
                    if (settings.Settings.themeSettings.colorThemes[i].id.Equals("Background"))
                    {
                        index = i;
                        break;
                    }
                }

                colTheme.Apply(settings.Settings.themeSettings, index);
            }

            TextMeshProUGUI textScript = new GameObject().AddComponent<TextMeshProUGUI>();
            textScript.name = "Text (TMP)";
            textScript.text = "TopDown";
            textScript.transform.SetParent(csFitter.transform);
            textScript.fontSize = 20;
            textScript.alignment = TextAlignmentOptions.Center;
            textScript.gameObject.layer = LayerMask.NameToLayer("TopDownVisible");
            textScript.transform.localScale = Vector3.one;

            FontTheme fontTheme = textScript.gameObject.AddComponent<FontTheme>();
            colTheme = textScript.gameObject.AddComponent<ColorTheme>();

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

            goCanvas.transform.localEulerAngles = new Vector3(90, 0, 0);
            goCanvas.transform.localPosition = new Vector3(0, 5, 0);
            goCanvas.gameObject.AddComponent<MapLabel>();

            CreateParentAndSelectObject("_TOPDOWNLABELS", goCanvas.transform);
            goCanvas.transform.localPosition = new Vector3(0, 5, 0);
        }

       // [MenuItem("BrandLab360/Core/Map/Key Object")]
        public static void CreateKey()
        {
            RectTransform rectT = null;

            Canvas goCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goLockCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goLockRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();
            goCanvas.gameObject.layer = LayerMask.NameToLayer("TopDownVisible");

            goCanvas.name = "TopDownCanvas_";
            rectT = goCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(0.00527f, 0.00527f, 0.00527f);

            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;

            VerticalLayoutGroup vLayout = goCanvas.gameObject.AddComponent<VerticalLayoutGroup>();
            vLayout.childControlHeight = true;
            vLayout.childControlWidth = true;
            vLayout.childForceExpandHeight = true;
            vLayout.childForceExpandWidth = true;
            vLayout.childAlignment = TextAnchor.UpperLeft;

            RectOffset rOffset = new RectOffset();
            rOffset.left = 0;
            rOffset.right = 0;
            rOffset.top = 0;
            rOffset.bottom = 0;

            vLayout.padding = rOffset;

            ContentSizeFitter csFitter = vLayout.gameObject.AddComponent<ContentSizeFitter>();
            csFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            csFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Image img = new GameObject().AddComponent<Image>();
            img.transform.SetParent(csFitter.transform);
            img.gameObject.layer = LayerMask.NameToLayer("TopDownVisible");
            img.gameObject.name = "Image_Key";
            img.transform.localScale = Vector3.one;

            goCanvas.transform.localEulerAngles = new Vector3(90, 0, 0);
            goCanvas.gameObject.AddComponent<MapLabel>();
            goCanvas.transform.position = new Vector3(0, 3, 0);
            SelectObject(csFitter.transform);
        }
    }
}
