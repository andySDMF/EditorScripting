#if UNITY_STANDALONE && UNITY_2021_1_OR_NEWER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BrandLab360.Editor
{
    public class PostBuildProcess : MonoBehaviour
    {
        private static string m_ClientID = "";
        private static string m_ClienSecret = "";
        private static WebClientMode m_mode;
        private static string m_zipFolderPath = "";
        private static string m_appName = "";
        private static string m_pureWebModelJson = "";
        private static string m_previousMessage = "";
        private static bool m_pureWebModelStarted = false;
        private static bool m_pureWebUploadCancelled = false;
        private static Thread m_thread;
        private static Process m_process;
        private static bool m_pureWebFinished = false;
        private static PureWebTaskEvent m_pureWebTask;
        private static PureWebPassword m_pureWewbPassword;

        private static string inactiveModelVersion;
        private static string inactiveModelID;
        private static bool m_pureWebPasswordRequired = false;

        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            string folderPath = Path.GetDirectoryName(pathToBuiltProject);
            folderPath = folderPath.Replace('/', '\\');
            string folder = Path.GetDirectoryName(pathToBuiltProject);

            if(string.IsNullOrWhiteSpace(pathToBuiltProject))
            {
                UnityEngine.Debug.Log("File path or EXE contains space. This is invalid!");
                return;
            }

            //need to check if the zipped folder exists, if true delete
            if(File.Exists(folderPath + ".zip"))
            {
                File.Delete(folderPath + ".zip");
            }

            //get the HUB project settings

            AppConstReferences appReferences = Resources.Load<AppConstReferences>("AppConstReferences");
            AppSettings settings = null;

            if (appReferences != null)
            {
                settings = appReferences.Settings;
            }
            else
            {
                settings = Resources.Load<AppSettings>("ProjectAppSettings");
            }

            WebClientMode streamingMode = BuildApp.HUBBuild ? BuildApp.BuildStreamingMode : settings.projectSettings.streamingMode;

            switch (streamingMode)
            {
                case WebClientMode.PureWeb:
                    m_pureWebPasswordRequired = false;
                    m_ClientID = settings.editorTools.pureWebClientID;
                    m_ClienSecret = settings.editorTools.pureWebClientSecret;
                    m_mode = WebClientMode.PureWeb;
                    break;
                case WebClientMode.Vagon:
                    m_mode = WebClientMode.Vagon;
                    break;
                case WebClientMode.None:
                case WebClientMode.WebGL:
                case WebClientMode.BrandLab360:
                    m_mode = WebClientMode.None;
                    break;
            }

            string fp = folderPath.Replace('\\', '/');
            string[] folders = fp.Split('/');
            m_appName = folders[folders.Length - 1];
            m_zipFolderPath = folderPath;
            m_pureWebModelJson = "";
            m_previousMessage = "";
            m_pureWebModelStarted = false;
            BuildApp.HUBBuild = false;

            UnityEngine.Debug.Log("Compressing project to zip folder");

            bool zipCreated = CreateZipfileFromBuild(folderPath, folderPath + ".zip");

            if(zipCreated)
            {
                if (BuildApp.DeployToTargetPlatform)
                {
                    BuildApp.DeployToTargetPlatform = false;

                    if (m_mode.Equals(WebClientMode.PureWeb))
                    {
                        GetPureWebProgressUploadValue();
                        ExecutePureWebCommand("/k npm -v", PureWebTaskEvent.PureWebCLICheck);
                    }
                    else if (m_mode.Equals(WebClientMode.Vagon))
                    {
                        
                    }
                    else
                    {

                    }
                }
                else
                {
                    BuildApp.DeployToTargetPlatform = false;
                    EditorUtility.RevealInFinder(pathToBuiltProject);
                }
            }
        }

        private static bool CreateZipfileFromBuild(string buildDirectory, string pathZipFile)
        {
            if (File.Exists(pathZipFile))
                File.Delete(pathZipFile);
            try
            {
                ZipFile.CreateFromDirectory(buildDirectory, pathZipFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void ExecutePureWebCommand(string args, PureWebTaskEvent evt)
        {
            m_thread = new Thread(delegate () { PureWebCommand(args, evt); });
            m_thread.Start();
        }

        private static void PureWebCommand(string args, PureWebTaskEvent evt)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            m_pureWebTask = evt;

            if (!evt.Equals(PureWebTaskEvent.Finished))
            {
                switch (evt)
                {
                    case PureWebTaskEvent.PureWebNPMCheck:
                    case PureWebTaskEvent.PureWebInstallCLI:
                        process.StartInfo.WorkingDirectory = @"C:\Program Files\nodejs\";
                        break;
                    case PureWebTaskEvent.PureWebCLICheck:
                    case PureWebTaskEvent.PurewebConnectClientID:
                    case PureWebTaskEvent.PurewebConnectClientSecret:
                        //  string userDirectory = System.Environment.GetEnvironmentVariable("USERPROFILE");
                        //  process.StartInfo.WorkingDirectory = userDirectory + @"\AppData\Roaming\npm\";
                        break;
                }
            }

            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.ErrorDialog = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            if(evt.Equals(PureWebTaskEvent.PureWebConfirmation))
            {
                process.StartInfo.RedirectStandardInput = true;
            }

            process.EnableRaisingEvents = true;

            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log(e.Data);

                    if (evt.Equals(PureWebTaskEvent.PureWebNPMCheck))
                    {
                        if (e.Data.Contains("'npm' is not recognized as an internal or external command"))
                        {
                            UnityEngine.Debug.Log("Please install node.js OR restart PC");
                            Application.OpenURL("https://nodejs.org/en/");
                            evt = PureWebTaskEvent.Finished;
                        }
                    }
                    else if(evt.Equals(PureWebTaskEvent.PureWebCLICheck))
                    {
                        if (e.Data.Contains("'pw' is not recognized as an internal or external command"))
                        {
                            UnityEngine.Debug.Log("Please restart PC");
                            evt = PureWebTaskEvent.Finished;
                        }
                    }
                    else if (evt.Equals(PureWebTaskEvent.PureWebConfirmation))
                    {
                        if (e.Data.Contains("Exceeded version limit"))
                        {
                            evt = PureWebTaskEvent.PureWebListVersions;
                            process.Kill();
                        }
                    }
                }
            });

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log(e.Data);

                    if (evt.Equals(PureWebTaskEvent.PureWebCLICheck))
                    {
                        evt = PureWebTaskEvent.PurewebConnectClientID;
                    }
                    else if (evt.Equals(PureWebTaskEvent.PureWebConfirmation))
                    {
                        if(e.Data.Contains("Runtime OS: Windows"))
                        {
                            process.StandardInput.WriteLine("Y");
                        }
                        else if(e.Data.Contains("Initiating model upload"))
                        {
                            UnityEngine.Debug.Log("Please Wait. This might take 10+ minutes.......");
                        }
                        else if(e.Data.Contains("Your model Plugin_Demo has been uploaded to our deployment queue"))
                        {
                            process.Kill();
                        }
                    }
                    else if(evt.Equals(PureWebTaskEvent.PureWebListVersions))
                    {
                        //need to create a string from the output
                        if (m_previousMessage.Equals("Retrieving all models"))
                        {
                            m_pureWebModelStarted = true;
                        }

                        if(m_pureWebModelStarted)
                        {
                            m_pureWebModelJson += e.Data;

                            if(e.Data.Equals("]"))
                            {
                                m_pureWebModelStarted = false;
                                evt = PureWebTaskEvent.PureWebDelete;
                                process.Kill();
                            }
                        }
                    }

                    m_previousMessage = e.Data;
                }
            });

            m_process = process;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Close();

            if(m_mode.Equals(WebClientMode.PureWeb))
            {
                PureWeb(evt);
            }
        }

        private static void PureWeb(PureWebTaskEvent evt)
        {
            if (evt.Equals(PureWebTaskEvent.PureWebCLICheck))
            {
                UnityEngine.Debug.Log("PureWeb CLI check");
                ExecutePureWebCommand("/k pw --version", PureWebTaskEvent.PureWebInstallCLI);
            }
            else if (evt.Equals(PureWebTaskEvent.PureWebInstallCLI))
            {
                UnityEngine.Debug.Log("PureWeb Install check");
                ExecutePureWebCommand("/k npm install -g @pureweb/platform-cli", PureWebTaskEvent.PurewebConnectClientID);
            }
            else if (evt.Equals(PureWebTaskEvent.PurewebConnectClientID))
            {
                UnityEngine.Debug.Log("PureWeb Client ID Configuration");
                ExecutePureWebCommand("/k pw config set client-id " + m_ClientID, PureWebTaskEvent.PurewebConnectClientSecret);
            }
            else if (evt.Equals(PureWebTaskEvent.PurewebConnectClientSecret))
            {
                UnityEngine.Debug.Log("PureWeb Client Secret Configuration");
                ExecutePureWebCommand("/k pw config set client-secret " + m_ClienSecret, PureWebTaskEvent.PureWebUpload);
            }
            else if (evt.Equals(PureWebTaskEvent.PureWebUpload))
            {
                UnityEngine.Debug.Log("PureWeb Upload");
                ExecutePureWebCommand("/k pw model create " + m_appName + " " + m_zipFolderPath + ".zip --exe " + m_appName + ".exe --primary", PureWebTaskEvent.PureWebConfirmation);
          
            }
            else if(evt.Equals(PureWebTaskEvent.PureWebListVersions))
            {
                UnityEngine.Debug.Log("PureWeb attain model list");
                ExecutePureWebCommand("/k pw model list", PureWebTaskEvent.PureWebListVersions);
            }
            else if (evt.Equals(PureWebTaskEvent.PureWebDelete))
            {
                //de-serialize the mode json
                string singleModelJson = "";
                bool singleModelStarted = false;
                List<PureWebModel> models = new List<PureWebModel>();

                for (int i = 0; i < m_pureWebModelJson.Length; i++)
                {
                    if(m_pureWebModelJson[i].Equals('{'))
                    {
                        singleModelJson = "";
                        singleModelStarted = true;
                    }

                    if(singleModelStarted)
                    {
                        singleModelJson += m_pureWebModelJson[i];
                    }

                    if (m_pureWebModelJson[i].Equals('}'))
                    {
                        singleModelStarted = false;
                        PureWebModel model = JsonUtility.FromJson<PureWebModel>(singleModelJson.Replace(" ", ""));

                        if (model != null && model.name.Equals(m_appName))
                        {
                            models.Add(model);
                        }
                    }
                }

                inactiveModelVersion = models.FirstOrDefault(x => x.active == false).version;
                inactiveModelID = models.FirstOrDefault(x => x.active == false).id;

                UnityEngine.Debug.Log("PureWeb Model Delete version [" + inactiveModelVersion + "]");
                m_pureWebPasswordRequired = true;
            }
            else
            {
                UnityEngine.Debug.Log("Finsihed Post build process & upload");
                m_pureWebFinished = true;
            }
        }

        private static void DeletePureWebVersion(string id, string version, bool cancelled)
        {
            if(m_pureWewbPassword != null)
            {
                UnityEngine.Debug.Log("Closing window");
                m_pureWewbPassword.Close();
            }

            m_pureWewbPassword = null;
            m_pureWebPasswordRequired = false;

            if (!cancelled)
            {
                ExecutePureWebCommand("/k pw model delete " + id + " " + version, PureWebTaskEvent.PureWebUpload);
            }
            else
            {
                m_pureWebUploadCancelled = true;
                EditorUtility.ClearProgressBar();
                PureWeb(PureWebTaskEvent.Finished);
            }
        }

        private static async void GetPureWebProgressUploadValue()
        {
            EditorUtility.ClearProgressBar();

            float progressValue = 0.0f;
            m_pureWebUploadCancelled = false;
            m_pureWebFinished = false;

            while (!m_pureWebUploadCancelled && progressValue < 1.0f)
            {
                if (m_pureWebPasswordRequired)
                {
                    m_pureWewbPassword = (PureWebPassword)EditorWindow.GetWindow(typeof(PureWebPassword));
                    m_pureWewbPassword.Set(inactiveModelID, inactiveModelVersion, DeletePureWebVersion);
                    m_pureWewbPassword.maxSize = new Vector2(500f, 200f);
                    m_pureWewbPassword.minSize = m_pureWewbPassword.maxSize;

                    var position = m_pureWewbPassword.position;
                    position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
                    m_pureWewbPassword.position = position;

                    m_pureWewbPassword.Show();
                }
                else
                {
                    if (!m_pureWebFinished)
                    {
                        if (progressValue < 0.7f)
                        {
                            progressValue += 0.0001f;
                        }
                    }
                    else
                    {
                        progressValue += 0.01f;
                    }
                }

                await Task.Delay(1);

                if(EditorUtility.DisplayCancelableProgressBar("Deploy to PureWeb platform", m_pureWebTask.ToString() + ":: Uploading: " + m_appName + ".zip", progressValue))
                {
                    m_pureWebUploadCancelled = true;
                    UnityEngine.Debug.Log("Post build process & upload aborted");

                    if (m_process !=  null)  m_process.Kill();
                    if(m_thread !=  null) m_thread.Abort();

                    if(m_pureWewbPassword != null)
                    {
                        m_pureWewbPassword.Close();
                    }
                }
            }

            EditorUtility.ClearProgressBar();
        }

        public enum PureWebTaskEvent { Finished, PureWebNPMCheck, PureWebCLICheck, PureWebInstallCLI, PurewebConnectClientID, PurewebConnectClientSecret, PureWebListVersions, PureWebUpload, PureWebDelete, PureWebConfirmation }

        [System.Serializable]
        public class PureWebModel
        {
            public string id;
            public string version;
            public string name;
            public bool active;
        }
    }
}
#endif