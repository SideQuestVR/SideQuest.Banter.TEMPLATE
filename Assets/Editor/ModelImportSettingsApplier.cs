using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ModelImportSettings))]
public class ModelImportSettingsEditor : Editor
{
    SerializedProperty sourceObjectProp;
    SerializedProperty targetObjectsProp;

    void OnEnable()
    {
        sourceObjectProp = serializedObject.FindProperty("sourceObject");
        targetObjectsProp = serializedObject.FindProperty("targetObjects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(sourceObjectProp);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(targetObjectsProp, new GUIContent("Target Objects"), true);

        if (GUILayout.Button("Apply Settings"))
        {
            ApplyImportSettings();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void ApplyImportSettings()
    {
        ModelImportSettings settings = (ModelImportSettings)target;

        if (settings.sourceObject == null)
        {
            Debug.LogError("Source object is not set.");
            return;
        }

        string sourcePath = AssetDatabase.GetAssetPath(settings.sourceObject);
        var sourceImporter = AssetImporter.GetAtPath(sourcePath) as ModelImporter;

        if (sourceImporter == null)
        {
            Debug.LogError("Source object is not a model.");
            return;
        }

        foreach (GameObject targetObject in settings.targetObjects)
        {
            if (targetObject != null)
            {
                string targetPath = AssetDatabase.GetAssetPath(targetObject);
                var targetImporter = AssetImporter.GetAtPath(targetPath) as ModelImporter;

                if (targetImporter != null)
                {
                    // Copying the 'Generate Lightmap UVs' setting
                    targetImporter.generateSecondaryUV = sourceImporter.generateSecondaryUV;

                    // Apply other settings here if needed

                    targetImporter.SaveAndReimport();
                }
                else
                {
                    Debug.LogWarning($"Object at {targetPath} is not a model.");
                }
            }
        }

        Debug.Log("Import settings applied.");
    }
}
