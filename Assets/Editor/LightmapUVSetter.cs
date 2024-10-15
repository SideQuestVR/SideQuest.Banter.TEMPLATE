using UnityEngine;
using UnityEditor;
using System.IO;

public class LightmapUVSetter : EditorWindow
{
    private string folderPath = "Assets/PolygonPrototype/Models"; // Default folder path

    [MenuItem("Banter/Tools/Set Lightmap UVs in Folder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LightmapUVSetter));
    }

    void OnGUI()
    {
        GUILayout.Label("Set Generate Lightmap UVs", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

        if (GUILayout.Button("Set Lightmap UVs"))
        {
            SetLightmapUVs();
        }
    }

    private void SetLightmapUVs()
    {
        string fullPath = Path.Combine(Application.dataPath, folderPath.Replace("Assets/", ""));
        if (!Directory.Exists(fullPath))
        {
            Debug.LogError("Folder not found: " + fullPath);
            return;
        }

        string[] fileEntries = Directory.GetFiles(fullPath, "*.fbx", SearchOption.AllDirectories);
        foreach (string fileName in fileEntries)
        {
            string assetPath = "Assets" + fileName.Replace(Application.dataPath, "").Replace('\\', '/');
            ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;

            if (importer != null)
            {
                importer.generateSecondaryUV = true;
                importer.SaveAndReimport();
            }
        }

        Debug.Log("Lightmap UVs set for all models in folder: " + folderPath);
    }
}
