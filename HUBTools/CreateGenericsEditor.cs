using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BrandLab360.Editor
{
    public class CreateGenericsEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/Generics/Triggers/Interactive")]
        public static void CreateInteractiveTrigger()
        {
            Material mat = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");

            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "InteractiveTrigger_";
            trigger.AddComponent<InteractiveTrigger>();

            if (mat != null)
            {
                trigger.GetComponent<Renderer>().material = mat;
            }

            CreateParentAndSelectObject("_TRIGGERS", trigger.transform);
            SetPosition(trigger.transform);
        }

       // [MenuItem("BrandLab360/Core/Generics/Triggers/Hint")]
        public static void CreateHintTrigger()
        {
            Material mat = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");

            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "HintTrigger_";
            trigger.AddComponent<HintTrigger>();

            if (mat != null)
            {
                trigger.GetComponent<Renderer>().material = mat;
            }

            CreateParentAndSelectObject("_TRIGGERS", trigger.transform);
            SetPosition(trigger.transform);
        }

        //[MenuItem("BrandLab360/Core/Generics/Triggers/Popup")]
        public static void CreatePopupTrigger()
        {
            Material mat = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");

            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "PopUpTrigger_";
            trigger.AddComponent<PopUpTrigger>();

            if (mat != null)
            {
                trigger.GetComponent<Renderer>().material = mat;
            }

            CreateParentAndSelectObject("_TRIGGERS", trigger.transform);

            SetPosition(trigger.transform);
        }

        //[MenuItem("BrandLab360/Core/Generics/Triggers/Audio")]
        public static void CreateAudioTrigger()
        {
            Material mat = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/Trigger.mat");

            GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trigger.name = "AudioTrigger_";
            trigger.AddComponent<AudioTrigger>();

            if (mat != null)
            {
                trigger.GetComponent<Renderer>().material = mat;
            }

            CreateParentAndSelectObject("_TRIGGERS", trigger.transform);
            SetPosition(trigger.transform);
        }


        //[MenuItem("BrandLab360/Core/Generics/Music")]
        public static void CreateMusic()
        {
            AudioSource audio = new GameObject().AddComponent<AudioSource>();
            audio.name = "Music_";
            audio.playOnAwake = false;
            audio.loop = true;
            audio.volume = 0.0f;

            audio.gameObject.AddComponent<Music>();

            CreateParentAndSelectObject("_MUSIC", audio.transform);
            SetPosition(audio.transform);
        }

        //[MenuItem("BrandLab360/Core/Generics/ChromaKey Video")]
        public static void CreateChromaKeyVideo()
        {
            Material mat = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/ChromaKey.mat");

            GameObject video = GameObject.CreatePrimitive(PrimitiveType.Plane);
            video.name = "3DChromaKeyVideo_";
            video.AddComponent<MaterialVideo>();

            if (mat != null)
            {
                video.GetComponent<Renderer>().material = mat;
            }

            video.transform.localEulerAngles = new Vector3(90, 0, 180);
            video.transform.localScale = new Vector3(0.192f, 1, 0.108f);

            AudioSource m_audio = video.GetComponent<AudioSource>();
            m_audio.playOnAwake = false;

            VideoPlayer m_video = video.GetComponent<VideoPlayer>();
            m_video.playOnAwake = false;
            m_video.source = VideoSource.VideoClip;
            m_video.renderMode = VideoRenderMode.MaterialOverride;
            m_video.targetMaterialRenderer = video.GetComponent<Renderer>();

            m_video.audioOutputMode = VideoAudioOutputMode.AudioSource;
            m_video.EnableAudioTrack(0, true);
            m_video.SetTargetAudioSource(0, m_audio);

            CreateParentAndSelectObject("_3DMATERIALVIDEOS", video.transform);

            SetPosition(video.transform);
        }

        //[MenuItem("BrandLab360/Core/Generics/Button/Grab")]
        public static void Create3DGrabButton()
        {
            Create3DButton("IconGrab");
        }

        //[MenuItem("BrandLab360/Core/Generics/Button/Information")]
        public static void Create3DInformationButton()
        {
            Create3DButton("IconInformation");
        }

       // [MenuItem("BrandLab360/Core/Generics/Button/Interact")]
        public static void Create3DInteractButton()
        {
            Create3DButton("IconInteract");
        }

       // [MenuItem("BrandLab360/Core/Generics/Arrow/Straight")]
        public static void CreateStraightArrow()
        {
            UnityEngine.Object arrow = (UnityEngine.Object)GetAsset<UnityEngine.Object>("Assets/com.brandlab360.core/Runtime/Models/arrows/Arrow_straight.fbx");

            if(arrow != null)
            {
                GameObject go = (GameObject)Instantiate(arrow);
                SetPosition(go.transform);
            }
        }

        //[MenuItem("BrandLab360/Core/Generics/Arrow/Bent")]
        public static void CreateBentArrow()
        {
            UnityEngine.Object arrow = (UnityEngine.Object)GetAsset<UnityEngine.Object>("Assets/com.brandlab360.core/Runtime/Models/arrows/Arrow_bent.fbx");

            if (arrow != null)
            {
                GameObject go = (GameObject)Instantiate(arrow);
                SetPosition(go.transform);
            }
        }


        public static void Create3DButton(string icon)
        {
            RectTransform rectT = null;

            Canvas goCanvas = new GameObject().AddComponent<Canvas>();
            CanvasScaler goCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
            GraphicRaycaster goRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();
            goRaycaster.ignoreReversedGraphics = false;
            goCanvas.gameObject.layer = LayerMask.NameToLayer("Default");
     

            goCanvas.name = "3DButton_";
            rectT = goCanvas.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.zero;
            rectT.localScale = new Vector3(1f, 1f, 1);
            rectT.sizeDelta = new Vector2(0.25f, 0.25f);

            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            goCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;

            Image goImage = goCanvas.gameObject.AddComponent<Image>();
            goImage.GetComponent<CanvasRenderer>().cullTransparentMesh = false;


            goImage.color = Color.white;

            Button but = goCanvas.gameObject.AddComponent<Button>();
            but.targetGraphic = goImage;
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            but.navigation = nav;

            Image goIcon = new GameObject().AddComponent<Image>();
            goIcon.gameObject.name = "Image_Icon";

            UnityEngine.Object[] atlas = GetAssets("Assets/com.brandlab360.core/Runtime/Sprites/World/Wolrd_Icons_1.png");

            for (int i = 0; i < atlas.Length; i++)
            {
                if (atlas[i].name.Contains(icon))
                {
                    goIcon.sprite = (Sprite)atlas[i];

                    break;
                }
            }

            goIcon.transform.SetParent(rectT);
            goIcon.transform.localPosition = Vector3.zero;
            goIcon.transform.localScale = Vector3.one;
            goIcon.transform.localEulerAngles = Vector3.zero;
            rectT = goIcon.GetComponent<RectTransform>();
            rectT.anchorMin = Vector2.zero;
            rectT.anchorMax = Vector2.one;
            rectT.pivot = new Vector2(0.5f, 0.5f);
            rectT.offsetMax = new Vector2(-0.05f, -0.05f);
            rectT.offsetMin = new Vector2(0.05f, 0.05f);
            rectT.localScale = Vector3.one;

            AppConstReferences settings = Resources.Load<AppConstReferences>("AppConstReferences");
            ButtonTheme theme = goCanvas.gameObject.AddComponent<ButtonTheme>();

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

            CreateParentAndSelectObject("_3DBUTTONS", goCanvas.transform);

            SetPosition(goCanvas.transform);
        }

        //[MenuItem("BrandLab360/Core/Generics/3D Material Video")]
        public static void Create3DMaterialVideo()
        {
            Material mat = (Material)GetAsset<Material>("Assets/com.brandlab360.core/Runtime/Materials/VideoMaterial.mat");

            GameObject video = GameObject.CreatePrimitive(PrimitiveType.Plane);
            video.name = "3DMaterialVideo_";
            video.AddComponent<MaterialVideo>();

            if (mat != null)
            {
                video.GetComponent<Renderer>().material = mat;
            }

            video.transform.localEulerAngles = new Vector3(90, 0, 180);
            video.transform.localScale = new Vector3(0.192f, 1, 0.108f);

            AudioSource m_audio = video.GetComponent<AudioSource>();
            m_audio.playOnAwake = false;

            VideoPlayer m_video = video.GetComponent<VideoPlayer>();
            m_video.playOnAwake = false;
            m_video.source = VideoSource.VideoClip;
            m_video.renderMode = VideoRenderMode.MaterialOverride;
            m_video.targetMaterialRenderer = video.GetComponent<Renderer>();

            m_video.audioOutputMode = VideoAudioOutputMode.AudioSource;
            m_video.EnableAudioTrack(0, true);
            m_video.SetTargetAudioSource(0, m_audio);

            CreateParentAndSelectObject("_3DMATERIALVIDEOS", video.transform);

            SetPosition(video.transform);
        }

        private static void SetPosition(Transform t)
        {
            Camera view = UnityEditor.SceneView.lastActiveSceneView.camera;
            Vector3 pos = view.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            t.position = pos;

            SceneView es = UnityEditor.SceneView.lastActiveSceneView;
            es.AlignViewToObject(t);
        }
    }
}
