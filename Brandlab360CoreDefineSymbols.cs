using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using System.IO.Compression;
using BrandLab360.Editor;
using UnityEditor.Build;

/// <summary>
/// Adds the given define symbols to PlayerSettings define symbols.
/// </summary>
[InitializeOnLoad]
public class Brandlab360CoreDefineSymbols : Editor
{
    enum PipelineType
    {
        Unsupported,
        BuiltInPipeline,
        UniversalPipeline,
        HDPipeline
    }

    /// <summary>
    /// Symbols that will be added to the editor
    /// </summary>
    public static readonly string[] Symbols = new string[] {
            "BRANDLAB360"
        };


    /// <summary>
    /// Used to store the list of packages
    /// </summary>
    private static ListRequest packageListRequest;
    private static AddRequest m_addPackageRequest;

    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    static Brandlab360CoreDefineSymbols()
    {
        // Auto Add the Define Symbols required for the Core package
        string definesString = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(Symbols.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbols(
            NamedBuildTarget.Standalone,
            string.Join(";", allDefines.ToArray()));

        // Add the layers and tags used by the Core package

        Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");

        if (asset != null && asset.Length > 0)
        {
            SerializedObject serializedObject = new SerializedObject(asset[0]);
            SerializedProperty layers = serializedObject.FindProperty("layers");
            SerializedProperty tags = serializedObject.FindProperty("tags");

            foreach (KeyValuePair<int, string> layerDefinition in CoreLayersTags.Layers)
            {
                var layerNumber = layerDefinition.Key;
                var layerName = layerDefinition.Value;

                addLayerAt(layers, layerNumber, layerName, false);
            }

            foreach(var tag in CoreLayersTags.Tags)
            {
                addTag(tags, tag);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        UpdateDefines();

        //need to check if the asset file for paths has been created
        BrandLab360.AppConstReferences appReferences = Resources.Load<BrandLab360.AppConstReferences>("AppConstReferences");

        if(appReferences == null)
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            appReferences = CreateInstance<BrandLab360.AppConstReferences>();
            AssetDatabase.CreateAsset(appReferences, "Assets/Resources/AppConstReferences.asset");
            AssetDatabase.SaveAssets();
        }

      /*  if (System.IO.Directory.Exists(Application.dataPath + "/com.brandlab360.core"))
        {
            AddSceneToBuildSettings("Assets/com.brandlab360.core/Runtime/Scenes/SampleSceneIntro");
            AddSceneToBuildSettings("Assets/com.brandlab360.core/Runtime/Scenes/SampleSceneAvatar");
        }
        else
        {
            AddSceneToBuildSettings("Packages/com.brandlab360.core/Runtime/Scenes/SampleSceneIntro");
            AddSceneToBuildSettings("Packages/com.brandlab360.core/Runtime/Scenes/SampleSceneAvatar");
        }*/

        OnCompilationCheck();
    }

    static void OnCompilationCheck()
    {
        packageListRequest = Client.List();
        EditorApplication.update += CheckCallback;
    }

    static void CheckCallback()
    {
        if (packageListRequest.IsCompleted)
        {
            bool corePackageExists = false;
            bool gltFastExists = false;

            if (packageListRequest.Status == StatusCode.Success)
            {
                foreach (var package in packageListRequest.Result)
                {
                    if (package.name.Equals("com.brandlab360.core"))
                    {
                        corePackageExists = true;
                    }

                    if (package.name.Equals("com.glTFast"))
                    {
                        gltFastExists = true;
                    }
                }
            }
            else if (packageListRequest.Status >= StatusCode.Failure)
            {
                Debug.Log(packageListRequest.Error.message);
            }

            Pipeline.AddDiffusionProfilesHDRP();

            if (corePackageExists)
            {
#if UNITY_WEBGL && UNITY_2021_1_OR_NEWER
                string zipAssetPath = "";

                if (!corePackageExists)
                {
                    zipAssetPath = Path.GetFullPath("Assets/com.brandlab360.core/Runtime/Photon/PhotonLibs/WebSocket/WebSocket.cs");
                }
                else
                {
                    zipAssetPath = Path.GetFullPath("Packages/com.brandlab360.core/Runtime/Photon/PhotonLibs/WebSocket/WebSocket.cs");
                }

                //check if exists first
                bool folderExists = AssetDatabase.IsValidFolder($"Assets/Plugins");
                string folder = Path.GetDirectoryName(zipAssetPath);

                if (!folderExists)
                {
                    string zipPackage = DeleteFilesForZip(folder);

                    if (!string.IsNullOrEmpty(zipPackage))
                    {
                        //unzip package
                        ZipFile.ExtractToDirectory(zipPackage, Application.dataPath);
                    }

                    AssetDatabase.Refresh();
                }
                else
                {
                    DeleteFilesForZip(folder);
                }
#endif


                //need to install GLTFast if package does not exist
                if(!gltFastExists)
                {
                  //  m_addPackageRequest = Client.Add("https://github.com/atteneder/glTFast.git");
                }

#if !UNITY_2021_1_OR_NEWER
                removeBuildLibs();
#endif
            }

            EditorApplication.update -= CheckCallback;
        }
    }

    private static string DeleteFilesForZip(string folder)
    {
        //need to delete the files within the websocket file
        string[] files = Directory.GetFiles(folder);
        string zipPackage = "";

        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Contains(".zip"))
            {
                File.Delete(files[i]);
            }
            else
            {
                if (!files[i].Contains(".meta"))
                {
                    zipPackage = files[i];
                }
            }
        }

        AssetDatabase.Refresh();

        return zipPackage;
    }


