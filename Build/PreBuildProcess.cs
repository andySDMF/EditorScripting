using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor;
using BrandLab360;
using System.IO;

namespace BrandLab360.Editor
{ 
    public class PreBuildProcess : MonoBehaviour, IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        [System.Obsolete]
        public void OnPreprocessBuild(BuildReport report)
        {
            //get the HUB project settings
            AppSettings settings = Resources.Load<AppSettings>("ProjectAppSettings");

            WebClientMode streamingMode = BuildApp.HUBBuild ? BuildApp.BuildStreamingMode : settings.projectSettings.streamingMode;

            if (streamingMode == WebClientMode.PureWeb)
            {
                if (!EditorUtility.DisplayDialog("Active Input Handling", "PureWeb does not support Both Inputs well & Input Asset backend behaviour should be set to Ignore for streaming. Please check settings first. Are you happy to continue", "Continue", "Abort"))
                {
                    throw new BuildFailedException("Build was canceled by the user.");
                }

                return;
            }
            else if(streamingMode == WebClientMode.WebGL)
            {
                if (!EditorUtility.DisplayDialog("Input Asset", "Input Asset backend behaviour should be set to Ignore for streaming. Please check settings first. Are you happy to continue", "Continue", "Abort"))
                {
                    throw new BuildFailedException("Build was canceled by the user.");
                }

                return;
            }

#if UNITY_STANDALONE
            //potentially get the build folder and delete contents from this first
            string exe = Path.GetFileName(report.summary.outputPath);
            string buildfolder = report.summary.outputPath.Replace(exe, "");

            string[] directoryFiles = Directory.GetFiles(buildfolder);

            for(int i = 0; i < directoryFiles.Length; i++)
            {
                File.Delete(directoryFiles[i]);
            }

            //delete all folders in folder
            string[] directoryFolders = Directory.GetDirectories(buildfolder);

            for (int i = 0; i < directoryFolders.Length; i++)
            {
                Directory.Delete(directoryFolders[i], true);
            }
#endif
            switch (streamingMode)
            {
                case WebClientMode.PureWeb:
                    PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                    PlayerSettings.defaultScreenHeight = 1080;
                    PlayerSettings.defaultScreenWidth = 1920;
                    break;
                case WebClientMode.Vagon:
                    PlayerSettings.fullScreenMode = FullScreenMode.MaximizedWindow;
                    PlayerSettings.defaultScreenHeight = 1080;
                    PlayerSettings.defaultScreenWidth = 1920;
                    break;
                case WebClientMode.None:
                    PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                    PlayerSettings.defaultScreenHeight = 720;
                    PlayerSettings.defaultScreenWidth = 1024;
                    break;
                case WebClientMode.WebGL:
                case WebClientMode.BrandLab360:

                    PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                    PlayerSettings.defaultScreenHeight = 1080;
                    PlayerSettings.defaultScreenWidth = 1920;
                    break;
            }

            Debug.Log("MyCustomBuildProcessor.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
        }
    }
}
