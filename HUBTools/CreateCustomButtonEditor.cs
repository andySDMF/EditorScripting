using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using TMPro;

namespace BrandLab360.Editor
{
    public class CreateCustomButtonEditor : CreateBaseEditor
    {
       // [MenuItem("BrandLab360/Core/Generics/Button/Custom")]
        public static void Create()
        {
            //show window to define chair options
            CustomButtonWindow window = (CustomButtonWindow)EditorWindow.GetWindow(typeof(CustomButtonWindow));
            window.setup = "Custom Button Window";
            window.maxSize = new Vector2(800f, 600f);
            window.minSize = window.maxSize;
            window.Show();
        }

        private class CustomButtonWindow : EditorWindow
        {
            public string setup { get; set; }

            private AppSettings cache;
            private Vector2 scrollPos;
            private Texture2D buttonImageSquare;
            private Texture2D buttonImageCircle;
            private Sprite buttonIcon;
            private ButtonAppearance.Appearance buttonAppearance = ButtonAppearance.Appearance._Round;
            private ButtonContent buttonContentType = ButtonContent._Icon;

            private string m_buttonTextString = "Text";
            private int theme = -1;
            private string[] allThemes;
            private ButtonJsonWrapper jsonWrapper;
            private Sprite selectedSprite;
            private string m_currentFile = "";

            public ButtonJsonWrapper Wrapper
            {
                get
                {
                    return jsonWrapper;
                }
            }

            public string CurrentFileName
            {
                get
                {
                    return m_currentFile;
                }
            }

            protected void OnEnable()
            {
                AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

                if (appReferences != null)
                {
                    cache = appReferences.Settings;
                }
                else
                {
                    cache = Resources.Load<AppSettings>("ProjectAppSettings");
                }

                //themes
                allThemes = new string[appReferences.Settings.themeSettings.buttonThemes.Count];

                for (int i = 0; i < allThemes.Length; i++)
                {
                    allThemes[i] = appReferences.Settings.themeSettings.buttonThemes[i].id;
                }

                if (theme >= allThemes.Length)
                {
                    if (allThemes.Length > 0)
                    {
                        theme = allThemes.Length - 1;
                    }
                    else
                    {
                        theme = 0;
                    }
                }
                else
                {
                    if (allThemes.Length > 0)
                    {
                        theme = 0;
                    }
                }

                if (jsonWrapper == null)
                {
                    jsonWrapper = new ButtonJsonWrapper();
                }

                //need to load Json textfile
                TextAsset jsonAsset = (TextAsset)GetAsset<TextAsset>("Assets/CustomButtons.txt");

                if(jsonAsset != null)
                {
                    jsonWrapper = JsonUtility.FromJson<ButtonJsonWrapper>(jsonAsset.text);
                }

                buttonImageSquare = (Texture2D)GetAsset<Texture2D>("Assets/com.brandlab360.core/Editor/ProceduralUIImage/Square.png");
                buttonImageCircle = (Texture2D)GetAsset<Texture2D>("Assets/com.brandlab360.core/Editor/ProceduralUIImage/Circle.png");
                buttonIcon = (Sprite)GetAsset<Sprite>("Assets/com.brandlab360.core/Editor/ProceduralUIImage/Null.png");
            }

            protected void OnGUI()
            {
                DrawTitle();
                DrawButtonDesigner();
                DrawButtons();
            }

