using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateAPIEndpointsEditor : EditorWindow
    {
        private static CreateAPIEndpointsEditor m_window;
        private AppSettings m_settings;
        private SerializedObject m_asset;
        private AppLoginAPISettings m_loginSettings;
        private Vector2 m_scroll;
        private float m_variableSize = 200;

        public static void ShowWindow()
        {
            m_window = (CreateAPIEndpointsEditor)EditorWindow.GetWindow(typeof(CreateAPIEndpointsEditor));
            m_window.maxSize = new Vector2(800, 600);
            m_window.minSize = m_window.maxSize;
            m_window.Show();
        }

        private void OnEnable()
        {
            AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

            if (appReferences != null)
            {
                m_settings = appReferences.Settings;
            }
            else
            {
                m_settings = Resources.Load<AppSettings>("ProjectAppSettings");
            }

            m_loginSettings = Resources.Load<AppLoginAPISettings>("ProjectAPISettings");
            
            if(m_loginSettings != null)
            {
                m_asset = new SerializedObject(m_loginSettings);
            }

            HUBWindow.OnWindowClose += OnClose;
        }

        private void OnDestroy()
        {
            if (m_loginSettings != null)
            {
                EditorUtility.SetDirty(m_loginSettings);
                m_asset.ApplyModifiedProperties();
            }
        }

        private void OnClose()
        {
            m_window.Close();
        }

        private void OnGUI()
        {
            if (m_settings != null)
            {
                if (m_settings.brandlabLogo_Banner != null)
                {
                    GUILayout.Box(m_settings.brandlabLogo_Banner.texture, GUILayout.ExpandWidth(true));
                }
                else
                {
                    m_settings.brandlabLogo_Banner = Resources.Load<Sprite>("Logos/BrandLab360_Banner");
                }

                if (m_loginSettings == null)
                {
                    EditorGUILayout.LabelField("API Settings for the project has not been created!");

                    if (GUILayout.Button("Create"))
                    {
                        if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
                        {
                            AssetDatabase.CreateFolder("Assets", "Resources");
                        }

                        m_loginSettings = CreateInstance<AppLoginAPISettings>();
                        AssetDatabase.CreateAsset(m_loginSettings, "Assets/Resources/ProjectAPISettings.asset");
                        EditorUtility.FocusProjectWindow();

                        AssetDatabase.SaveAssets();

                        if (m_loginSettings != null)
                        {
                            m_asset = new SerializedObject(m_loginSettings);
                        }
                    }
                }
                else
                {
                    m_asset.Update();

                    EditorGUILayout.LabelField(m_settings.projectSettings.loginAPIMode.ToString().Substring(1), EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if(m_settings.projectSettings.loginAPIMode.Equals(APILoginMode._Salesforce))
                    {
                        DrawSalesforce();
                    }
                    else if(m_settings.projectSettings.loginAPIMode.Equals(APILoginMode._Hubspot))
                    {
                        DrawHubspot();
                    }
                }

                if(GUI.changed)
                {
                    EditorUtility.SetDirty(this);

                    EditorUtility.SetDirty(m_loginSettings);
                    m_asset.ApplyModifiedProperties();
                }
            }
        }

        private void DrawSalesforce()
        {
            EditorGUILayout.LabelField("Project", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Reference", GUILayout.Width(m_variableSize));
            m_loginSettings.salesforceSettings.projectID = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.projectID, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            m_loginSettings.salesforceSettings._DISPLAYVALUE = EditorGUILayout.Popup(m_loginSettings.salesforceSettings._DISPLAYVALUE, m_loginSettings.salesforceSettings._DISPLAYTYPE, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            m_scroll = EditorGUILayout.BeginScrollView(m_scroll);

            if (m_loginSettings.salesforceSettings._DISPLAYVALUE == 0)
            {
                EditorGUILayout.LabelField("Authentication", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("URL", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.authURL = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.authURL, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.authID = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.authID, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Secret", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.authSecret = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.authSecret, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Username", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.authUsername = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.authUsername, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Password", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.authPassword = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.authPassword, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Grant Type", GUILayout.Width(m_variableSize));

                if (m_loginSettings.salesforceSettings._EDITGRANTTYPE)
                {
                    m_loginSettings.salesforceSettings.authGranttype = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.authGranttype, GUILayout.ExpandWidth(true));
                }
                else
                {
                    EditorGUILayout.LabelField(m_loginSettings.salesforceSettings.authGranttype, GUILayout.ExpandWidth(true));
                }

                string butName = m_loginSettings.salesforceSettings._EDITGRANTTYPE ? "Save" : "Edit";

                if (GUILayout.Button(butName, GUILayout.Width(80)))
                {
                    m_loginSettings.salesforceSettings._EDITGRANTTYPE = !m_loginSettings.salesforceSettings._EDITGRANTTYPE;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Endpoints", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Create URL", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.sessionCreateURL = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.sessionCreateURL, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Login URL", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.sessionCheckURL = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.sessionCheckURL, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Push URL", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.sessionPushURL = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.sessionPushURL, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
            if (m_loginSettings.salesforceSettings._DISPLAYVALUE == 1)
            {
                EditorGUILayout.LabelField("Login", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", EditorStyles.miniBoldLabel, GUILayout.Width(m_variableSize));
                EditorGUILayout.LabelField("Saleforce Table Field", EditorStyles.miniBoldLabel, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("Form Reference", EditorStyles.miniBoldLabel, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Username", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.loginUsername.tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.loginUsername.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.salesforceSettings.loginUsername.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.loginUsername.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Password", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.loginPassword.tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.loginPassword.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.salesforceSettings.loginPassword.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.loginPassword.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.LabelField("Profile Data To Collect", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", EditorStyles.miniBoldLabel, GUILayout.Width(m_variableSize));
                EditorGUILayout.LabelField("Saleforce Table Field", EditorStyles.miniBoldLabel, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("Profile Reference", EditorStyles.miniBoldLabel, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                for (int i = 0; i < m_loginSettings.salesforceSettings.loginDataToCollect.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Field Data " + i.ToString(), GUILayout.Width(m_variableSize));
                    m_loginSettings.salesforceSettings.loginDataToCollect[i].tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.loginDataToCollect[i].tableField, GUILayout.ExpandWidth(true));
                    m_loginSettings.salesforceSettings.loginDataToCollect[i].profileReference = (ProfileDataReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.loginDataToCollect[i].profileReference, GUILayout.Width(200));

                    EditorGUILayout.EndHorizontal();
                }

                if (m_loginSettings.salesforceSettings.loginDataToCollect.Length > m_loginSettings.salesforceSettings.FixedDataCount)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        ProfileReference[] temp = new ProfileReference[m_loginSettings.salesforceSettings.loginDataToCollect.Length - 1];

                        for (int i = 0; i < temp.Length; i++)
                        {
                            temp[i] = m_loginSettings.salesforceSettings.loginDataToCollect[i];
                        }

                        m_loginSettings.salesforceSettings.loginDataToCollect = temp;
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                if (GUILayout.Button("Add"))
                {
                    ProfileReference[] temp = new ProfileReference[m_loginSettings.salesforceSettings.loginDataToCollect.Length + 1];

                    for (int i = 0; i < m_loginSettings.salesforceSettings.loginDataToCollect.Length; i++)
                    {
                        temp[i] = m_loginSettings.salesforceSettings.loginDataToCollect[i];
                    }

                    temp[temp.Length - 1] = new ProfileReference("", ProfileDataReference._custom);
                    m_loginSettings.salesforceSettings.loginDataToCollect = temp;
                    GUIUtility.ExitGUI();
                    return;
                }
            }
            else
            {
                EditorGUILayout.LabelField("Register", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", EditorStyles.miniBoldLabel, GUILayout.Width(m_variableSize));
                EditorGUILayout.LabelField("Saleforce Table Field", EditorStyles.miniBoldLabel, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("Form Reference", EditorStyles.miniBoldLabel, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Username", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.username.tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.username.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.salesforceSettings.username.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.username.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Email", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.email.tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.email.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.salesforceSettings.email.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.email.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Password", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.password.tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.password.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.salesforceSettings.password.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.password.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Terms & Conditions", GUILayout.Width(m_variableSize));
                m_loginSettings.salesforceSettings.termsAndCondition.tableField = EditorGUILayout.TextField("", m_loginSettings.salesforceSettings.termsAndCondition.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.salesforceSettings.termsAndCondition.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.salesforceSettings.termsAndCondition.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawHubspot()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Display", GUILayout.Width(m_variableSize));
            EditorGUILayout.Space();
            m_loginSettings.hubspotSettings._DISPLAYVALUE = EditorGUILayout.Popup(m_loginSettings.hubspotSettings._DISPLAYVALUE, m_loginSettings.hubspotSettings._DISPLAYTYPE, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if(m_loginSettings.hubspotSettings._DISPLAYVALUE == 0)
            {
                EditorGUILayout.LabelField("Access", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Token", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.accessToken = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.accessToken, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Endpoint", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.accessEndpoint = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.accessEndpoint, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Session ID Field", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.sessionIDField = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.sessionIDField, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
            else if(m_loginSettings.hubspotSettings._DISPLAYVALUE == 1)
            {
                EditorGUILayout.LabelField("Register", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", EditorStyles.miniBoldLabel, GUILayout.Width(m_variableSize));
                EditorGUILayout.LabelField("Hubspot Property Field", EditorStyles.miniBoldLabel, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("Form Reference", EditorStyles.miniBoldLabel, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Username", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.username.tableField = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.username.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.hubspotSettings.username.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.hubspotSettings.username.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Password", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.password.tableField = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.password.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.hubspotSettings.password.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.hubspotSettings.password.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Email", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.email.tableField = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.email.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.hubspotSettings.email.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.hubspotSettings.email.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Terms & Conditions", GUILayout.Width(m_variableSize));
                m_loginSettings.hubspotSettings.termsAndCondition.tableField = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.termsAndCondition.tableField, GUILayout.ExpandWidth(true));
                m_loginSettings.hubspotSettings.termsAndCondition.formReference = (LoginFormReference)EditorGUILayout.EnumPopup("", m_loginSettings.hubspotSettings.termsAndCondition.formReference, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.LabelField("Profile Data To Collect", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", EditorStyles.miniBoldLabel, GUILayout.Width(m_variableSize));
                EditorGUILayout.LabelField("Hubspot Property Field", EditorStyles.miniBoldLabel, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("Profile Reference", EditorStyles.miniBoldLabel, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();

                for (int i = 0; i < m_loginSettings.hubspotSettings.properties.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Field Data " + i.ToString(), GUILayout.Width(m_variableSize));
                    m_loginSettings.hubspotSettings.properties[i].tableField = EditorGUILayout.TextField("", m_loginSettings.hubspotSettings.properties[i].tableField, GUILayout.ExpandWidth(true));
                    m_loginSettings.hubspotSettings.properties[i].profileReference = (ProfileDataReference)EditorGUILayout.EnumPopup("", m_loginSettings.hubspotSettings.properties[i].profileReference, GUILayout.Width(200));

                    EditorGUILayout.EndHorizontal();
                }

                if (m_loginSettings.hubspotSettings.properties.Length > m_loginSettings.hubspotSettings.FixedDataCount)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        ProfileReference[] temp = new ProfileReference[m_loginSettings.hubspotSettings.properties.Length - 1];

                        for (int i = 0; i < temp.Length; i++)
                        {
                            temp[i] = m_loginSettings.hubspotSettings.properties[i];
                        }

                        m_loginSettings.hubspotSettings.properties = temp;
                        GUIUtility.ExitGUI();
                        return;
                    }
                }

                if (GUILayout.Button("Add"))
                {
                    ProfileReference[] temp = new ProfileReference[m_loginSettings.hubspotSettings.properties.Length + 1];

                    for (int i = 0; i < m_loginSettings.hubspotSettings.properties.Length; i++)
                    {
                        temp[i] = m_loginSettings.hubspotSettings.properties[i];
                    }

                    temp[temp.Length - 1] = new ProfileReference("", ProfileDataReference._custom);
                    m_loginSettings.hubspotSettings.properties = temp;
                    GUIUtility.ExitGUI();
                    return;
                }
            }
        }
    }
}
