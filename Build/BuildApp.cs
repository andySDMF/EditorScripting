using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

namespace BrandLab360.Editor
{
    public static class BuildApp
    {
#if UNITY_EDITOR
        public static WebClientMode BuildStreamingMode
        {
            get;
            set;
        }

        public static bool HUBBuild
        {
            get;
            set;
        }

        public static bool DeployToTargetPlatform
        {
            get;
            set;
        }

        private static string GetBuildPath()
        {
            return PlayerPrefs.GetString("Brandlab360_BuildPath");
        }
#endif

#if !UNITY_WEBGL
       //// [MenuItem("BrandLab360/Build/Local/New Build")]
        public static void LocalNewBuild()
        {
            Build(WebClientMode.None, "");
        }


       // [MenuItem("BrandLab360/Build/Local/Existing Build")]
        public static void LocalExistingBuild()
        {
            //get the HUB project settings
            AppSettings settings = GetSettings();

            PathCheck(settings);
            Build(WebClientMode.None, GetBuildPath());
        }

       // [MenuItem("BrandLab360/Build/PureWeb/Publish/New Build")]
        public static void NewPureWebBuild()
        {
            //get the HUB project settings
            Build(WebClientMode.PureWeb, "");
        }

      //  [MenuItem("BrandLab360/Build/PureWeb/Publish/Existing Build")]
        public static void ExistingPureWebBuild()
        {
            //get the HUB project settings
            AppSettings settings = GetSettings();
            PathCheck(settings);
            Build(WebClientMode.PureWeb, GetBuildPath());
        }

       // [MenuItem("BrandLab360/Build/PureWeb/PublishAndDeploy/New Build")]
        public static void NewPureWebPublishAndDeploy()
        {
            //get the HUB project settings
            DeployToTargetPlatform = true;
            NewPureWebBuild();
        }

       // [MenuItem("BrandLab360/Build/PureWeb/PublishAndDeploy/Existing Build")]
        public static void ExistingPureWebPublishAndDeploy()
        {
            //get the HUB project settings
            DeployToTargetPlatform = true;
            ExistingPureWebBuild();
        }

       // [MenuItem("BrandLab360/Build/Vagon/Publish/New Build")]
        public static void NewVagonBuild()
        {
            //get the HUB project settings
            Build(WebClientMode.Vagon, "");
        }

       // [MenuItem("BrandLab360/Build/Vagon/Publish/Existing Build")]
        public static void ExistingVagonBuild()
        {
            //get the HUB project settings
            AppSettings settings = GetSettings();
            PathCheck(settings);
            Build(WebClientMode.Vagon, GetBuildPath());
        }

        // [MenuItem("BrandLab360/Build/PureWeb/PublishAndDeploy/New Build")]
        public static void NewVagonPublishAndDeploy()
        {
            EditorUtility.DisplayDialog("Build Error", "Vagon publish not implemented yet", "OK");

            //get the HUB project settings
            // DeployToTargetPlatform = true;
            // NewPureWebBuild();
        }

        // [MenuItem("BrandLab360/Build/PureWeb/PublishAndDeploy/Existing Build")]
        public static void ExistingVagonPublishAndDeploy()
        {
            EditorUtility.DisplayDialog("Build Error", "Vagon publish not implemented yet", "OK");

            //get the HUB project settings
            // DeployToTargetPlatform = true;
            // ExistingPureWebBuild();
        }

        // [MenuItem("BrandLab360/Build/Vagon/Publish/New Build")]
        public static void NewBrandLabBuild()
        {
            EditorUtility.DisplayDialog("Build Error", "BlandLab build not implemented yet", "OK");

            //get the HUB project settings
            //Build(WebClientMode.BrandLab360, "");
        }

        // [MenuItem("BrandLab360/Build/Vagon/Publish/Existing Build")]
        public static void ExistingBrandLabBuild()
        {
            EditorUtility.DisplayDialog("Build Error", "BlandLab build not implemented yet", "OK");

            //get the HUB project settings
           // AppSettings settings = GetSettings();
           // PathCheck(settings);
           // Build(WebClientMode.BrandLab360, GetBuildPath());
        }


        // [MenuItem("BrandLab360/Build/PureWeb/PublishAndDeploy/New Build")]
        public static void NewBrandLabPublishAndDeploy()
        {
            EditorUtility.DisplayDialog("Build Error", "BlandLab publish not implemented yet", "OK");

            //get the HUB project settings
            // DeployToTargetPlatform = true;
            // NewPureWebBuild();
        }

        // [MenuItem("BrandLab360/Build/PureWeb/PublishAndDeploy/Existing Build")]
        public static void ExistingBrandLabPublishAndDeploy()
        {
            EditorUtility.DisplayDialog("Build Error", "BlandLab publish not implemented yet", "OK");

            //get the HUB project settings
            // DeployToTargetPlatform = true;
            // ExistingPureWebBuild();
        }
#endif

#if UNITY_WEBGL
        //[MenuItem("BrandLab360/Build/WebGL/Publish/New Build")]
        public static void NewWebGLBuild()
        {
            //get the HUB project settings
            BuildApp(WebClientMode.WebGL, "");
        }

