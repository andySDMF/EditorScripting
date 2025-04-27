using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace BrandLab360.Editor
{
    public class CreateAssetBundles
    {
		//[MenuItem("BrandLab360/Assets/Build Asset Bundle")]
	/*	private static void ExportResource()
		{
			if (Selection.activeObject == null) return;

			string ext = Selection.activeObject.GetType().ToString().Contains("SceneAsset") ? "unity3d" : "prefab";
			string path = EditorUtility.SaveFilePanel("Save Resource", Application.streamingAssetsPath, Selection.activeObject.name, ext);

			if (path.Length != 0)
			{
				if(ext.Equals("unity3d"))
                {
					//this does not work anymore
					string resourceSourcePath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());

					string[] levels = new string[1] { resourceSourcePath };
					BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, EditorUserBuildSettings.activeBuildTarget);
				
				}
				else
                {
					// Build the resource file from the active selection.
					Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
					BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies, EditorUserBuildSettings.activeBuildTarget);
					Selection.objects = selection;
				}
			}
		}*/
	}
}
