using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BrandLab360.Editor
{
    public class CreateAssetBundleLoaderEditor : CreateBaseEditor
    {
        //[MenuItem("BrandLab360/Core/AssetBundle/Loader")]
        public static void CreateAssetBundleLoader()
        {
            GameObject loader = CreateParentAndSelectObject("_ASSETBUNDLES", null);

            if(loader.GetComponent<LoadAssetBundles>() == null)
            {
                loader.AddComponent<LoadAssetBundles>();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Asset Bundle Loader allready exists in opened scene", "OK");
            }
        }
    }
}
