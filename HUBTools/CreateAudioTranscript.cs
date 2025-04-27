using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;
using System;

namespace BrandLab360.Editor
{
    public class CreateAudioTranscript : CreateBaseEditor
    {
        private const string json = @"{'timings':[{'start':'','end':'','text':''},{'start':'','end':'','text':''}]}";

        //[MenuItem("BrandLab360/Audio/Transcript/Create From Audio-Video")]
        public static void CreateFromAudio()
        {
            //show window to define chair options
            AudioTranscriptEditorWindow window = (AudioTranscriptEditorWindow)EditorWindow.GetWindow(typeof(AudioTranscriptEditorWindow));
            window.maxSize = new Vector2(500f, 500f);
            window.minSize = window.maxSize;
            window.Show();
        }

        //[MenuItem("BrandLab360/Audio/Transcript/Create From SRT")]
        public static void CreateFromSRT()
        {
            //show window to define chair options
            SRTTranscriptEditorWindow window = (SRTTranscriptEditorWindow)EditorWindow.GetWindow(typeof(SRTTranscriptEditorWindow));
            window.maxSize = new Vector2(800f, 500f);
            window.minSize = window.maxSize;
            window.Show();
        }

        //[MenuItem("BrandLab360/Audio/Open VEED.io")]
        public static void OpenVeed()
        {
            Application.OpenURL("https://www.veed.io/workspaces/97779c6a-ad7b-49d0-a305-1731282d8fe0/home");
        }

        //[MenuItem("BrandLab360/Audio/Transcript/Create JSON File")]
        public static void CreateJson()
        {
            UnityEngine.Object txt = (TextAsset)GetAsset<TextAsset>("Assets/com.brandlab360.core/Runtime/Audio/SubtitleTimingJson.txt");

            if (txt != null)
            {
                if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                if (!AssetDatabase.IsValidFolder($"Assets/Resources/Audio"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "Audio");
                }

                if (!AssetDatabase.IsValidFolder($"Assets/Resources/Audio/Transcripts"))
                {
                    AssetDatabase.CreateFolder("Assets/Resources/Audio", "Transcripts");
                }

                File.WriteAllText(Application.dataPath + "/Resources/Audio/Transcripts/AudioTranscriptJson_" + GUID.Generate().ToString(), json);
            }   
        }
    }

    public class SRTTranscriptEditorWindow : EditorWindow
    {
        private AppSettings cache;
        private TextAsset m_asset;
        private string requestOutput = "";

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
        }

        protected void OnDisable()
        {
            EditorUtility.ClearProgressBar();
        }

        protected void OnGUI()
        {
            DrawTitle();
            DrawSRT();
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

            EditorGUILayout.LabelField("SRT Transcript Convert", EditorStyles.boldLabel);
            EditorGUILayout.Space();
        }

