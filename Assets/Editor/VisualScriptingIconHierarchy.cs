using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[InitializeOnLoad]
public static class VisualScriptingIconHierarchy
{
    private static Texture2D scriptMachineIcon;
    private static Texture2D stateMachineIcon;

    // Static constructor to subscribe to the hierarchy drawing event
    static VisualScriptingIconHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        
        // Load icons from the Gizmos folder
        scriptMachineIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Gizmos/ScriptMachineIcon.png", typeof(Texture2D));
        stateMachineIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Gizmos/StateMachineIcon.png", typeof(Texture2D));
    }

    // Method that gets called for every object in the hierarchy
    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (obj != null)
        {
            // Check if the GameObject has a ScriptMachine or StateMachine component
            if (obj.GetComponent<ScriptMachine>() != null)
            {
                DrawIcon(selectionRect, scriptMachineIcon);
            }
            else if (obj.GetComponent<StateMachine>() != null)
            {
                DrawIcon(selectionRect, stateMachineIcon);
            }
        }
    }

    // Method to draw the icon in the hierarchy
    private static void DrawIcon(Rect selectionRect, Texture2D icon)
    {
        if (icon != null)
        {
            Rect rect = new Rect(selectionRect.xMax - 20, selectionRect.y, 18, 18);
            GUI.DrawTexture(rect, icon);
        }
    }
}
