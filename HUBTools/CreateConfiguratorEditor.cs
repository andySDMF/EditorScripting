using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateConfiguratorEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Configurator/Color")]
        public static void CreateConfigColor()
        {
            GameObject config = CreateBaseObject("Color");
            Configurator script = config.AddComponent<Configurator>();
            script.Type = ConfiguratorManager.ConfiguratorType.Color;

            CreateParentAndSelectObject("_CONFIGURATORS", config.transform);
        }

       // [MenuItem("BrandLab360/Core/Configurator/Materials")]
        public static void CreateConfigMaterials()
        {
            GameObject config = CreateBaseObject("Material");
            Configurator script = config.AddComponent<Configurator>();
            script.Type = ConfiguratorManager.ConfiguratorType.Material;

            CreateParentAndSelectObject("_CONFIGURATORS", config.transform);
        }

       // [MenuItem("BrandLab360/Core/Configurator/Model")]
        public static void CreateConfigModel()
        {
            GameObject config = CreateBaseObject("Model");
            Configurator script = config.AddComponent<Configurator>();
            script.Type = ConfiguratorManager.ConfiguratorType.Model;

            CreateParentAndSelectObject("_CONFIGURATORS", config.transform);
        }

       // [MenuItem("BrandLab360/Core/Configurator/Transform")]
        public static void CreateConfigTransform()
        {
            GameObject config = CreateBaseObject("Transform");
            Configurator script = config.AddComponent<Configurator>();
            script.Type = ConfiguratorManager.ConfiguratorType.Transform;

            CreateParentAndSelectObject("_CONFIGURATORS", config.transform);
        }

        private static GameObject CreateBaseObject(string id)
        {
            RectTransform rectT = null;
            Canvas goCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goLockCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goLockRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();

            goCanvas.name = "Config_" + id;
            rectT = goCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(0.0004526008f, 0.0004526008f, 0.0004526008f);
            goLockRaycaster.ignoreReversedGraphics = false;

            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;


            return goCanvas.gameObject;
        }
    }
}