       // [MenuItem("BrandLab360/Build/WebGL/Publish/Existing Build")]
        public static void ExistingWebGLBuild()
        {
            //get the HUB project settings
            AppSettings settings = GetSettings();
            PathCheck(settings);
            BuildApp(WebClientMode.WebGL, GetBuildPath());
        }

       // [MenuItem("BrandLab360/Build/WebGL/PublishAndRun/New Build")]
        public static void NewWebGLBuildAndRun()
        {
            //get the HUB project settings
            BuildApp(WebClientMode.WebGL, "", BuildOptions.AutoRunPlayer);
        }

       // [MenuItem("BrandLab360/Build/WebGL/PublishAndRun/Existing Build")]
        public static void ExistingWebGLBuildAndRun()
        {
            //get the HUB project settings
            AppSettings settings = GetSettings();
            PathCheck(settings);
            BuildApp(WebClientMode.WebGL, GetBuildPath(), BuildOptions.AutoRunPlayer);
        }
#endif
        private static AppSettings GetSettings()
        {
            AppSettings settings;

            AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");

            if (appReferences != null)
            {
                settings = appReferences.Settings;
            }
            else
            {
                settings = Resources.Load<AppSettings>("ProjectAppSettings");
            }

            return settings;
        }

        public static void PublishBuild(WebClientMode mode)
        {
            DeployToTargetPlatform = true;

            //get the HUB project settings
            AppSettings settings = GetSettings();
            PathCheck(settings);
            Build(mode, GetBuildPath());
        }

        private static void Build(WebClientMode mode, string folder, BuildOptions buildOptions = BuildOptions.None)
        {
            HUBWindow.IsBuilding = true;

            //check build settings for empty or  null scenes
            SetEditorBuildSettingsScenes();

            //get the HUB project settings
            AppSettings settings = GetSettings();
            BuildStreamingMode = mode;
            HUBBuild = true;

            settings.projectSettings.streamingMode = mode;
            SerializedObject sObject = new SerializedObject(settings);

            if (sObject != null)
            {
                sObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(settings);
            }

            // Get filename.
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built App", folder, "");

            if (path.Length != 0)
            {
                if(string.IsNullOrWhiteSpace(path))
                {
                    UnityEngine.Debug.Log("File path or EXE contains space. This is invalid!");
                    return;
                }

                HUBWindow.CloseWindow();

                //get all levels in the build settings
                List<string> scenes = GetScenes();
                string[] levels = new string[scenes.Count];

                for (int i = 0; i < scenes.Count; i++)
                {
                    levels[i] = scenes[i];

                    Debug.Log(levels[i]);
                }

                string fp = path.Replace('\\', '/');
                string[] folders = fp.Split('/');
                string appname = folders[folders.Length - 1];

                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
                buildPlayerOptions.scenes = levels;
                buildPlayerOptions.locationPathName = path + "/" + appname + ".exe";
                buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
                buildPlayerOptions.options = buildOptions;

                BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
                BuildSummary summary = report.summary;

                if (summary.result == BuildResult.Succeeded)
                {
                    Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                    PlayerPrefs.SetString("Brandlab360_BuildPath", path);
                }

                if (summary.result == BuildResult.Failed)
                {
                    Debug.Log("Build failed. Please check your build settings for deleted scenes");
                }
            }
        }

        private static void SetEditorBuildSettingsScenes()
        {
            Debug.Log("checking build settings");

            // Find valid Scene paths and make a list of EditorBuildSettingsScene
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            for(int i =0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string assetPath = "";
                bool assetPathExists = false;

                if (!string.IsNullOrEmpty(EditorBuildSettings.scenes[i].path))
                {
                    //need to check if scene path contains packages
                    if(EditorBuildSettings.scenes[i].path.Contains("Packages"))
                    {
                        assetPathExists = true;
                    }
                    else
                    {

                        assetPath = EditorBuildSettings.scenes[i].path.Replace("Assets", "");
                        assetPathExists = File.Exists(Application.dataPath + assetPath);
                    }
                }

                if (assetPathExists)
                {
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(EditorBuildSettings.scenes[i].path, true));
                }
                else
                {
                    Debug.Log("Scene does not exists [" + EditorBuildSettings.scenes[i].path + "]. Removing from build settings");
                }
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        private static void PathCheck(AppSettings settings)
        {
            if (!string.IsNullOrEmpty(GetBuildPath()) && !Directory.Exists(GetBuildPath()))
            {
                SerializedObject sObject = new SerializedObject(settings);
                PlayerPrefs.SetString("Brandlab360_BuildPath", "");

                if (sObject != null)
                {
                    sObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(settings);
                }
            }
        }

        private static List<string> GetScenes()
        {
            List<string> scenes = new List<string>();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenes.Add(EditorBuildSettings.scenes[i].path);
            }

            return scenes;
        }
    }
}