        private void DrawSRT()
        {
            m_asset = (TextAsset)EditorGUILayout.ObjectField(new GUIContent("SRT File", ""), m_asset, typeof(TextAsset), true);

            if (GUILayout.Button("Process") && m_asset != null)
            {
                Timing output = new Timing();

                char[] delims = new[] { '\r', '\n' };
                string[] timings = m_asset.text.Split(delims, StringSplitOptions.RemoveEmptyEntries);

       
                for(int i = 0; i < timings.Length;)
                {
                    int n;

                    if (int.TryParse(timings[i], out n))
                    {
                        SRTTiming timing = new SRTTiming();
                        string[] times = timings[i + 1].Split(" --> ");

                        timing.start = (float)TimeSpan.Parse(times[0].Split(',')[0]).TotalSeconds;
                        timing.end = (float)TimeSpan.Parse(times[1].Split(',')[0]).TotalSeconds;

                        timing.text = timings[i + 2];
                        i += 3;
                        output.timings.Add(timing);
                    }
                }

                requestOutput = JsonUtility.ToJson(output);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Transcript");
            GUIStyle style = new GUIStyle();
            style.wordWrap = false;
            style.clipping = TextClipping.Clip;
            style.active.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.normal.textColor = Color.white;
            requestOutput = EditorGUILayout.TextArea(requestOutput, style, GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Export"))
            {
                if (!string.IsNullOrEmpty(requestOutput) && m_asset != null)
                {
                    Export(m_asset.name);
                }
            }
        }

        private void Export(string name)
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Audio"))
            {
                AssetDatabase.CreateFolder("Assets", "Audio");
            }

            if (!AssetDatabase.IsValidFolder($"Assets/Audio/Transcripts"))
            {
                AssetDatabase.CreateFolder("Assets/Audio", "Transcripts");
            }

            File.WriteAllText(Application.dataPath + "/Audio/Transcripts/" + name + ".txt", requestOutput);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        [System.Serializable]
        private class Timing
        {
            public List<SRTTiming> timings = new List<SRTTiming>();
        }

        [System.Serializable]
        private class SRTTiming
        {
            public float start;
            public float end;
            public string text;
        }
    }

    public class AudioTranscriptEditorWindow : EditorWindow
    {
        private AppSettings cache;
        private AudioClip m_audioSRC;
        private string ApiKey = "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";
        private string domain = "https://api.openai.com/";
        private string version = "v1/audio/transcriptions";
        private string model = "whisper-1";
        private SourceType m_srcType = SourceType._Audio;
        private VideoClip m_videoClip;
        private string requestOutput = "";
        private string m_currentFileName = "";

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
        }

        protected void OnDisable()
        {
            EditorUtility.ClearProgressBar();
        }

        protected void OnGUI()
        {
            DrawTitle();
            DrawAudio();
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

            EditorGUILayout.LabelField("Audio Transcript Convert", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Formats: mp3, mp4 (ATM)", EditorStyles.miniBoldLabel);
            EditorGUILayout.Space();
        }

        private void DrawAudio()
        {
            m_srcType = (SourceType)EditorGUILayout.EnumPopup(new GUIContent("Source Type", ""), m_srcType);

            if(m_srcType.Equals(SourceType._Audio))
            {
                m_audioSRC = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Audio Clip", ""), m_audioSRC, typeof(AudioClip), true);
            }
            else
            {
                m_videoClip = (VideoClip)EditorGUILayout.ObjectField(new GUIContent("Video Clip", ""), m_videoClip, typeof(VideoClip), true);
            }

            if (GUILayout.Button("Process"))
            {
                if (m_srcType.Equals(SourceType._Audio))
                {
                    if (m_audioSRC != null)
                    {
                        GetTranscript();
                    }
                }
                else
                {
                    if (m_videoClip != null)
                    {
                        GetTranscript();
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Transcript");
            GUIStyle style = new GUIStyle();
            style.wordWrap = true;
            style.active.textColor = Color.white;
            style.hover.textColor = Color.white;
            style.normal.textColor = Color.white;
            requestOutput = EditorGUILayout.TextArea(requestOutput, style, GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Export"))
            {
                if (!string.IsNullOrEmpty(requestOutput) && !string.IsNullOrEmpty(m_currentFileName))
                {
                    Export(m_currentFileName);
                }
            }
        }

        private void RequestCallback(string str)
        {
            requestOutput = str;
        }

        private async void GetTranscript()
        {
            TranscriptRequest request = new TranscriptRequest();

            if (m_srcType.Equals(SourceType._Audio))
            {
                request.file = Application.dataPath + AssetDatabase.GetAssetPath(m_audioSRC).Substring(6);
            }
            else
            {
                request.file = Application.dataPath + AssetDatabase.GetAssetPath(m_videoClip).Substring(6);
            }

            m_currentFileName = CoreUtilities.GetFilename(request.file);
            request.model = model;

            TranscriptProcesser process = new TranscriptProcesser();
            bool success = await process.Start(request, domain, version, ApiKey, RequestCallback);
        }

        private void Export(string name)
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Audio"))
            {
                AssetDatabase.CreateFolder("Assets", "Audio");
            }

            if (!AssetDatabase.IsValidFolder($"Assets/Audio/Transcripts"))
            {
                AssetDatabase.CreateFolder("Assets/Audio", "Transcripts");
            }

            File.WriteAllText(Application.dataPath + "/Audio/Transcripts/" + name + ".txt", requestOutput);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        private class TranscriptProcesser
        {
            public async Task<bool> Start(TranscriptRequest data, string domain, string version, string ApiKey, System.Action<string> callback)
            {
                return await ProcessRequest(data, domain, version, ApiKey, callback);
            }

            private async Task<bool> ProcessRequest(TranscriptRequest data, string domain, string version, string ApiKey, System.Action<string> callback)
            {
                using (UnityWebRequest DL = UnityWebRequest.Get(data.file))
                {
                    DL.method = UnityWebRequest.kHttpVerbGET;
                    DL.downloadHandler = new DownloadHandlerBuffer();

                    EditorUtility.DisplayProgressBar("Proccessing Transcript. Please Wait...", "Prcoessing", 0.0f);
                    await DL.SendWebRequest();

                    if (DL.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Could not find file");
                        EditorUtility.ClearProgressBar();
                    }
                    else
                    {
                        if (DL.responseCode == 200)
                        {
                            WWWForm form = new WWWForm();
                            string fType = (CoreUtilities.GetExtension(data.file).Equals(".mp3")) ? "audio/mp3" : "video/mp4";

                            byte[] byteArray = File.ReadAllBytes(data.file);
                            form.AddBinaryData("file", DL.downloadHandler.data, data.file, fType);
                            form.AddField("model", data.model);
                            var uri = domain + version;

                            using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
                            {
                                var bearer = "Bearer " + ApiKey;
                                request.SetRequestHeader("Authorization", bearer);
                                request.downloadHandler = new DownloadHandlerBuffer();
                                request.timeout = 1 * 60 * 60;

                                EditorUtility.DisplayProgressBar("Proccessing Transcript. Please Wait...", "Prcoessing", 0.0f);
                                await request.SendWebRequest();

                                if (request.result != UnityWebRequest.Result.Success)
                                {
                                    Debug.Log("Error OpenAI Transcriptions: " + request.error + request.downloadHandler.text);
                                    EditorUtility.ClearProgressBar();
                                    callback.Invoke(request.error);
                                }
                                else
                                {
                                    if (request.responseCode == 200)
                                    {
                                        float time = 0.0f;
                                        float percentage = 0.0f;

                                        while (percentage < 1.0f)
                                        {
                                            time += Time.deltaTime;
                                            percentage = time / 0.2f;
                                            EditorUtility.DisplayProgressBar("Proccessing Transcript. Please Wait...", "Prcoessing", percentage);

                                            await Task.Yield();
                                        }

                                        EditorUtility.ClearProgressBar();

                                        Debug.Log("OpenAI Completions success:" + request.downloadHandler.text);

                                        var jsonData = new JSONObject(request.downloadHandler.text);
                                        string temp = jsonData.GetField("text").stringValue[0].ToString().ToUpper();
                                        callback.Invoke(temp + jsonData.GetField("text").stringValue.Substring(1));

                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }

        [System.Serializable]
        private enum SourceType { _Audio, _Video }
    }
}