    static void removeBuildLibs()
    {
        // Remove lib files supporting furioos build upload on unsupoorted unity version
        var furioosPath = Path.GetFullPath("Packages/com.brandlab360.core/Editor/Build/Furioos/Libs/Flurl.dll");
        string folder = Path.GetDirectoryName(furioosPath);

        DeleteFilesForZip(folder);
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        editorBuildSettingsScenes.AddRange(EditorBuildSettings.scenes.ToList());

        EditorBuildSettingsScene exists = editorBuildSettingsScenes.FirstOrDefault(x => x.path.Equals(scenePath + ".unity"));

        if (exists == null)
        {
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath + ".unity", true));
        }

        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }

    /// <summary>
    /// Adds a Layer at specified index
    /// </summary>
    /// <param name="layers">layers property rom tag manager</param>
    /// <param name="index">index to set the layer</param>
    /// <param name="layerName">layer name to set</param>
    /// <param name="tryOtherIndex">whether to set on other index if taken</param>
    static void addLayerAt(SerializedProperty layers, int index, string layerName, bool tryOtherIndex = true)
    {
        // Skip if a layer with the name already exists.
        for (int i = 0; i < layers.arraySize; ++i)
        {
            if (layers.GetArrayElementAtIndex(i).stringValue == layerName)
            {
                return;
            }
        }

        // Extend layers if necessary
        if (index >= layers.arraySize)
            layers.arraySize = index + 1;

        // set layer name at index
        var element = layers.GetArrayElementAtIndex(index);
        if (string.IsNullOrEmpty(element.stringValue))
        {
            element.stringValue = layerName;
            Debug.Log("Added layer '" + layerName + "' at index " + index + ".");
        }
        else
        {
            Debug.LogWarning("Warning: Could not add layer at index " + index + " because there already is another layer '" + element.stringValue 
                + "'. BrandLab360 Core Package requires this layer set. Try removing the layer and restarting the Editor.");

            if (tryOtherIndex)
            {
                // Go up in layer indices and try to find an empty spot.
                for (int i = index + 1; i < 32; ++i)
                {
                    // Extend layers if necessary
                    if (i >= layers.arraySize)
                        layers.arraySize = i + 1;

                    element = layers.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(element.stringValue))
                    {
                        element.stringValue = layerName;
                        Debug.Log("Added layer '" + layerName + "' at index " + i + " instead of " + index + ".");
                        return;
                    }
                }

                Debug.LogError("Could not add layer " + layerName + " because there is no space left in the layers array.");
            }
        }
    }

    /// <summary>
    /// Adds a tag 
    /// </summary>
    /// <param name="tags">tags property in tag manager</param>
    /// <param name="tag">tag name to set</param>
    static void addTag(SerializedProperty tags, string tag)
    {
        for (int i = 0; i < tags.arraySize; ++i)
        {
            if (tags.GetArrayElementAtIndex(i).stringValue == tag)
            {
                return;
            }
        }

        Debug.Log("Adding Tag: " + tag);
        tags.InsertArrayElementAtIndex(0);
        tags.GetArrayElementAtIndex(0).stringValue = tag;
    }

    public static void UpdateDefines()
    {
        var pipeline = GetPipeline();

        if (pipeline == PipelineType.UniversalPipeline)
        {
            AddDefine("UNITY_PIPELINE_URP");
        }
        else
        {
            RemoveDefine("UNITY_PIPELINE_URP");
        }
        if (pipeline == PipelineType.HDPipeline)
        {
            AddDefine("UNITY_PIPELINE_HDRP");
        }
        else
        {
            RemoveDefine("UNITY_PIPELINE_HDRP");
        }
    }

    /// <summary>
    /// Returns the type of renderpipeline that is currently running
    /// </summary>
    /// <returns></returns>
    static PipelineType GetPipeline()
    {
#if UNITY_2019_1_OR_NEWER
        if (GraphicsSettings.renderPipelineAsset != null)
        {
            // SRP
            var srpType = GraphicsSettings.renderPipelineAsset.GetType().ToString();
            if (srpType.Contains("HDRenderPipelineAsset"))
            {
                return PipelineType.HDPipeline;
            }
            else if (srpType.Contains("UniversalRenderPipelineAsset") || srpType.Contains("LightweightRenderPipelineAsset"))
            {
                return PipelineType.UniversalPipeline;
            }
            else return PipelineType.Unsupported;
        }
#elif UNITY_2017_1_OR_NEWER
        if (GraphicsSettings.renderPipelineAsset != null) {
            // SRP not supported before 2019
            return PipelineType.Unsupported;
        }
#endif
        // no SRP
        return PipelineType.BuiltInPipeline;
    }

    /// <summary>
    /// Add a custom define
    /// </summary>
    /// <param name="define"></param>
    /// <param name="buildTargetGroup"></param>
    static void AddDefine(string define)
    {
        var definesList = GetDefines();
        if (!definesList.Contains(define))
        {
            definesList.Add(define);
            SetDefines(definesList);
        }
    }

    /// <summary>
    /// Remove a custom define
    /// </summary>
    /// <param name="_define"></param>
    /// <param name="_buildTargetGroup"></param>
    public static void RemoveDefine(string define)
    {
        var definesList = GetDefines();
        if (definesList.Contains(define))
        {
            definesList.Remove(define);
            SetDefines(definesList);
        }
    }

    public static List<string> GetDefines()
    {
       // var target = EditorUserBuildSettings.activeBuildTarget;
      //  var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(target);
        var defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
        return defines.Split(';').ToList();
    }

    public static void SetDefines(List<string> definesList)
    {
       // var target = EditorUserBuildSettings.activeBuildTarget;
      //  var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(target);
        var defines = string.Join(";", definesList.ToArray());
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, defines);
    }
}
