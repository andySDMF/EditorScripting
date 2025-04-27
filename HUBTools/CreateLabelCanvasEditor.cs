using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace BrandLab360.Editor
{
    public class CreateLabelCanvasEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Labels/Label")]
        public static void Create2DLabel()
        {
            RectTransform rectT = null;

            Canvas goCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goLockCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goLockRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();

            goCanvas.name = "LabelCanvas_";
            rectT = goCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(1f, 1f, 1f);

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
            rOffset.left = 0;
            rOffset.right = 0;
            rOffset.top = 0;
            rOffset.bottom = 0;

            vLayout.padding = rOffset;

            ContentSizeFitter csFitter = goCanvas.gameObject.AddComponent<ContentSizeFitter>();
            csFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            csFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            TextMeshProUGUI textScript = new GameObject().AddComponent<TextMeshProUGUI>();
            textScript.name = "Text (TMP)";
            textScript.text = "Label";
            textScript.transform.SetParent(csFitter.transform);
            textScript.fontSize = 1;
            textScript.alignment = TextAlignmentOptions.Center;
            textScript.transform.localScale = Vector3.one;

            AppConstReferences settings = Resources.Load<AppConstReferences>("AppConstReferences");
            FontTheme fontTheme = textScript.gameObject.AddComponent<FontTheme>();
            ColorTheme colTheme = textScript.gameObject.AddComponent<ColorTheme>();

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

            CreateParentAndSelectObject("_LABELS", csFitter.transform);
        }

        public static void Create3DLabel()
        {

        }
    }
}