            private void DrawTitle()
            {
                if (cache != null)
                {
                    if (cache.brandlabLogo_Banner != null)
                    {
                        GUILayout.Box(cache.brandlabLogo_Banner.texture, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        cache.brandlabLogo_Banner = Resources.Load<Sprite>("Logos/BrandLab360_Banner");
                    }
                }


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(setup.ToUpper(), EditorStyles.boldLabel, GUILayout.ExpandWidth(true));

                EditorGUILayout.LabelField(".txt File", GUILayout.Width(50));

                if (GUILayout.Button("Import", GUILayout.Width(80)))
                {
                    //open new window to save
                    CustomButtonImportWindow window = (CustomButtonImportWindow)EditorWindow.GetWindow(typeof(CustomButtonImportWindow));
                    window.Main = this;
                    window.maxSize = new Vector2(350f, 200f);
                    window.minSize = window.maxSize;
                    window.Show();
                }

                if (GUILayout.Button("Export", GUILayout.Width(80)))
                {
                    //open new window to save
                    CustomButtonExportWindow window = (CustomButtonExportWindow)EditorWindow.GetWindow(typeof(CustomButtonExportWindow));
                    window.Main = this;
                    window.maxSize = new Vector2(350f, 200f);
                    window.minSize = window.maxSize;
                    window.Show();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            private void DrawButtonDesigner()
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Button Designer", EditorStyles.miniBoldLabel, GUILayout.ExpandWidth(true));

                EditorGUILayout.LabelField("Load Design", GUILayout.Width(80));

                if (GUILayout.Button("Open", GUILayout.Width(80)))
                {
                    //open new window to save
                    CustomButtonLoadWindow window = (CustomButtonLoadWindow)EditorWindow.GetWindow(typeof(CustomButtonLoadWindow));
                    window.Main = this;
                    window.maxSize = new Vector2(300f, 500f);
                    window.minSize = window.maxSize;
                    window.Show();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Style", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));
                buttonAppearance = (ButtonAppearance.Appearance)EditorGUILayout.EnumPopup(buttonAppearance, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("Theme", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));
                theme = EditorGUILayout.Popup(theme, allThemes);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Content", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.Width(100));
                buttonContentType = (ButtonContent)EditorGUILayout.EnumPopup(buttonContentType, GUILayout.ExpandWidth(true));

       
                if (buttonContentType.Equals(ButtonContent._Icon))
                {
                    selectedSprite = (Sprite)EditorGUILayout.ObjectField(selectedSprite, typeof(Sprite), false, GUILayout.Width(150));

                    if(selectedSprite != null)
                    {
                        buttonIcon = selectedSprite;
                    }
                    else
                    {
                        if(!buttonIcon.name.Equals("Null"))
                        {
                            buttonIcon = (Sprite)GetAsset<Sprite>("Assets/com.brandlab360.core/Editor/ProceduralUIImage/Null.png");
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(m_buttonTextString))
                    {
                        m_buttonTextString = "Text";
                    }

                    m_buttonTextString = EditorGUILayout.TextField(m_buttonTextString, GUILayout.Width(100));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (buttonAppearance.Equals(ButtonAppearance.Appearance._Square))
                {
                    GUILayout.Box(buttonImageSquare, GUILayout.ExpandWidth(true), GUILayout.MinHeight(400));
                }
                else
                {
                    GUILayout.Box(buttonImageCircle, GUILayout.ExpandWidth(true), GUILayout.MinHeight(400));
                }

                if (buttonContentType.Equals(ButtonContent._Icon))
                {
                    Rect rect = new Rect(300, 250, 200, 200);
                    GUI.DrawTexture(rect, buttonIcon.texture);
                }
                else
                {
                    Rect rect = new Rect(300, 250, 200, 200);
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.black;
                    style.fontSize = 62;
                    style.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(rect, m_buttonTextString, style);
                }

                GUIStyle warning = new GUIStyle();
                warning.normal.textColor = Color.yellow;
                warning.fontStyle = FontStyle.BoldAndItalic;
                EditorGUILayout.LabelField("*Button not to scale, this is for representational value only", warning);
            }

            private void DrawButtons()
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Create"))
                {
                    Create(m_currentFile);
                }

                if (GUILayout.Button("Apply changes to Scene"))
                {
                    ButtonJson exists = jsonWrapper.buttons.FirstOrDefault(x => x.filename.Equals(m_currentFile));

                    if (exists != null)
                    {
                        //show display dialog warning to override button json
                        if (!EditorUtility.DisplayDialog("Warning", "Are you sure you want to apply changes in scene?", "Yes", "No"))
                        {
                            GUIUtility.ExitGUI();
                            return;
                        }
                        
                        //need to locate all with a CustomButton.cs on it and GUID matches

                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Warning", "Cannot apply to changes to a button that has not been saved.", "OK");
                    }
                }

                if (GUILayout.Button("Save"))
                {
                    bool canSave = true;

                    if (buttonContentType.Equals(ButtonContent._Icon))
                    {
                        if(buttonIcon.name.Equals("Null"))
                        {
                            EditorUtility.DisplayDialog("Warning", "Icon cannot be empty", "OK");
                            canSave = false;
                        }
                    }
                    else
                    {

                    }

                    if(canSave)
                    {
                        //open new window to save
                        CustomButtonSaveWindow window = (CustomButtonSaveWindow)EditorWindow.GetWindow(typeof(CustomButtonSaveWindow));
                        window.Main = this;
                        window.Filename = m_currentFile;
                        window.maxSize = new Vector2(350f, 200f);
                        window.minSize = window.maxSize;
                        window.Show();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            private Object GetAsset<T>(string path)
            {
                Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(T));

                if (obj == null)
                {
                    obj = GetPackageAsset<T>(path);
                }

                return obj;
            }

            private Object GetPackageAsset<T>(string path)
            {
                return AssetDatabase.LoadAssetAtPath(path.Replace("Assets", "Packages"), typeof(T));
            }

            public void Save(string filename, bool create = false)
            {
                m_currentFile = filename;

                if (jsonWrapper == null)
                {
                    jsonWrapper = new ButtonJsonWrapper();
                }

                ButtonJson exists = jsonWrapper.buttons.FirstOrDefault(x => x.filename.Equals(filename));
                bool overrideJson = false;

                if(exists != null)
                {
                    //show display dialog warning to override button json
                    if(!EditorUtility.DisplayDialog("Warning", "Button [" + filename + "]. Are you sure you want to override existing button?", "Yes", "No"))
                    {
                        return;
                    }

                    overrideJson = true;

                }
                else
                {
                    exists = new ButtonJson(System.Guid.NewGuid().ToString());
                    exists.filename = filename;
                }

                exists.style = buttonAppearance.ToString();
                exists.theme = theme > 0 && theme < cache.themeSettings.buttonThemes.Count ? cache.themeSettings.buttonThemes[theme].id : "";


                string assetContent = "";

                if (buttonContentType.Equals(ButtonContent._Icon))
                {
                    assetContent = AssetDatabase.GetAssetPath(buttonIcon);
                }
                else
                {
                    assetContent = m_buttonTextString;
                }

                exists.content = assetContent;

                if(!overrideJson)
                {
                    jsonWrapper.buttons.Add(exists);
                }

                //need to post to txt file
                System.IO.File.WriteAllText(Application.dataPath + "/CustomButtons.txt", JsonUtility.ToJson(jsonWrapper));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                if(create)
                {
                    Create(m_currentFile);
                }
            }

            private void Create(string filename)
            {
                if(string.IsNullOrEmpty(filename))
                {
                    filename = System.Guid.NewGuid().ToString();
                }

                bool canCreate = true;

                //this is where we need to create the physical GameObject from the design
                if (buttonContentType.Equals(ButtonContent._Icon))
                {
                    if (buttonIcon.name.Equals("Null"))
                    {
                        EditorUtility.DisplayDialog("Warning", "Icon is empty. Cannot create button", "OK");
                        canCreate = false;
                    }
                }

                if(canCreate)
                {
                    Create3DButton(filename);
                }
            }

            public void Load(string filename)
            {
                ButtonJson exists = jsonWrapper.buttons.FirstOrDefault(x => x.filename.Equals(filename));

                if(exists != null)
                {
                    m_currentFile = filename;

                    buttonAppearance = exists.style.Equals("_Square") ? ButtonAppearance.Appearance._Square : ButtonAppearance.Appearance._Round;
                    buttonContentType = System.IO.Path.HasExtension(exists.content) ? ButtonContent._Icon : ButtonContent._Text;

                    if(buttonContentType.Equals(ButtonContent._Icon))
                    {
                        Sprite sp = (Sprite)GetAsset<Sprite>(exists.content);

                        if(sp != null)
                        {
                            selectedSprite = sp;
                            buttonIcon = selectedSprite;
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Warning", "Project does not contain Sprite [" + exists.content + "]", "OK");
                        }
                    }
                    else
                    {
                        m_buttonTextString = exists.content;
                    }
                }
            }

            private void Create3DButton(string filename)
            {
                RectTransform rectT = null;

                Canvas goCanvas = new GameObject().AddComponent<Canvas>();
                CanvasScaler goCanvasScaler = goCanvas.gameObject.AddComponent<CanvasScaler>();
                GraphicRaycaster goRaycaster = goCanvas.gameObject.AddComponent<GraphicRaycaster>();
                goRaycaster.ignoreReversedGraphics = false;
                goCanvas.gameObject.layer = LayerMask.NameToLayer("Default");


                goCanvas.name = "3DButton_" + filename;
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

                if (buttonAppearance.Equals(ButtonAppearance.Appearance._Round))
                {
                    goImage.sprite = Resources.Load<Sprite>("World/Circle");
                }
                else
                {
                    goImage.sprite = Resources.Load<Sprite>("World/Square");
                }

                goImage.color = Color.white;

                Button but = goCanvas.gameObject.AddComponent<Button>();
                but.targetGraphic = goImage;
                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.None;
                but.navigation = nav;

                if(buttonContentType.Equals(ButtonContent._Icon))
                {
                    Image goIcon = new GameObject().AddComponent<Image>();
                    goIcon.gameObject.name = "Image_Icon";
                    goIcon.sprite = buttonIcon;

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
                }
                else
                {
                    TextMeshProUGUI tmpPro = new GameObject().AddComponent<TextMeshProUGUI>();
                    tmpPro.gameObject.name = "TMPro_Text";
                    tmpPro.text = m_buttonTextString;
                    tmpPro.fontSize = 0.05f;
                    tmpPro.alignment = TextAlignmentOptions.Center;

                    tmpPro.transform.SetParent(rectT);
                    tmpPro.transform.localPosition = Vector3.zero;
                    tmpPro.transform.localScale = Vector3.one;
                    tmpPro.transform.localEulerAngles = Vector3.zero;
                    rectT = tmpPro.GetComponent<RectTransform>();
                    rectT.anchorMin = Vector2.zero;
                    rectT.anchorMax = Vector2.one;
                    rectT.pivot = new Vector2(0.5f, 0.5f);
                    rectT.offsetMax = new Vector2(-0.05f, -0.05f);
                    rectT.offsetMin = new Vector2(0.05f, 0.05f);
                    rectT.localScale = Vector3.one;
                }

                AppConstReferences settings = Resources.Load<AppConstReferences>("AppConstReferences");
                ButtonTheme theme = goCanvas.gameObject.AddComponent<ButtonTheme>();

                if (settings != null)
                {
                    theme.Apply(settings.Settings.themeSettings, this.theme);
                }

                if(EditorUtility.DisplayDialog("Action", "Do you want to add ButtonAppearance.cs to button?", "Yes", "No"))
                {
                    ButtonAppearance style = goCanvas.gameObject.AddComponent<ButtonAppearance>();
                    style.Apply(buttonAppearance);
                }

                ButtonJson exists = jsonWrapper.buttons.FirstOrDefault(x => x.filename.Equals(filename));
                string guid = exists != null ? exists.guid : filename;

                CustomButton customBut = goCanvas.gameObject.AddComponent<CustomButton>();
                customBut.GUID = guid;

                CreateParentAndSelectObject("_3DBUTTONS", goCanvas.transform);
            }

            private enum ButtonContent { _Icon, _Text }
        }

        [System.Serializable]
        protected class ButtonJsonWrapper
        {
            public List<ButtonJson> buttons = new List<ButtonJson>();
        }

        [System.Serializable]
        public class ButtonJson
        {
            public string filename;
            public string style;
            public string content;
            public string theme;
            public string guid;

            public ButtonJson(string guid)
            {
                this.guid = guid;
            }
        }

        private class CustomButtonSaveWindow : EditorWindow
        {
            public CustomButtonWindow Main { get; set; }

            private static string filename = "";

            public string Filename
            {
                get
                {
                    return filename;
                }
                set
                {
                    filename = value;
                }
            }

            protected void OnGUI()
            {
                EditorGUILayout.LabelField("Save Custom Button", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Filename", EditorStyles.miniBoldLabel);
                filename = EditorGUILayout.TextField(filename);

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Cancel", GUILayout.Height(30)))
                {
                    Close();
                    GUIUtility.ExitGUI();
                    return;
                }

                if (GUILayout.Button("Save", GUILayout.Height(30)))
                {
                    bool canSave = true;

                    if (string.IsNullOrEmpty(filename))
                    {
                        EditorUtility.DisplayDialog("Warning", "Filename must not be empty", "OK");
                        canSave = false;
                    }

                    if (canSave)
                    {
                        Main.Save(filename);
                        Close();
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                if (GUILayout.Button("Save & Create", GUILayout.Height(30)))
                {
                    bool canSave = true;

                    if (string.IsNullOrEmpty(filename))
                    {
                        EditorUtility.DisplayDialog("Warning", "Filename must not be empty", "OK");
                        canSave = false;
                    }

                    if (canSave)
                    {
                        Main.Save(filename, true);
                        Close();
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private class CustomButtonLoadWindow : EditorWindow
        {
            public CustomButtonWindow Main { get; set; }
            private Vector2 scrollPos;
            private Color normal = Color.white;
            private Color selected = Color.cyan;

            private string selectedFile = "";

            protected void OnGUI()
            {
                EditorGUILayout.LabelField("Load Custom Button", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Filename", EditorStyles.miniBoldLabel);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));

                foreach (ButtonJson button in Main.Wrapper.buttons)
                {
                    if(selectedFile.Equals(button.filename))
                    {
                        GUI.backgroundColor = selected;
                    }
                    else
                    {
                        GUI.backgroundColor = normal;
                    }

                    if(GUILayout.Button(button.filename))
                    {
                        selectedFile = button.filename;
                    }
                }

                EditorGUILayout.EndScrollView();


                GUI.backgroundColor = normal;

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Cancel", GUILayout.Height(30)))
                {
                    Close();
                    GUIUtility.ExitGUI();
                    return;
                }

                if (GUILayout.Button("Load", GUILayout.Height(30)))
                {
                    if(string.IsNullOrEmpty(selectedFile))
                    {
                        EditorUtility.DisplayDialog("Warning", "Cannot load. There is no selected file. Please select a file.", "OK");
                    }
                    else
                    {
                        Main.Load(selectedFile);
                        Close();
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private class CustomButtonImportWindow : EditorWindow
        {
            public CustomButtonWindow Main { get; set; }

            protected void OnGUI()
            {
                EditorGUILayout.LabelField("Import Custom Button .TXT", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("File Importer", EditorStyles.miniBoldLabel);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Cancel", GUILayout.Height(30)))
                {
                    Close();
                    GUIUtility.ExitGUI();
                    return;
                }

                if (GUILayout.Button("Import", GUILayout.Height(30)))
                {
                    string path = EditorUtility.OpenFilePanel("Import Custom Button TXT", Application.dataPath, "txt");

                    if(!string.IsNullOrEmpty(path) && path.Length != 0)
                    {
                        string json = System.IO.File.ReadAllText(path);

                        if(json.Contains("{\"buttons\""))
                        {
                            ButtonJsonWrapper wrapper = JsonUtility.FromJson<ButtonJsonWrapper>(json);

                            if (wrapper != null)
                            {
                                foreach (ButtonJson but in wrapper.buttons)
                                {
                                    ButtonJson exists = Main.Wrapper.buttons.FirstOrDefault(x => x.filename.Equals(but.filename));

                                    if (exists != null)
                                    {
                                        //show display dialog warning to override button json
                                        if (!EditorUtility.DisplayDialog("Warning", "Button [" + but.filename + "]. Are you sure you want to override existing button?", "Yes", "No"))
                                        {
                                            continue;
                                        }

                                        exists.filename = but.filename;
                                        exists.style = but.style;
                                        exists.theme = but.theme;
                                        exists.content = but.content;
                                    }
                                    else
                                    {
                                        exists = new ButtonJson(but.guid);
                                        exists.filename = but.filename;
                                        exists.style = but.style;
                                        exists.theme = but.theme;
                                        exists.content = but.content;

                                        Main.Wrapper.buttons.Add(exists);
                                    }
                                }

                                //need to post to txt file
                                System.IO.File.WriteAllText(Application.dataPath + "/CustomButtons.txt", JsonUtility.ToJson(Main.Wrapper));
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();

                                Close();
                                GUIUtility.ExitGUI();
                                return;
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Warning", "File could not be loaded. Json does not match ButtonJsonWrapper", "OK");
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Warning", "File could not be loaded. Json does not match ButtonJsonWrapper", "OK");
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private class CustomButtonExportWindow : EditorWindow
        {
            public CustomButtonWindow Main { get; set; }

            private string m_filename = "";

            protected void OnGUI()
            {
                EditorGUILayout.LabelField("Export Custom Button .TXT", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("FileName", EditorStyles.miniBoldLabel);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                m_filename = EditorGUILayout.TextField(m_filename, GUILayout.ExpandWidth(true));

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Cancel", GUILayout.Height(30)))
                {
                    Close();
                    GUIUtility.ExitGUI();
                    return;
                }

                if (GUILayout.Button("Export", GUILayout.Height(30)))
                {
                    string path = EditorUtility.SaveFilePanel("Choose Location of Export", Application.dataPath, m_filename, "txt");

                    if(!string.IsNullOrEmpty(path) && path.Length != 0)
                    {
                        System.IO.File.WriteAllText(path, JsonUtility.ToJson(Main.Wrapper));
                        EditorUtility.RevealInFinder(path);
                    }

                    Close();
                    GUIUtility.ExitGUI();
                    return;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
