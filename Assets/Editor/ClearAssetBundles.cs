using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ClearAssetBundles : EditorWindow
{
    [MenuItem("Banter/Clear All Asset Bundles")]
    public static void ClearAllAssetBundles()
    {
        // Fetch all asset paths in the project
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();

        // Initialize a list to hold asset bundle names
        List<string> assetBundleNames = new List<string>();

        // Iterate through all asset paths to collect asset bundle names
        foreach (string assetPath in allAssetPaths)
        {
            string assetBundleName = AssetDatabase.GetImplicitAssetBundleName(assetPath);
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                assetBundleNames.Add(assetBundleName);
            }
        }

        // Iterate through all asset bundle names to remove them
        foreach (string assetBundleName in assetBundleNames)
        {
            AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
        }

        // Delete asset bundle files from "AssetBundles" directory
        string assetBundleDirectory = "Assets/AssetBundles";
        if (Directory.Exists(assetBundleDirectory))
        {
            Directory.Delete(assetBundleDirectory, true);
            File.Delete(assetBundleDirectory + ".meta");
        }

        // Refresh and update the asset database
        AssetDatabase.Refresh();
		
		AssetDatabase.RemoveUnusedAssetBundleNames();
		AssetDatabase.Refresh();
        Debug.Log("Cleared all asset bundles.");
		
		
    }
}
