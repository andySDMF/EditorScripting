using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace BrandLab360.Editor
{
    [InitializeOnLoad]
    public class HUBWindow : EditorWindow
    {
        private static HUBWindow m_mainWindow;
        private Texture2D m_header;
        private Vector2 m_sideBarScroll;
        private static Vector2 m_providerScroll;
        private static HUBTab m_currentTab = HUBTab.Settings;
        private Texture2D m_copyright;
        private float m_domainArea = 170;
        public static string _COREPREFAB = "_BRANDLAB360_CORE";

        private static List<ProviderDrawer> m_providers = new List<ProviderDrawer>();
        private static HUBProvider m_currentProvider;

        private static bool m_hasInit = false;

        private GUIStyle m_HeaderFont;
        private GUIStyle m_DomainFont;
        private GUIStyle m_StandardFont;

        private GUIStyle m_DomainSelected;
        private GUIStyle m_DomainNormal;

        private GUIStyle m_TabSelected;
        private GUIStyle m_TabNormal;

        private GUIStyle m_Documentation;
        private GUIStyle m_Copyright;
        private GUIStyle m_mini;
        private GUIStyle m_hyperlink;

        private const string _BLANDLAB360WEBITE = "www.brandlab-360.com";
        private const string _DOCUMENTATION = "www.brandlab-360.com/Documentation/ControlPanel";

        public static AppSettings _APPSETTINGS;
        public static AppInstances _APPINSTANCES;
        public static SerializedObject _SOSETTINGS;
        public static SerializedObject _SOINSTANCES;
        public static int _LABELWIDTH = 250;

        private static bool IsOpen = false;

        public static System.Action OnWindowClose { get; set; }

        public static Color SelectedColor { get; private set; }

        public static bool IsBuilding { get; set; }

        [MenuItem("BrandLab360/Control Panel")]
        private static void Open()
        {
            _APPSETTINGS = null;
            _APPINSTANCES = null;
            _SOSETTINGS = null;
            _SOINSTANCES = null;

            m_mainWindow = (HUBWindow)GetWindow(typeof(HUBWindow));
            m_mainWindow.maxSize = new Vector2(1024f, 800f);
            m_mainWindow.minSize = m_mainWindow.maxSize;
            m_mainWindow.titleContent = new GUIContent("Control Panel");
            m_mainWindow.Show();

            

            m_hasInit = false;
            IsOpen = true;
            IsBuilding = false;
        }

        static HUBWindow()
        {
            m_providers.Clear();

            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            var providers = new List<ProviderDrawer>();

            foreach (var assembly in assemblies)
            {
                var methods = assembly.GetTypes().SelectMany(t => t.GetMethods())
                                     .Where(m => m.GetCustomAttributes(typeof(HUBSettingsProviderAttribute), false).Length > 0)
                                     .ToArray();

                for (int i = 0; i < methods.Length; i++)
                {
                    HUBProvider provider = (HUBProvider)methods[i].Invoke(null, null);
                    ProviderDrawer drawer = new ProviderDrawer();
                    drawer.domain = provider.Domain;
                    drawer.label = provider.label;
                    drawer.order = provider.Order;
                    drawer.method = methods[i];
                    drawer.tab = provider.Tab;

                    providers.Add(drawer);
                }
            }

            m_providers = providers.OrderBy(x => x.order).ToList();

            m_hasInit = false;
        }

        private void OnLostFocus()
        {
            if (_APPSETTINGS != null)
            {
                EditorUtility.SetDirty(_APPSETTINGS);
            }

            if (_APPINSTANCES != null)
            {
                EditorUtility.SetDirty(_APPINSTANCES);
            }

            if (_SOSETTINGS != null)
            {
                _SOSETTINGS.ApplyModifiedProperties();
            }

            if (_APPINSTANCES != null)
            {
                _SOINSTANCES.ApplyModifiedProperties();
            }
        }

        private void OnDestroy()
        {
            if (_APPSETTINGS != null)
            {
                EditorUtility.SetDirty(_APPSETTINGS);
            }

            if (_APPINSTANCES != null)
            {
                EditorUtility.SetDirty(_APPINSTANCES);
            }

            if (_SOSETTINGS != null)
            {
                _SOSETTINGS.ApplyModifiedProperties();
            }

            if (_APPINSTANCES != null)
            {
                _SOINSTANCES.ApplyModifiedProperties();
            }

            IsOpen = false;

            if(OnWindowClose != null)
            {
                OnWindowClose.Invoke();
            }

            OnWindowClose = null;
        }

        private void OnEnable()
        {
            IsBuilding = false;

            Sprite sp = (Sprite)HUBUtils.GetAsset<Sprite>("Assets/com.brandlab360.core/Editor/Sprites/BrandLab360.png");
            if (sp != null)
            {
                m_header = sp.texture;
            }

            sp = (Sprite)HUBUtils.GetAsset<Sprite>("Assets/com.brandlab360.core/Editor/Sprites/copyright.png");

            if(sp != null)
            {
                m_copyright = sp.texture;
            }
        }

        private void OnDisable()
        {
            if(m_currentProvider != null)
            {
                m_currentProvider.OnDeactivate();
            }

            PlayerPrefs.SetString("HUBWindowTAB", m_currentTab.ToString());
            PlayerPrefs.SetString("HUBWindowDOMAIN", m_currentProvider !=  null ? m_currentProvider.label : "");
            PlayerPrefs.Save();
        }

        private void OnGUI()
        {
            if(!m_hasInit)
            {
                m_hasInit = true;
                Initialse();
            }

            if(_SOSETTINGS != null)
            {
                _SOSETTINGS.Update();
            }

            if(_SOINSTANCES != null)
            {
                _SOINSTANCES.Update();
            }

            DrawHeader();
            DrawTabs();
            EditorGUILayout.BeginHorizontal();
            DrawDomains();
            EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.ExpandHeight(true), GUILayout.Width(2));
            DrawProvider();

            if(!IsBuilding)
            {
                EditorGUILayout.EndHorizontal();
                DrawFooter();
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }


            if(this != null)
            {
                EditorUtility.SetDirty(this);
            }

            if (_APPSETTINGS != null)
            {
                EditorUtility.SetDirty(_APPSETTINGS);

            }

            if (_APPINSTANCES != null)
            {
                EditorUtility.SetDirty(_APPINSTANCES);

            }

            if (_SOSETTINGS != null)
            {
                _SOSETTINGS.ApplyModifiedProperties();
            }

            if (_APPINSTANCES != null)
            {
                _SOINSTANCES.ApplyModifiedProperties();
            }
        }

        private void Initialse()
        {
            AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

            if (appReferences != null)
            {
                _APPSETTINGS = appReferences.Settings;
                _APPINSTANCES = appReferences.Instances;
            }
            else
            {
                _APPSETTINGS = Resources.Load<AppSettings>("ProjectAppSettings");
                _APPINSTANCES = Resources.Load<AppInstances>("ProjectAppInstances");
            }

            if(_APPSETTINGS != null)
            {
                _SOSETTINGS = new SerializedObject(_APPSETTINGS);

                SetDefaults();
            }

            if (_APPINSTANCES != null)
            {
                _SOINSTANCES = new SerializedObject(_APPINSTANCES);
            }

            SelectedColor = new Color32(1, 51, 69, 255);


            m_Copyright = new GUIStyle();
            m_Copyright.alignment = TextAnchor.MiddleLeft;

            m_HeaderFont = HUBGUIStyle.H1;

            m_DomainFont = HUBGUIStyle.H2;

            m_StandardFont = HUBGUIStyle.H3;
            m_StandardFont.alignment = TextAnchor.MiddleLeft;

            m_DomainNormal = HUBGUIStyle.DomainNormal;
            m_DomainNormal.fontSize = 15;
            m_DomainNormal.alignment = TextAnchor.MiddleLeft;
            m_DomainNormal.normal.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));
            m_DomainNormal.hover.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(255, 255, 255, 100));

            m_DomainSelected = HUBGUIStyle.DomainSelected;
            m_DomainSelected.fontSize = 15;
            m_DomainSelected.alignment = TextAnchor.MiddleLeft;
            m_DomainSelected.normal.background = HUBGUIStyle.MakeTexture(1, 1, SelectedColor);

            m_TabNormal = HUBGUIStyle.TabNormal;
            m_TabNormal.fontSize = 15;
            m_TabNormal.normal.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));
            m_TabNormal.hover.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(255, 255, 255, 100));

            m_TabSelected = HUBGUIStyle.TabSelected;
            m_TabSelected.fontSize = 15;
            m_TabSelected.normal.background = HUBGUIStyle.MakeTexture(1, 1, SelectedColor);

            m_Documentation = HUBGUIStyle.ButtonStandard;
            m_Documentation.fixedHeight = 25;
            m_Documentation.normal.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));
            m_Documentation.hover.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(255, 255, 255, 100));

            m_hyperlink = HUBGUIStyle.ButtonHyperlink;
            m_hyperlink.fixedHeight = 25;
            m_hyperlink.alignment = TextAnchor.MiddleLeft;
            m_hyperlink.normal.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));
            m_hyperlink.hover.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));
            m_hyperlink.active.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));

            m_mini = HUBGUIStyle.ButtonMini;
            m_mini.normal.background = HUBGUIStyle.MakeTexture(1, 1, new Color32(45, 45, 45, 255));

            if (m_providers.Count > 0)
            {
                string playerPrefsTab = PlayerPrefs.GetString("HUBWindowTAB");

                if(!string.IsNullOrEmpty(playerPrefsTab))
                {
                    m_currentTab = (HUBTab)System.Enum.Parse(typeof(HUBTab), playerPrefsTab);
                }

                string playerPrefsDomain = PlayerPrefs.GetString("HUBWindowDOMAIN");

                if (!string.IsNullOrEmpty(playerPrefsTab))
                {
                    ProviderDrawer drawer = m_providers.FirstOrDefault(x => x.label.Equals(playerPrefsDomain) && x.tab.Equals(m_currentTab));

                    if(drawer != null)
                    {
                        m_currentProvider = (HUBProvider)drawer.method.Invoke(null, null);
                        m_currentProvider.OnActivate("", null);
                    }
                    
                }
                else
                {
                    m_currentProvider = (HUBProvider)m_providers[0].method.Invoke(null, null);
                    m_currentProvider.OnActivate("", null);
                }
            }
        }

        public static bool DrawSettings()
        {
            if(Application.isPlaying)
            {
                EditorGUILayout.LabelField("Setting unavailable during play mode!");
                return false;
            }

            if (_APPSETTINGS != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Show Advanced Settings", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(_LABELWIDTH));
                _APPSETTINGS.editorShowAdvancedSettings = EditorGUILayout.Toggle(_APPSETTINGS.editorShowAdvancedSettings, GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();

                if (_APPSETTINGS.editorShowAdvancedSettings)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Settings Asset", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(_LABELWIDTH));
                    EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(_APPSETTINGS), EditorStyles.boldLabel, GUILayout.ExpandWidth(true));

                    var icon = EditorGUIUtility.IconContent("_Popup");

                    if (GUILayout.Button(icon, GUILayout.Width(80)))
                    {
                        AppConstReferences appReference = Resources.Load<AppConstReferences>("AppConstReferences");

                        if (appReference != null)
                        {
                            if (appReference.OpenAppSetting())
                            {
                                GUIUtility.ExitGUI();
                                return true;
                            }
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            else
            {
                EditorGUILayout.LabelField("Settings for the project has not been created!");

                if (GUILayout.Button("Create"))
                {
                    if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    _APPSETTINGS = CreateInstance<AppSettings>();
                    AssetDatabase.CreateAsset(_APPSETTINGS, "Assets/Resources/ProjectAppSettings.asset");
                    EditorUtility.FocusProjectWindow();

                    _SOSETTINGS = new SerializedObject(_APPSETTINGS);

                    if (!AssetDatabase.IsValidFolder($"Assets/BundledAssets"))
                    {
                        AssetDatabase.CreateFolder("Assets", "BundledAssets");
                    }

                    if (!AssetDatabase.IsValidFolder($"Assets/StreamingAssets"))
                    {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }

                    AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

                    if (appReferences != null)
                    {
                        _APPSETTINGS = appReferences.Settings;
                    }

                    //need to apply all defaults to settings
                    SetDefaults();

                    //check if the settings exists
                    AppInstances instances = Resources.Load<AppInstances>("ProjectAppInstances");

                    if (instances == null)
                    {
                        AppInstances temp = CreateInstance<AppInstances>();
                        AssetDatabase.CreateAsset(temp, "Assets/Resources/ProjectAppInstances.asset");
                        temp = appReferences.Instances;
                    }

                    Debug.Log("Adding scenes to build settings");

                    if (System.IO.Directory.Exists(Application.dataPath + "/com.brandlab360.core"))
                    {
                        HUBUtils.AddSceneToBuildSettings("Assets/com.brandlab360.core/Runtime/Scenes/" + _APPSETTINGS.projectSettings.loginSceneName);
                        HUBUtils.AddSceneToBuildSettings("Assets/com.brandlab360.core/Runtime/Scenes/" + _APPSETTINGS.projectSettings.avatarSceneName);
#if BRANDLAB360_SAMPLE_HDRP
                        HUBUtils.AddSceneToBuildSettings("Assets/com.brandlab360.core/Samples/HDRP Sample/Scenes/" + _APPSETTINGS.projectSettings.mainSceneName);
#endif
                    }
                    else
                    {
                        HUBUtils.AddSceneToBuildSettings("Packages/com.brandlab360.core/Runtime/Scenes/" + _APPSETTINGS.projectSettings.loginSceneName);
                        HUBUtils.AddSceneToBuildSettings("Packages/com.brandlab360.core/Runtime/Scenes/" + _APPSETTINGS.projectSettings.avatarSceneName);
#if BRANDLAB360_SAMPLE_HDRP
                        HUBUtils.AddSceneToBuildSettings("Assets/Samples/BrandLab360 Core/1.0.0/HDRP Sample/Scenes/" + _APPSETTINGS.projectSettings.mainSceneName);
#endif
                    }

                    AssetDatabase.SaveAssets();

                    //need to open build settings
                    EditorApplication.ExecuteMenuItem("File/Build Settings...");
                }
            }

            return true;
        }

        private static void SetDefaults()
        {
            //need to apply all defaults to settings
            _APPSETTINGS.HUDSettings.DefaultHUDOptions();

            if (_APPSETTINGS.playerSettings.managerInteraction.Count <= 0)
            {
                if (!string.IsNullOrEmpty(_COREPREFAB))
                {
                    HUBUtils.GetIRayCasters(_COREPREFAB, _APPSETTINGS.playerSettings.managerInteraction);
                }
            }

            if (string.IsNullOrEmpty(_APPSETTINGS.playerSettings.playerController))
            {
                _APPSETTINGS.playerSettings.playerController = "BLPlayer";
            }

            if (string.IsNullOrEmpty(_APPSETTINGS.editorTools.simulator))
            {
                _APPSETTINGS.editorTools.simulator = "_BRANDLAB360_SIMULATOR";
            }

            if (_APPSETTINGS.themeSettings.themeSettingsCreated == false)
            {
                _APPSETTINGS.themeSettings.CreateDefaults();
                _APPSETTINGS.themeSettings.themeSettingsCreated = true;
            }

            if (!_APPSETTINGS.playerSettings.emotesCreated)
            {
                _APPSETTINGS.playerSettings.emotesCreated = true;
                _APPSETTINGS.playerSettings.CreateDefaultEmotes();
            }

            if (!_APPSETTINGS.playerSettings.defaultsCreated)
            {
                _APPSETTINGS.playerSettings.defaultsCreated = true;
                _APPSETTINGS.playerSettings.CreateDefaults();
            }

            if (!_APPSETTINGS.projectSettings.defaultsCreated)
            {
                _APPSETTINGS.projectSettings.defaultsCreated = true;
                _APPSETTINGS.projectSettings.CreateDefaults();
            }

            if (!_APPSETTINGS.HUDSettings.defaultsCreated)
            {
                _APPSETTINGS.HUDSettings.defaultsCreated = true;
                _APPSETTINGS.HUDSettings.CreateDefaults();
            }
        }

        public static bool DrawContents()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Contents unavailable during play mode!");
                return false;
            }

            if(_APPINSTANCES != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ignore All IOObject Settings", EditorStyles.boldLabel, GUILayout.Width(_LABELWIDTH));
                _APPINSTANCES.ignoreIObjectSettings = EditorGUILayout.Toggle(_APPINSTANCES.ignoreIObjectSettings, GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();

                if (!_APPINSTANCES.ignoreIObjectSettings)
                {
                    EditorGUILayout.BeginHorizontal();

                    if (HUBUtils.GetScenes().ToArray().Length > 0)
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField("Manage your IOObjects within your project", EditorStyles.miniBoldLabel);
                        EditorGUILayout.LabelField("If your IOObject does not display then click the refesh button (current opened scene) OR open scene and select GameObject", EditorStyles.miniBoldLabel);
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        /* m_selectedIOSceneIndex = EditorGUILayout.Popup("", m_selectedIOSceneIndex, GetScenes().ToArray());
                         m_SelectedIOSceneName = GetScenes().ToArray()[m_selectedIOSceneIndex];*/


                        if (GUILayout.Button("Refresh"))
                        {
                            RefreshIOObjects();
                            GUIUtility.ExitGUI();
                            return false;
                        }

                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        EditorGUILayout.LabelField("There are no scenes in your build settings (excluding Intro & Avatar)", EditorStyles.miniBoldLabel);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                return !_APPINSTANCES.ignoreIObjectSettings;
            }
            else
            {
                EditorGUILayout.LabelField("APP Instances for the project has not been created!");

                if (GUILayout.Button("Create"))
                {
                    if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    _APPINSTANCES = CreateInstance<AppInstances>();
                    AssetDatabase.CreateAsset(_APPINSTANCES, "Assets/Resources/ProjectAppInstances.asset");
                    EditorUtility.FocusProjectWindow();

                    AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

                    if (appReferences != null)
                    {
                        _APPINSTANCES = appReferences.Instances;
                    }

                    //check if the settings exists
                    AppSettings settings = Resources.Load<AppSettings>("ProjectAppSettings");

                    if (settings == null)
                    {
                        AppSettings temp = CreateInstance<AppSettings>();
                        AssetDatabase.CreateAsset(temp, "Assets/Resources/ProjectAppSettings.asset");
                        _APPSETTINGS = temp;

                        SetDefaults();
                    }

                    AssetDatabase.SaveAssets();
                }
            }

            return true;
        }

        private void DrawHeader()
        {
            if (m_header != null)
            {
                GUILayout.Box(m_header, GUILayout.ExpandWidth(true));
            }
        }

        public void DrawFooter()
        {
            //copyright icon
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(m_copyright);

            if (GUILayout.Button(_BLANDLAB360WEBITE, m_hyperlink, GUILayout.Width(175)))
            {
                Application.OpenURL(_BLANDLAB360WEBITE);
            }

            if (GUILayout.Button("", m_hyperlink))
            {
                
            }

            if (GUILayout.Button("Documentation", m_Documentation, GUILayout.Width(150)))
            {
                Application.OpenURL(_DOCUMENTATION);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

        }

        private void DrawTabs()
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < System.Enum.GetValues(typeof(HUBTab)).Length; i++)
            {
                GUIStyle tabStyle = m_currentTab.Equals((HUBTab)i)? m_TabSelected : m_TabNormal;

                if (GUILayout.Button(((HUBTab)i).ToString(), tabStyle))
                {
                    m_currentTab = (HUBTab)i;
                    m_providerScroll = Vector2.zero;

                    if(m_currentProvider != null)
                    {
                        if (!m_currentProvider.Tab.Equals(m_currentTab))
                        {
                            m_currentProvider.OnDeactivate();
                            m_currentProvider = null;
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawDomains()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(m_domainArea));
            m_sideBarScroll = EditorGUILayout.BeginScrollView(m_sideBarScroll);

            foreach (ProviderDrawer provider in m_providers)
            {
                if (!provider.tab.Equals(m_currentTab)) continue;

                if(m_currentProvider == null)
                {
                    m_currentProvider = (HUBProvider)provider.method.Invoke(null, null);
                }

                GUIStyle domainStyle = m_currentProvider != null ? provider.label.Equals(m_currentProvider.label) ? m_DomainSelected : m_DomainNormal : m_DomainNormal;

                if (GUILayout.Button(provider.label, domainStyle))
                {
                    if(!m_currentProvider.label.Equals(provider.label))
                    {
                        if (m_currentProvider != null)
                        {
                            m_currentProvider.OnDeactivate();
                        }

                        m_currentProvider = (HUBProvider)provider.method.Invoke(null, null);

                        if (m_currentProvider != null)
                        {
                            m_currentProvider.OnActivate("", null);
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawProvider()
        {
            EditorGUILayout.BeginVertical();

            if (m_currentProvider != null)
            {
                EditorGUILayout.LabelField(m_currentProvider.label, m_HeaderFont, GUILayout.Height(30));
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                bool continueDrawCall = true;

                if(m_currentTab.Equals(HUBTab.Settings))
                {
                    continueDrawCall = DrawSettings();
                }
                else if(m_currentTab.Equals(HUBTab.Content))
                {
                    continueDrawCall = DrawContents();
                }

                if (continueDrawCall)
                {
                    m_currentProvider.OnHeader(position);
                }

                m_providerScroll = EditorGUILayout.BeginScrollView(m_providerScroll);

                if (continueDrawCall)
                {
                    m_currentProvider.OnGUI(position);
                }

                if (!IsBuilding)
                {
                    EditorGUILayout.EndScrollView();
                }

                if (continueDrawCall)
                {
                    m_currentProvider.OnFooter(position);
                }
            }

            if (!IsBuilding)
            {
                EditorGUILayout.EndVertical();
            }
        }

        public static bool ProviderExists(string label)
        {
            return m_providers.FirstOrDefault(x => x.label.Equals(label)) != null;
        }

        public static void OpenProvider(HUBTab tab, string label)
        {
            m_currentTab = tab;
            m_providerScroll = Vector2.zero;

            if (m_currentProvider != null)
            {
                m_currentProvider.OnDeactivate();
            }

            m_currentProvider = null;

            foreach (ProviderDrawer provider in m_providers)
            {
                if (!provider.tab.Equals(m_currentTab)) continue;

                if(provider.label.Equals(label))
                {
                    m_currentProvider = (HUBProvider)provider.method.Invoke(null, null);

                    if (m_currentProvider != null)
                    {
                        m_currentProvider.OnActivate("", null);
                    }

                    break;
                }
            }
        }

        public static void CloseWindow()
        {
            if (IsOpen)
            {
                m_mainWindow.Close();

                if (OnWindowClose != null)
                {
                    OnWindowClose.Invoke();
                }

                OnWindowClose = null;
            }
        }

        private static void RefreshIOObjects()
        {
            //need to get all uniqueIDs
            if (_APPINSTANCES != null)
            {
                //get active scene
                Scene scene = EditorSceneManager.GetActiveScene();

                //delete all
                foreach (GameObject sGO in scene.GetRootGameObjects())
                {
                    foreach (UniqueID tID in sGO.GetComponentsInChildren<UniqueID>(true))
                    {
                        AppInstances.SwitchSceneTriggerID sTrigger = _APPINSTANCES.GetSwitchSceneReference(tID.ID);

                        if (sTrigger == null)
                        {
                            _APPINSTANCES.RemoveUniqueID(tID.ID);
                        }
                    }
                }

                //get all
                foreach (GameObject sGO in scene.GetRootGameObjects())
                {
                    foreach (UniqueID tID in sGO.GetComponentsInChildren<UniqueID>(true))
                    {
                        //if ioobjects does not contain uniqueID, then add, else ignore.
                        _APPINSTANCES.AddIOObject(tID.ID, tID.GetSettings(), true);
                    }
                }
            }
        }

        [System.Serializable]
        private class ProviderDrawer
        {
            public int order;
            public string domain;
            public string label;
            public HUBTab tab;
            public MethodInfo method;
        }
    }

    public enum HUBTab { Settings, Interfaces, ToolBox, Content, Editor, Build, Host }
}
