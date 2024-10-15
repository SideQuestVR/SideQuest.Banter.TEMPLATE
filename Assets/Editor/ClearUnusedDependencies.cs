using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ClearUnusedDependencies : EditorWindow
{
    [MenuItem("Banter/Tools/Clear Unused Dependencies")]
    public static void ClearUnusedDependenciesInScenes()
    {
        // Get all scenes in the project
        string[] allSceneGuids = AssetDatabase.FindAssets("t:Scene");

        foreach (string sceneGuid in allSceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);

            // Check if the scene is part of a read-only package
            if (AssetDatabase.IsOpenForEdit(scenePath, StatusQueryOptions.UseCachedIfPossible))
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                // Find all materials in the scene
                Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
                foreach (Material material in materials)
                {
                    // Clear unused textures from materials
                    SerializedObject materialSO = new SerializedObject(material);
                    SerializedProperty textureProperty = materialSO.FindProperty("m_SavedProperties.m_TexEnvs");
                    if (textureProperty != null && textureProperty.isArray)
                    {
                        for (int i = textureProperty.arraySize - 1; i >= 0; i--)
                        {
                            SerializedProperty element = textureProperty.GetArrayElementAtIndex(i);
                            if (element.FindPropertyRelative("second.m_Texture").objectReferenceValue == null)
                            {
                                textureProperty.DeleteArrayElementAtIndex(i);
                            }
                        }
                    }
                    materialSO.ApplyModifiedProperties();
                }

                // Optionally, clear unused components or objects if needed
                // Note: Implement additional checks and clearances as per your project requirements

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
            else
            {
                Debug.LogWarning($"Skipping read-only scene: {scenePath}");
            }
        }

        // Refresh the AssetDatabase
        AssetDatabase.Refresh();
        Debug.Log("Cleared unused dependencies in editable scenes.");
    }
}
